using System;
using Terraria;
using System.Linq;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace BossRush.Common.Systems.TrialSystem;

public class TrialGlobalNPC : GlobalNPC {
	public bool SpawnedByTrial = false;
	public override bool InstancePerEntity => true;
}
internal class TrialModSystem : ModSystem {
	private static readonly List<ModTrial> _trial = new();
	public static int TotalCount => _trial.Count;
	public static int Register(ModTrial trial) {
		ModTypeLookup<ModTrial>.Register(trial);
		_trial.Add(trial);
		return _trial.Count - 1;
	}
	public static ModTrial GetTrial(int type) {
		return type >= 0 && type < _trial.Count ? _trial[type] : null;
	}
	public override void OnModLoad() {
		Trial = null;
		Trial_NPC = new List<NPC>();
		CurrentWave = 0;
		NextWave = 0;
	}
	public override void OnModUnload() {
		ResetTrial();
		Trial_NPC = null;
	}
	public override void PostDrawTiles() {
		//Main.spriteBatch.Begin();



		//Main.spriteBatch.End();
	}
	public static void SetTrial(int TrialID, Vector2 activatePosition) {
		Trial = GetTrial(Math.Clamp(TrialID, 0, TotalCount));
		Trial_StartPos = activatePosition;
	}
	public override void PostUpdateInvasions() {
		//Ensure trial will only start if trial data is not null
		if (Trial == null) {
			return;
		}
		//Check the trial type
		if (Trial.IsABattleTrial) {
			//Start trial 
			BattleTrialUpdate();
		}
	}
	private void SpawnTrialNPC(int npcType, Rectangle spawnRect) {
		NPC newnpc = null;
		if (Trial.UsesCustomSpawn(CurrentWave, npcType, npcType, spawnRect, out Vector2 pos)) {
			newnpc = NPC.NewNPCDirect(new EntitySource_Misc("spawned_Trial"), pos, npcType);
		}
		else {
			int failsafe = 0;
			while (failsafe <= 999) {
				Point position = new Point(
					Main.rand.Next(spawnRect.Left, spawnRect.Right + 1) / 16,
					Main.rand.Next(spawnRect.Top, spawnRect.Bottom + 1) / 16);
				if (WorldGen.TileEmpty(position.X / 16, position.Y / 16)) {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (WorldGen.TileEmpty(position.X + offsetX, position.Y + offsetY)) {
								pass++;
							}
						}
					}
					if (pass >= 8) {
						newnpc = NPC.NewNPCDirect(new EntitySource_Misc("spawned_Trial"), position.ToVector2() * 16, npcType);
						break;
					}
					failsafe++;
				}
			}
			if (newnpc == null) {
				newnpc = NPC.NewNPCDirect(new EntitySource_Misc("spawned_Trial"), Main.rand.NextVector2FromRectangle(spawnRect), npcType);
			}
		}
		newnpc.GetGlobalNPC<TrialGlobalNPC>().SpawnedByTrial = true;
		Trial_NPC.Add(newnpc);
	}
	public static ModTrial Trial = null;
	public static List<NPC> Trial_NPC = new List<NPC>();
	public static int CurrentWave = 0;
	public static int NextWave = 0;
	private int Trial_TotalRemainingNPCs = 0;
	private static Vector2 Trial_StartPos = Vector2.Zero;

	private List<int> Trial_NPCPool = new();
	private void ResetTrial() {
		Trial_NPC.Clear();
		Trial = null;
		NextWave = 0;
		CurrentWave = 0;
		Trial_TotalRemainingNPCs = 0;
	}
	private void TrialNPCManage() {
		//Check if list Trial NPC is null or not, but wtf why we are doing this ?
		if (Trial_NPC == null) {
			//idk what the hell we was thinking here
			Trial_NPC = new();
			foreach (NPC npc in Main.ActiveNPCs) {
				if (npc.TryGetGlobalNPC(out TrialGlobalNPC globalNPC)) {
					if (globalNPC.SpawnedByTrial) {
						Trial_NPC.Add(npc);
					}
				}
			}
			return;
		}
		else {
			//Do a reverse iteration so that we can remove element safely
			for (int i = Trial_NPC.Count - 1; i >= 0; i--) {
				NPC npc = Trial_NPC[i];
				//Check if the NPC in the question is still active or not
				if (npc.active && npc.life > 0) {
					//Since the NPC is still alive and active, we move on
					continue;
				}
				//Since the NPC is dead, we remove them
				Trial_NPC.RemoveAt(i);
			}
		}
		//Checking if the Trial NPC is empty, if it is not, do nothing
		if (Trial_NPC.Count < 1) {
			//Since the trial NPC is empty, we are moving to new wave if possible
			NextWave++;
			//If new wave is not possible, that mean we have reached the end of the wave
			if (NextWave > Trial.WaveAmount()) {
				//Spawn reward for player
				Trial.TrialReward(Main.LocalPlayer.GetSource_Misc("trial_reward"), Main.LocalPlayer);
				//Reset the trial
				ResetTrial();
			}
			else {
				//Resize the trial and then restart trial NPC pool
				//This is here as a fail safe in case something stupid happen
				if (Trial_StartPos == Vector2.Zero) {
					Trial_StartPos = Main.LocalPlayer.Center;
				}
				Trial_ArenaSize = Trial.TrialSize(Trial_StartPos);
				Trial_NPCPool.Clear();
				Trial_NPCPool = Trial.NPCpool(NextWave);
			}
		}
	}
	private void StartTrial() {
		if (CurrentWave != NextWave) {
			CurrentWave = Math.Min(CurrentWave + 1, NextWave);
			Rectangle spawnRect = Trial_ArenaSize;
			foreach (var npcType in Trial_NPCPool) {
				if (npcType > 0) {
					SpawnTrialNPC(npcType, spawnRect);
				}
			}
		}
	}
	private Rectangle Trial_ArenaSize = new();
	private void BattleTrialUpdate() {
		//Show border, temporary solution, not optimal at all cause we are relying on dust to draw border
		for (int i = 0; i < 300; i++) {
			Dust dust = Dust.NewDustDirect(Main.rand.NextVector2RectangleEdge(Trial_ArenaSize), 0, 0, DustID.WhiteTorch, Scale: 2);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
		}
		//Manage trial NPC
		//This won't actually do anything until Start trial method is run
		//However this method is still required to be run before StartTrial method as it initialize wave
		TrialNPCManage();
		//Start trial
		StartTrial();
	}
}
public abstract class ModTrial : ModType {
	public string TrialRoom = null;
	public bool IsABattleTrial = true;
	public int Type { get; private set; }
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = TrialModSystem.Register(this);
	}
	public virtual int WaveAmount() => 0;
	/// <summary>
	/// Keys : NPC type<br/>
	/// Values : Amount of npc<br/>
	/// <br/>
	/// Use this to set amount of specific NPC can be spawned
	/// </summary>
	/// <returns></returns>
	public virtual List<int> NPCpool(int currentWave) => new();
	//Set this trial size so that it can spawn correctly
	public virtual Rectangle TrialSize(Vector2 TrialStartPosition) => new Rectangle((int)TrialStartPosition.X, (int)TrialStartPosition.Y, 0, 0);
	/// <summary>
	/// This will enable developer to customized how their NPC can be spawned
	/// </summary>
	/// <param name="currentWave">The current wave of the trial</param>
	/// <param name="currentNPCtype">The current selected NPC type</param>
	/// <param name="currentNPCamount">The current amount of specified NPC</param>
	/// <param name="size">The size of the trial room ( included trial room position )</param>
	/// <param name="pos">Position of <paramref name="currentNPCtype"/> which will be spawned</param>
	/// <returns>
	/// <b>True</b> to force the system to use custom spawn<br/>
	/// <b>False</b> to use system default
	/// </returns>
	public virtual bool UsesCustomSpawn(int currentWave, int currentNPCtype, int currentNPCamount, Rectangle size, out Vector2 pos) {
		pos = Vector2.Zero;
		return false;
	}

	public virtual void TrialReward(IEntitySource source, Player player) { }
}
