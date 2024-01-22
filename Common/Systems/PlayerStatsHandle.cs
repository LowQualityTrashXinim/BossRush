using System;
using Terraria;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Items.Chest;
using Terraria.UI;
using Terraria.GameContent.UI.States;
using System.Reflection;

namespace BossRush.Common.Systems;
public class PlayerStatsHandle : ModPlayer {
	public float AuraRadius = 300f;
	/// <summary>
	/// This only keep track of current session
	/// </summary>
	public int CardTracker = 0;
	public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();
	public const int maxStatCanBeAchieved = 9999;

	public float MeleeDMG = 0;
	public float UpdateMelee = 0;

	public float RangeDMG = 0;
	public float UpdateRange = 0;

	public float MagicDMG = 0;
	public float UpdateMagic = 0;

	public float SummonDMG = 0;
	public float UpdateSummon = 0;

	public float Movement = 0;
	public float UpdateMovement = 0;

	public float JumpBoost = 0;
	public float UpdateJumpBoost = 0;

	public int HPMax = 0;
	public int UpdateHPMax = 0;

	public int HPRegen = 0;
	public int UpdateHPRegen = 0;

	public int ManaMax = 0;
	public int UpdateManaMax = 0;

	public int ManaRegen = 0;
	public int UpdateManaRegen = 0;

	public int DefenseBase = 0;
	public int UpdateDefenseBase = 0;

	public float DamagePure = 0;
	public float UpdateDamagePure = 0;

	public int CritStrikeChance = 0;
	public int UpdateCritStrikeChance = 0;

	public float Thorn = 0;
	public float UpdateThorn = 0;

	public float CritDamage = 0;
	public float UpdateCritDamage = 0;

	public float DefenseEffectiveness = 0;
	public float UpdateDefEff = 0;

	public int DropAmountIncrease = 0;
	public int UpdateDropAmount = 0;

	public int MinionSlot = 0;
	public int UpdateMinion = 0;

	public int SentrySlot = 0;
	public int UpdateSentry = 0;

	public int CardLuck = 0;

	public StatModifier DebuffTime = new StatModifier();
	public StatModifier BuffTime = new StatModifier();

	//public float LuckIncrease = 0;

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.CritDamage += Math.Clamp(UpdateCritDamage + CritDamage, -1, 999999);
	}
	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
		health = StatModifier.Default;
		mana = StatModifier.Default;

		health.Base = Math.Clamp(UpdateHPMax + HPMax + health.Base, -100, maxStatCanBeAchieved);
		mana.Base = Math.Clamp(UpdateManaMax + ManaMax + mana.Base, -20, maxStatCanBeAchieved);
	}
	public override void PostUpdate() {
		ChestLoot.amountModifier = Math.Clamp(UpdateDropAmount + DropAmountIncrease + ChestLoot.amountModifier, 0, maxStatCanBeAchieved);
	}
	public override void PostHurt(Player.HurtInfo info) {
		base.PostHurt(info);
		if (info.PvP) {
			return;
		}
	}
	public override void ResetEffects() {
		Player.GetDamage(DamageClass.Melee) += Math.Clamp(UpdateMelee + MeleeDMG, 0, maxStatCanBeAchieved);
		Player.GetDamage(DamageClass.Ranged) += Math.Clamp(UpdateRange + RangeDMG, 0, maxStatCanBeAchieved);
		Player.GetDamage(DamageClass.Magic) += Math.Clamp(UpdateMagic + MagicDMG, 0, maxStatCanBeAchieved);
		Player.GetDamage(DamageClass.Summon) += Math.Clamp(UpdateSummon + SummonDMG, 0, maxStatCanBeAchieved);
		Player.GetDamage(DamageClass.Generic) += Math.Clamp(UpdateDamagePure + DamagePure, 0, maxStatCanBeAchieved);
		Player.GetCritChance(DamageClass.Generic) += Math.Clamp(UpdateCritStrikeChance + CritStrikeChance, 0, maxStatCanBeAchieved);

		Player.statDefense += Math.Clamp(UpdateDefenseBase + DefenseBase, -(maxStatCanBeAchieved + Player.statDefense), maxStatCanBeAchieved);
		Player.moveSpeed = Math.Clamp(UpdateMovement + Movement + Player.moveSpeed, 0, maxStatCanBeAchieved);
		Player.jumpSpeedBoost = Math.Clamp(UpdateJumpBoost + JumpBoost + Player.jumpSpeedBoost, 0, maxStatCanBeAchieved);
		Player.lifeRegen = Math.Clamp(UpdateHPRegen + HPRegen + Player.lifeRegen, 0, maxStatCanBeAchieved);
		Player.manaRegen = Math.Clamp(UpdateManaRegen + ManaRegen + Player.manaRegen, 0, maxStatCanBeAchieved);
		Player.DefenseEffectiveness *= Math.Clamp(UpdateDefenseBase + DefenseEffectiveness + 1, 0, maxStatCanBeAchieved);
		Player.thorns += UpdateThorn + Thorn;

		Player.maxMinions = Math.Clamp(UpdateMinion + MinionSlot + Player.maxMinions, 0, maxStatCanBeAchieved);
		Player.maxTurrets = Math.Clamp(UpdateSentry + SentrySlot + Player.maxTurrets, 0, maxStatCanBeAchieved);

		UpdateMelee = 0;
		UpdateRange = 0;
		UpdateMagic = 0;
		UpdateSummon = 0;
		UpdateDamagePure = 0;
		UpdateMovement = 0;
		UpdateJumpBoost = 0;
		UpdateHPMax = 0;
		UpdateManaMax = 0;
		UpdateHPRegen = 0;
		UpdateManaRegen = 0;
		UpdateDefenseBase = 0;
		UpdateCritStrikeChance = 0;
		UpdateCritDamage = 0;
		UpdateDefEff = 0;
		UpdateDropAmount = 0;
		UpdateThorn = 0;
		UpdateMinion = 0;
		UpdateSentry = 0;
		DebuffTime = new StatModifier();
		BuffTime = new StatModifier();
	}
	public override void Initialize() {
		MeleeDMG = 0;
		RangeDMG = 0;
		MagicDMG = 0;
		SummonDMG = 0;
		Movement = 0;
		JumpBoost = 0;
		HPMax = 0;
		HPRegen = 0;
		ManaMax = 0;
		ManaRegen = 0;
		DefenseBase = 0;
		DamagePure = 0;
		CritStrikeChance = 0;
		Thorn = 0;
		CritDamage = 0;
		DefenseEffectiveness = 0;
		DropAmountIncrease = 0;
		MinionSlot = 0;
		SentrySlot = 0;
		CardTracker = 0;
		CardLuck = 0;
	}
	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
		try {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.CardEffect);
			packet.Write((byte)Player.whoAmI);
			packet.Write(MeleeDMG);
			packet.Write(RangeDMG);
			packet.Write(MagicDMG);
			packet.Write(SummonDMG);
			packet.Write(Movement);
			packet.Write(JumpBoost);
			packet.Write(HPMax);
			packet.Write(HPRegen);
			packet.Write(ManaMax);
			packet.Write(ManaRegen);
			packet.Write(DefenseBase);
			packet.Write(DamagePure);
			packet.Write(CritStrikeChance);
			packet.Write(CritDamage);
			packet.Write(DefenseEffectiveness);
			packet.Write(DropAmountIncrease);
			packet.Write(MinionSlot);
			packet.Write(SentrySlot);
			packet.Write(Thorn);
			packet.Write(CardTracker);
			packet.Write(CardLuck);
			packet.Send(toWho, fromWho);
		}
		catch (Exception ex) {

		}
	}
	public override void SaveData(TagCompound tag) {
		try {
			tag["MeleeDMG"] = MeleeDMG;
			tag["RangeDMG"] = RangeDMG;
			tag["MagicDMG"] = MagicDMG;
			tag["SummonDMG"] = SummonDMG;
			tag["Movement"] = Movement;
			tag["JumpBoost"] = JumpBoost;
			tag["HPMax"] = HPMax;
			tag["HPRegen"] = HPRegen;
			tag["ManaMax"] = ManaMax;
			tag["ManaRegen"] = ManaRegen;
			tag["DefenseBase"] = DefenseBase;
			tag["DamagePure"] = DamagePure;
			tag["CritStrikeChance"] = CritStrikeChance;
			tag["CritDamage"] = CritDamage;
			tag["DefenseEffectiveness"] = DefenseEffectiveness;
			tag["DropAmountIncrease"] = DropAmountIncrease;
			tag["MinionSlot"] = MinionSlot;
			tag["SentrySlot"] = SentrySlot;
			tag["Thorn"] = Thorn;
			tag["CardTracker"] = CardTracker;
			tag["CardLuck"] = CardLuck;
		}
		catch {
		}
	}
	public override void LoadData(TagCompound tag) {
		try {
			MeleeDMG = (float)tag["MeleeDMG"];
			RangeDMG = (float)tag["RangeDMG"];
			MagicDMG = (float)tag["MagicDMG"];
			SummonDMG = (float)tag["SummonDMG"];
			Movement = (float)tag["Movement"];
			JumpBoost = (float)tag["JumpBoost"];
			HPMax = (int)tag["HPMax"];
			HPRegen = (int)tag["HPRegen"];
			ManaMax = (int)tag["ManaMax"];
			ManaRegen = (int)tag["ManaRegen"];
			DefenseBase = (int)tag["DefenseBase"];
			DamagePure = (float)tag["DamagePure"];
			CritStrikeChance = (int)tag["CritStrikeChance"];
			CritDamage = (float)tag["CritDamage"];
			DefenseEffectiveness = (float)tag["DefenseEffectiveness"];
			DropAmountIncrease = (int)tag["DropAmountIncrease"];
			MinionSlot = (int)tag["MinionSlot"];
			SentrySlot = (int)tag["SentrySlot"];
			Thorn = (float)tag["Thorn"];
			CardTracker = (int)tag["CardTracker"];
			if (tag.TryGet("CardLuck", out int cardluck)) {
				CardLuck = cardluck;
			}
			else {
				CardLuck = 0;
			}
		}
		catch {
		}
	}
	public void ReceivePlayerSync(BinaryReader reader) {
		MeleeDMG = reader.ReadSingle();
		RangeDMG = reader.ReadSingle();
		MagicDMG = reader.ReadSingle();
		SummonDMG = reader.ReadSingle();
		Movement = reader.ReadSingle();
		JumpBoost = reader.ReadSingle();
		HPMax = reader.ReadInt32();
		HPRegen = reader.ReadInt32();
		ManaMax = reader.ReadInt32();
		ManaRegen = reader.ReadInt32();
		DefenseBase = reader.ReadInt32();
		DamagePure = reader.ReadSingle();
		CritStrikeChance = reader.ReadInt32();
		CritDamage = reader.ReadSingle();
		DefenseEffectiveness = reader.ReadSingle();
		DropAmountIncrease = reader.ReadInt32();
		MinionSlot = reader.ReadInt32();
		SentrySlot = reader.ReadInt32();
		Thorn = reader.ReadSingle();
		CardTracker = reader.ReadInt32();
		CardLuck = reader.ReadInt32();
	}
	public override void CopyClientState(ModPlayer targetCopy) {
		PlayerStatsHandle clone = (PlayerStatsHandle)targetCopy;
		clone.MeleeDMG = MeleeDMG;
		clone.RangeDMG = RangeDMG;
		clone.MagicDMG = MagicDMG;
		clone.SummonDMG = SummonDMG;
		clone.Movement = Movement;
		clone.JumpBoost = JumpBoost;
		clone.HPMax = HPMax;
		clone.HPRegen = HPRegen;
		clone.ManaMax = ManaMax;
		clone.ManaRegen = ManaRegen;
		clone.DefenseBase = DefenseBase;
		clone.DamagePure = DamagePure;
		clone.CritStrikeChance = CritStrikeChance;
		clone.CritDamage = CritDamage;
		clone.DefenseEffectiveness = DefenseEffectiveness;
		clone.DropAmountIncrease = DropAmountIncrease;
		clone.MinionSlot = MinionSlot;
		clone.SentrySlot = SentrySlot;
		clone.Thorn = Thorn;
		clone.CardTracker = CardTracker;
		clone.CardLuck = CardLuck;
	}
	public override void SendClientChanges(ModPlayer clientPlayer) {
		PlayerStatsHandle clone = (PlayerStatsHandle)clientPlayer;
		if (MeleeDMG != clone.MeleeDMG
			|| RangeDMG != clone.RangeDMG
			|| MagicDMG != clone.MagicDMG
			|| SummonDMG != clone.SummonDMG
			|| Movement != clone.Movement
			|| JumpBoost != clone.JumpBoost
			|| HPMax != clone.HPMax
			|| HPRegen != clone.HPRegen
			|| ManaMax != clone.ManaMax
			|| ManaRegen != clone.ManaRegen
			|| DefenseBase != clone.DefenseBase
			|| DamagePure != clone.DamagePure
			|| CritStrikeChance != clone.CritStrikeChance
			|| CritDamage != clone.CritDamage
			|| DefenseEffectiveness != clone.DefenseEffectiveness
			|| DropAmountIncrease != clone.DropAmountIncrease
			|| MinionSlot != clone.MinionSlot
			|| SentrySlot != clone.SentrySlot
			|| Thorn != clone.Thorn
			|| CardTracker != clone.CardTracker
			|| CardLuck != clone.CardLuck) {
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
public class PlayerStatsHandleSystem : ModSystem {
	public override void Load() {
		base.Load();
		On_NPC.AddBuff += HookBuffTimeModify;
		On_Player.AddBuff += IncreasesPlayerBuffTime;
	}

	private void IncreasesPlayerBuffTime(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack) {
		if (self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			if (!Main.debuff[type]) {
				orig(self, type, (int)modplayer.BuffTime.ApplyTo(timeToAdd), quiet, foodHack);
			}
			else {
				orig(self, type, timeToAdd, quiet, foodHack);
			}
		}
		else {
			orig(self, type, timeToAdd, quiet, foodHack);
		}
	}

	private void HookBuffTimeModify(On_NPC.orig_AddBuff orig, NPC self, int type, int time, bool quiet) {
		Player player = Main.player[self.lastInteraction];
		if (player.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			orig(self, type, (int)modplayer.DebuffTime.ApplyTo(time), quiet);
		}
		else {
			orig(self, type, time, quiet);
		}
	}
}
