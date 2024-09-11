using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.TrialSystem;

public class TrialGlobalNPC : GlobalNPC {
	public bool SpawnedByTrial = false;
	public override bool InstancePerEntity => base.InstancePerEntity;
}
internal class TrialModSystem : ModSystem {
	private static readonly List<TrialData> _trial = new();
	public static int TotalCount => _trial.Count;
	public static int Register(TrialData trial) {
		ModTypeLookup<TrialData>.Register(trial);
		_trial.Add(trial);
		return _trial.Count - 1;
	}
	public static TrialData GetTrial(int type) {
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
		NPC newnpc;
		if (Trial.UsesCustomSpawn(CurrentWave, npcType, Trial_NPCPool[npcType], spawnRect, out Vector2 pos)) {
			newnpc = NPC.NewNPCDirect(NPC.GetSource_None(), pos, npcType);
		}
		else {
			newnpc = NPC.NewNPCDirect(NPC.GetSource_None(), Main.rand.NextVector2FromRectangle(spawnRect), npcType);
		}
		newnpc.GetGlobalNPC<TrialGlobalNPC>().SpawnedByTrial = true;
		Trial_NPC.Add(newnpc);
		Trial_NPCPool[npcType] = Math.Clamp(Trial_NPCPool[npcType] - 1, 0, Main.maxNPCs);
		Trial_TotalRemainingNPCs = Trial_NPCPool.Values.Sum();
	}
	public static TrialData Trial = null;
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
		Trial_NPC = null;
		Trial = null;
		NextWave = 0;
		CurrentWave = 0;
		Trial_TotalRemainingNPCs = 0;
		Trial_NPCActiveLimit = 0;
		Trial_NPCStarterSpawnlimit = 0;
		Trial_DelaySpawning = 0;
	}
	private void TrialNPCManage() {
		if (Trial_NPC == null) {
			Trial_NPC = Main.npc.Where(npc => npc.GetGlobalNPC<TrialGlobalNPC>().SpawnedByTrial).ToList();
			return;
		}
		else {
			for (int i = Trial_NPC.Count - 1; i >= 0; i--) {
				NPC npc = Trial_NPC[i];
				if (npc.active && npc.life > 0) {
					continue;
				}
				Trial_NPC.Remove(npc);
				if (Trial_NPC.Count > Math.Max(Trial_NPCActiveLimit, 1) && Trial_SpawnTimer <= Trial_DelaySpawning) {
					Trial_SpawnTimer++;
					break;
				}
				Trial_SpawnTimer = 0;
				if (Trial_NPC.Count <= Math.Min(Trial_NPCActiveLimit, Trial_TotalRemainingNPCs)) {
					Rectangle spawnRect = Trial.TrialSize(Main.LocalPlayer.Center);
					int npcType = Main.rand.Next(Trial_NPCPool.Keys.ToArray());

					SpawnTrialNPC(npcType, spawnRect);
				}
			}
		}
		if (Trial_NPC.Count < 1) {
			NextWave++;
			if (NextWave > Trial.WaveAmount()) {
				ResetTrial();
			}
			else {
				Trial.TrialNPCSpawnData(out int start, out int activelimit, out int delay);
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
	private void BattleTrialUpdate() {
		TrialNPCManage();
		StartTrial();
	}
}
public abstract class TrialData : ModType {
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
	/// <summary>
	/// This by default set to 12
	/// </summary>
	/// <returns></returns>
	public virtual int ActiveLimit() => 12;
	public virtual bool UsesCustomSpawn(int currentWave, int currentNPCtype, int currentNPCamount, Rectangle size, out Vector2 pos) {
		pos = Vector2.Zero;
		return false;
	}
	public virtual void TrialNPCSpawnData(out int StartingSpawnAmount, out int ActiveLimit, out int delaySpawn) {
		StartingSpawnAmount = 10;
		ActiveLimit = 12;
		delaySpawn = 0;
	}
}
