using System;
using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using UtfUnknown.Core.Models;

namespace BossRush.Common.Systems;
public class PlayerStatsHandle : ModPlayer {
	public float AuraRadius = 50f;
	public float GetAuraRadius() => AuraModifier.ApplyTo(AuraRadius);
	public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();

	public StatModifier AuraModifier = new StatModifier();

	public StatModifier UpdateMovement = new StatModifier();

	public StatModifier UpdateJumpBoost = new StatModifier();

	public StatModifier UpdateHPMax = new StatModifier();

	public StatModifier UpdateHPRegen = new StatModifier();

	public StatModifier UpdateManaMax = new StatModifier();

	public StatModifier UpdateManaRegen = new StatModifier();

	public StatModifier UpdateDefenseBase = new StatModifier();

	public StatModifier UpdateThorn = new StatModifier();

	public StatModifier UpdateCritDamage = new StatModifier();

	public StatModifier UpdateDefEff = new StatModifier();

	public StatModifier UpdateDropAmount = new StatModifier();

	public StatModifier UpdateMinion = new StatModifier();

	public StatModifier UpdateSentry = new StatModifier();

	public StatModifier DebuffTime = new StatModifier();

	public StatModifier BuffTime = new StatModifier();

	public StatModifier AttackSpeed = new StatModifier();

	public StatModifier ShieldHealth = new StatModifier();

	public StatModifier ShieldEffectiveness = new StatModifier();

	public StatModifier LifeStealEffectiveness = new StatModifier();
	//public float LuckIncrease = 0;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.CritDamage = modifiers.CritDamage.CombineWith(UpdateCritDamage);
	}
	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
		health = StatModifier.Default;
		mana = StatModifier.Default;

		health = health.CombineWith(UpdateHPMax);
		mana = mana.CombineWith(UpdateManaMax);
	}
	public override void PostUpdate() {
		ChestLoot.amountModifier = (int)UpdateDropAmount.ApplyTo(ChestLoot.amountModifier);
	}
	public override void UpdateLifeRegen() {
		Player.lifeRegen = (int)UpdateHPRegen.ApplyTo(Player.lifeRegen);
	}
	public override void PostHurt(Player.HurtInfo info) {
		base.PostHurt(info);
		if (info.PvP) {
			return;
		}
	}
	public override void ResetEffects() {
		Player.moveSpeed = UpdateMovement.ApplyTo(Player.moveSpeed);
		Player.jumpSpeedBoost = UpdateJumpBoost.ApplyTo(Player.jumpSpeedBoost);
		Player.manaRegen = (int)UpdateManaRegen.ApplyTo(Player.manaRegen);
		Player.statDefense.AdditiveBonus += UpdateDefenseBase.Additive;
		Player.statDefense.FinalMultiplier *= UpdateDefenseBase.Multiplicative;
		Player.DefenseEffectiveness *= UpdateDefEff.ApplyTo(Player.DefenseEffectiveness.Value);
		Player.thorns = UpdateThorn.ApplyTo(Player.thorns);

		Player.maxMinions = (int)UpdateMinion.ApplyTo(Player.maxMinions);
		Player.maxTurrets = (int)UpdateSentry.ApplyTo(Player.maxTurrets);

		UpdateMovement = new StatModifier();
		UpdateJumpBoost = new StatModifier();
		UpdateHPMax = new StatModifier();
		UpdateManaMax = new StatModifier();
		UpdateHPRegen = new StatModifier();
		UpdateManaRegen = new StatModifier();
		UpdateDefenseBase = new StatModifier();
		UpdateCritDamage = new StatModifier();
		UpdateDefEff = new StatModifier();
		UpdateDropAmount = new StatModifier();
		UpdateThorn = new StatModifier();
		AuraModifier = new StatModifier();
		DebuffTime = new StatModifier();
		BuffTime = new StatModifier();
		ShieldEffectiveness = new StatModifier();
		ShieldHealth = new StatModifier();
		AttackSpeed = new StatModifier();
		AuraModifier = new StatModifier();
		LifeStealEffectiveness = new StatModifier();
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = AttackSpeed.ApplyTo(base.UseSpeedMultiplier(item));
		return useSpeed;
	}
	public void AddStatsToPlayer(PlayerStats stat, StatModifier StatMod) {
		if (stat == PlayerStats.None) {
			return;
		}
		switch (stat) {
			case PlayerStats.MeleeDMG:
				Player.GetDamage(DamageClass.Melee) = Player.GetDamage(DamageClass.Melee).CombineWith(StatMod);
				break;
			case PlayerStats.RangeDMG:
				Player.GetDamage(DamageClass.Ranged) = Player.GetDamage(DamageClass.Ranged).CombineWith(StatMod);
				break;
			case PlayerStats.MagicDMG:
				Player.GetDamage(DamageClass.Magic) = Player.GetDamage(DamageClass.Magic).CombineWith(StatMod);
				break;
			case PlayerStats.SummonDMG:
				Player.GetDamage(DamageClass.Summon) = Player.GetDamage(DamageClass.Summon).CombineWith(StatMod);
				break;
			case PlayerStats.MovementSpeed:
				UpdateMovement = UpdateMovement.CombineWith(StatMod);
				break;
			case PlayerStats.JumpBoost:
				UpdateJumpBoost = UpdateJumpBoost.CombineWith(StatMod);
				break;
			case PlayerStats.MaxHP:
				UpdateHPMax = UpdateHPMax.CombineWith(StatMod);
				break;
			case PlayerStats.RegenHP:
				UpdateHPRegen = UpdateHPRegen.CombineWith(StatMod);
				break;
			case PlayerStats.MaxMana:
				UpdateManaMax = UpdateManaMax.CombineWith(StatMod);
				break;
			case PlayerStats.RegenMana:
				UpdateManaRegen = UpdateManaRegen.CombineWith(StatMod);
				break;
			case PlayerStats.Defense:
				UpdateDefenseBase = UpdateDefenseBase.CombineWith(StatMod);
				break;
			case PlayerStats.DamageUniverse:
				Player.GetDamage(DamageClass.Generic) = Player.GetDamage(DamageClass.Generic).CombineWith(StatMod);
				break;
			case PlayerStats.CritChance:
				Player.GetCritChance(DamageClass.Generic) = StatMod.ApplyTo(Player.GetCritChance(DamageClass.Generic));
				break;
			case PlayerStats.CritDamage:
				UpdateCritDamage = UpdateCritDamage.CombineWith(StatMod);
				break;
			case PlayerStats.DefenseEffectiveness:
				UpdateDefEff = UpdateDefEff.CombineWith(StatMod);
				break;
			case PlayerStats.Thorn:
				UpdateThorn = UpdateThorn.CombineWith(StatMod);
				break;
			case PlayerStats.MaxMinion:
				UpdateMinion = UpdateMinion.CombineWith(StatMod);
				break;
			case PlayerStats.MaxSentry:
				UpdateSentry = UpdateSentry.CombineWith(StatMod);
				break;
			case PlayerStats.AuraRadius:
				AuraModifier = AuraModifier.CombineWith(StatMod);
				break;
			case PlayerStats.ShieldHealth:
				ShieldHealth = ShieldHealth.CombineWith(StatMod);
				break;
			case PlayerStats.ShieldEffectiveness:
				ShieldEffectiveness = ShieldEffectiveness.CombineWith(StatMod);
				break;
			case PlayerStats.AttackSpeed:
				AttackSpeed = AttackSpeed.CombineWith(StatMod);
				break;
			case PlayerStats.LifeStealEffectiveness:
				LifeStealEffectiveness = LifeStealEffectiveness.CombineWith(StatMod);
				break;
			default:
				break;
		}
	}
}
public class PlayerStatsHandleSystem : ModSystem {
	public override void Load() {
		base.Load();
		On_NPC.AddBuff += HookBuffTimeModify;
		On_Player.AddBuff += IncreasesPlayerBuffTime;
		On_Player.Heal += On_Player_Heal;
	}

	private void On_Player_Heal(On_Player.orig_Heal orig, Player self, int amount) {
		if (self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			orig(self, (int)modplayer.LifeStealEffectiveness.ApplyTo(amount));
		}
		else {
			orig(self, amount);
		}
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
