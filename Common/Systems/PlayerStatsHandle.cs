using System;
using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;

namespace BossRush.Common.Systems;
public class PlayerStatsHandle : ModPlayer {
	public float AuraRadius = 50f;
	public StatModifier AuraModifier = new StatModifier();
	public float GetAuraRadius() => AuraModifier.ApplyTo(AuraRadius);
	public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();
	public const int maxStatCanBeAchieved = 9999;

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

	public int UpdateMinion = 0;
	public int UpdateSentry = 0;

	public StatModifier DebuffTime = new StatModifier();
	public StatModifier BuffTime = new StatModifier();

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
		Player.DefenseEffectiveness *= UpdateDefEff.ApplyTo(Player.DefenseEffectiveness.Value);
		Player.thorns = UpdateThorn.ApplyTo(Player.thorns);

		Player.maxMinions = Math.Clamp(UpdateMinion + Player.maxMinions, 0, maxStatCanBeAchieved);
		Player.maxTurrets = Math.Clamp(UpdateSentry + Player.maxTurrets, 0, maxStatCanBeAchieved);

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
		UpdateMinion = 0;
		UpdateSentry = 0;
		DebuffTime = new StatModifier();
		BuffTime = new StatModifier();
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
