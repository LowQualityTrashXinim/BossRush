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
	public override void PostUpdateInvasions() {
		if (Trial == null) {
			return;
		}
		if (Trial.IsABattleTrial) {
			BattleTrialUpdate();
		}
	}
	private void SpawnTrialNPC(int npcType, Rectangle spawnRect) {
		NPC newnpc = null;
		if (Trial.UsesCustomSpawn(CurrentWave, npcType, Trial_NPCPool[npcType], spawnRect, out Vector2 pos)) {
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
						newnpc = NPC.NewNPCDirect(NPC.GetSource_None(), position.ToVector2() * 16, npcType);
						break;
					}
					failsafe++;
				}
			}
			if (newnpc == null) {
				newnpc = NPC.NewNPCDirect(NPC.GetSource_None(), Main.rand.NextVector2FromRectangle(spawnRect), npcType);
			}
		}
		newnpc.GetGlobalNPC<TrialGlobalNPC>().SpawnedByTrial = true;
		Trial_NPC.Add(newnpc);
		Trial_NPCPool[npcType] = Math.Clamp(Trial_NPCPool[npcType] - 1, 0, Main.maxNPCs);
		Trial_TotalRemainingNPCs = Trial_NPCPool.Values.Sum();
	}
	public static ModTrial Trial = null;
	public static List<NPC> Trial_NPC = new List<NPC>();
	public static int CurrentWave = 0;
	public static int NextWave = 0;
	private int Trial_NPCStarterSpawnlimit = 0;
	private int Trial_NPCActiveLimit = 0;
	private int Trial_TotalRemainingNPCs = 0;
	private int Trial_DelaySpawning = 0;
	private int Trial_SpawnTimer = 0;

	private Dictionary<int, int> Trial_NPCPool = new Dictionary<int, int>();
	private void ResetTrial() {
		Trial_NPC.Clear();
		Trial = null;
		NextWave = 0;
		CurrentWave = 0;
		Trial_TotalRemainingNPCs = 0;
		Trial_NPCActiveLimit = 0;
		Trial_NPCStarterSpawnlimit = 0;
		Trial_DelaySpawning = 0;
		Trial_SpawnTimer = 0;
	}
	private void TrialNPCManage() {
		if (Trial_NPC == null) {
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
			for (int i = Trial_NPC.Count - 1; i >= 0; i--) {
				NPC npc = Trial_NPC[i];
				if (npc.active && npc.life > 0) {
					continue;
				}
				Trial_NPC.RemoveAt(i);
				if (Trial_NPC.Count > Math.Max(Trial_NPCActiveLimit, 1) && Trial_SpawnTimer <= Trial_DelaySpawning) {
					Trial_SpawnTimer++;
					break;
				}
				Trial_SpawnTimer = 0;
				if (Trial_NPC.Count <= Math.Min(Trial_NPCActiveLimit, Trial_TotalRemainingNPCs) && !(Trial_NPC.Count == Trial_TotalRemainingNPCs && Trial_TotalRemainingNPCs == 0)) {
					Rectangle spawnRect = Trial.TrialSize(Main.LocalPlayer.Center);
					int npcType = Main.rand.Next(Trial_NPCPool.Keys.ToArray());

					SpawnTrialNPC(npcType, spawnRect);
				}
			}
		}
		if (Trial_NPC.Count < 1) {
			NextWave++;
			if (NextWave > Trial.WaveAmount()) {
				Trial.TrialReward(Main.LocalPlayer.GetSource_Misc("trial_reward"), Main.LocalPlayer);
				ResetTrial();
			}
			else {
				Trial.TrialNPCSpawnData(out int start, out int activelimit, out int delay);
				Trial_ArenaSize = Trial.TrialSize(Main.LocalPlayer.Center);
				Trial_NPCStarterSpawnlimit = start;
				Trial_NPCActiveLimit = activelimit;
				Trial_DelaySpawning = delay;
				Trial_NPCPool = Trial.NPCpool();
			}
		}
	}
	private void StartTrial() {
		if (CurrentWave != NextWave) {
			CurrentWave = Math.Min(CurrentWave + 1, NextWave);
			Rectangle spawnRect = Trial.TrialSize(Main.LocalPlayer.Center);
			int npcCounter = 0;
			do {
				foreach (var npcType in Trial_NPCPool.Keys) {
					if (Trial_NPCPool[npcType] > 0) {
						SpawnTrialNPC(npcType, spawnRect);
						npcCounter++;
					}
					if (npcCounter >= Trial_NPCStarterSpawnlimit) {
						break;
					}
				}
			} while (npcCounter < Math.Max(Trial_NPCStarterSpawnlimit, 1));
		}
	}
	private Rectangle Trial_ArenaSize = new();
	private void BattleTrialUpdate() {
		for (int i = 0; i < 300; i++) {
			Dust dust = Dust.NewDustDirect(Main.rand.NextVector2RectangleEdge(Trial_ArenaSize), 0, 0, DustID.WhiteTorch);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
		}
		TrialNPCManage();
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
	/// Keys : NPC type
	/// Values : Amount of npc
	/// </summary>
	/// <returns></returns>
	public virtual Dictionary<int, int> NPCpool() => new();
	public virtual Rectangle TrialSize(Vector2 TrialStartPosition) => new Rectangle((int)TrialStartPosition.X, (int)TrialStartPosition.Y, 0, 0);
	public virtual bool UsesCustomSpawn(int currentWave, int currentNPCtype, int currentNPCamount, Rectangle size, out Vector2 pos) {
		pos = Vector2.Zero;
		return false;
	}
	public virtual void TrialNPCSpawnData(out int StartingSpawnAmount, out int ActiveLimit, out int delaySpawn) {
		StartingSpawnAmount = 10;
		ActiveLimit = 12;
		delaySpawn = 0;
	}

	public virtual void TrialReward(IEntitySource source, Player player) { }
}
