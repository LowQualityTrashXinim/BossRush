using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Perks;
using System.Linq;

namespace BossRush.Common.Systems;
public class PlayerStatsHandle : ModPlayer {
	public float GetAuraRadius(int radius) => AuraModifier.ApplyTo(radius);
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

	public StatModifier DebuffBuffTime = new StatModifier();

	public StatModifier AttackSpeed = new StatModifier();

	public StatModifier ShieldHealth = new StatModifier();

	public StatModifier ShieldEffectiveness = new StatModifier();

	public StatModifier LifeStealEffectiveness = new StatModifier();

	public StatModifier EnergyCap = new StatModifier();

	public StatModifier RechargeEnergyCap = new StatModifier();

	public StatModifier UpdateFullHPDamage = new StatModifier();

	public StatModifier StaticDefense = new StatModifier();

	public StatModifier DebuffDamage = new StatModifier();

	public StatModifier SynergyDamage = new StatModifier();

	public StatModifier Iframe = new StatModifier();
	//public float LuckIncrease = 0; 
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.CritDamage = modifiers.CritDamage.CombineWith(UpdateCritDamage);
		if (target.life >= target.lifeMax) {
			modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(UpdateFullHPDamage);
		}
		if (target.buffType.Where(i => Main.debuff[i]).Any()) {
			modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(DebuffDamage);
		}
	}
	public override void PostHurt(Player.HurtInfo info) {
		base.PostHurt(info);
		if(!info.PvP) {
			//Player.immuneTime += (int)(Iframe - 1).ApplyTo(Player.immuneTime);
		}
	}
	public override void PostUpdate() {
		ChestLoot.amountModifier = (int)UpdateDropAmount.ApplyTo(ChestLoot.amountModifier);
	}
	public override void UpdateLifeRegen() {
		Player.lifeRegen = (int)UpdateHPRegen.ApplyTo(Player.lifeRegen);
	}
	public override void ResetEffects() {
		SkillHandlePlayer modplayer = Player.GetModPlayer<SkillHandlePlayer>();
		modplayer.EnergyCap = (int)EnergyCap.ApplyTo(500);
		modplayer.EnergyRechargeCap = (int)RechargeEnergyCap.ApplyTo(1);
		Player.moveSpeed = UpdateMovement.ApplyTo(Player.moveSpeed);
		Player.jumpSpeedBoost = UpdateJumpBoost.ApplyTo(Player.jumpSpeedBoost);
		Player.manaRegen = (int)UpdateManaRegen.ApplyTo(Player.manaRegen);
		Player.statDefense += (int)(UpdateDefenseBase.Base + UpdateDefenseBase.Flat);
		Player.statDefense.AdditiveBonus += UpdateDefenseBase.Additive - 1;
		Player.statDefense.FinalMultiplier *= UpdateDefenseBase.Multiplicative;
		Player.DefenseEffectiveness *= UpdateDefEff.ApplyTo(Player.DefenseEffectiveness.Value);
		Player.thorns = UpdateThorn.ApplyTo(Player.thorns);

		Player.maxMinions = (int)UpdateMinion.ApplyTo(Player.maxMinions);
		Player.maxTurrets = (int)UpdateSentry.ApplyTo(Player.maxTurrets);

		Player.statLifeMax2 = (int)UpdateHPMax.ApplyTo(Player.statLifeMax2);
		Player.statManaMax2 = (int)UpdateManaMax.ApplyTo(Player.statManaMax2);

		UpdateFullHPDamage = StatModifier.Default;
		UpdateMinion = StatModifier.Default;
		UpdateSentry = StatModifier.Default;
		UpdateMovement = StatModifier.Default;
		UpdateJumpBoost = StatModifier.Default;
		UpdateHPMax = StatModifier.Default;
		UpdateManaMax = StatModifier.Default;
		UpdateHPRegen = StatModifier.Default;
		UpdateManaRegen = StatModifier.Default;
		UpdateDefenseBase = StatModifier.Default;
		UpdateCritDamage = StatModifier.Default;
		UpdateDefEff = StatModifier.Default;
		UpdateDropAmount = StatModifier.Default;
		UpdateThorn = StatModifier.Default;
		AuraModifier = StatModifier.Default;
		DebuffTime = StatModifier.Default;
		BuffTime = StatModifier.Default;
		DebuffBuffTime = StatModifier.Default;
		ShieldEffectiveness = StatModifier.Default;
		ShieldHealth = StatModifier.Default;
		AttackSpeed = StatModifier.Default;
		AuraModifier = StatModifier.Default;
		LifeStealEffectiveness = StatModifier.Default;
		EnergyCap = StatModifier.Default;
		RechargeEnergyCap = StatModifier.Default;
		StaticDefense = StatModifier.Default - 1;
		DebuffDamage = StatModifier.Default;
		SynergyDamage = StatModifier.Default;
		Iframe = StatModifier.Default;
		successfullyKillNPCcount = 0;
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = AttackSpeed.ApplyTo(base.UseSpeedMultiplier(item));
		return useSpeed;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage.Flat = MathHelper.Clamp(modifiers.FinalDamage.Flat - StaticDefense.ApplyTo(1), 0, int.MaxValue);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage.Flat = MathHelper.Clamp(modifiers.FinalDamage.Flat - StaticDefense.ApplyTo(1), 0, int.MaxValue);
	}
	/// <summary>
	/// This should be uses in always update code
	/// when creating a new stat modifier, pleases uses the default and increases from there
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="Additive"></param>
	/// <param name="Multiplicative"></param>
	/// <param name="Flat"></param>
	/// <param name="Base"></param>
	public void AddStatsToPlayer(PlayerStats stat, StatModifier StatMod) {
		if (stat == PlayerStats.None) {
			return;
		}
		switch (stat) {
			case PlayerStats.MeleeDMG:
				Player.GetDamage(DamageClass.Melee) = Player.GetTotalDamage(DamageClass.Melee).CombineWith(StatMod);
				break;
			case PlayerStats.RangeDMG:
				Player.GetDamage(DamageClass.Ranged) = Player.GetTotalDamage(DamageClass.Ranged).CombineWith(StatMod);
				break;
			case PlayerStats.MagicDMG:
				Player.GetDamage(DamageClass.Magic) = Player.GetDamage(DamageClass.Magic).CombineWith(StatMod);
				break;
			case PlayerStats.SummonDMG:
				Player.GetDamage(DamageClass.Summon) = Player.GetTotalDamage(DamageClass.Summon).CombineWith(StatMod);
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
			case PlayerStats.PureDamage:
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
			case PlayerStats.EnergyCap:
				EnergyCap = EnergyCap.CombineWith(StatMod);
				break;
			case PlayerStats.EnergyRechargeCap:
				RechargeEnergyCap = RechargeEnergyCap.CombineWith(StatMod);
				break;
			case PlayerStats.FullHPDamage:
				UpdateFullHPDamage = UpdateFullHPDamage.CombineWith(StatMod);
				break;
			case PlayerStats.StaticDefense:
				StaticDefense = StaticDefense.CombineWith(StatMod);
				break;
			case PlayerStats.DebuffDamage:
				DebuffDamage = DebuffDamage.CombineWith(StatMod);
				break;
			case PlayerStats.SynergyDamage:
				SynergyDamage = SynergyDamage.CombineWith(StatMod);
				break;
			case PlayerStats.Iframe:
				Iframe = Iframe.CombineWith(StatMod);
				break;
			default:
				break;
		}
	}
	public int successfullyKillNPCcount = 0;
	public int requestShootExtra = 0;
	public float requestVelocityChange = 0;
	/// <summary>
	/// This should be uses in always update code
	/// when creating a new stat modifier, pleases uses the default and increases from there
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="Additive"></param>
	/// <param name="Multiplicative"></param>
	/// <param name="Flat"></param>
	/// <param name="Base"></param>
	public void AddStatsToPlayer(PlayerStats stat, float Additive = 1, float Multiplicative = 1, float Flat = 0, float Base = 0) {
		if (stat == PlayerStats.None) {
			return;
		}
		if (Additive < 0) {
			Additive += 1;
		}
		StatModifier StatMod = new StatModifier(Additive, Multiplicative, Flat, Base);
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
			case PlayerStats.PureDamage:
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
			case PlayerStats.FullHPDamage:
				UpdateFullHPDamage = UpdateFullHPDamage.CombineWith(StatMod);
				break;
			case PlayerStats.StaticDefense:
				StaticDefense = StaticDefense.CombineWith(StatMod);
				break;
			case PlayerStats.DebuffDamage:
				DebuffDamage = DebuffDamage.CombineWith(StatMod);
				break;
			case PlayerStats.SynergyDamage:
				SynergyDamage = SynergyDamage.CombineWith(StatMod);
				break;
			case PlayerStats.Iframe:
				Iframe = Iframe.CombineWith(StatMod);
				break;
			default:
				break;
		}
	}
}
public class PlayerStatsHandleSystem : ModSystem {
	public override void Load() {
		On_NPC.AddBuff += HookBuffTimeModify;
		On_Player.AddBuff += IncreasesPlayerBuffTime;
		On_Player.Heal += On_Player_Heal;
		On_Player.AddImmuneTime += On_Player_AddImmuneTime;
		On_Projectile.NewProjectile_IEntitySource_Vector2_Vector2_int_int_float_int_float_float_float += On_Projectile_NewProjectile_IEntitySource_Vector2_Vector2_int_int_float_int_float_float_float;
		On_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += On_Projectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
		On_Projectile.NewProjectileDirect += On_Projectile_NewProjectileDirect;
		On_Player.GiveImmuneTimeForCollisionAttack += On_Player_GiveImmuneTimeForCollisionAttack;
		On_Player.SetImmuneTimeForAllTypes += On_Player_SetImmuneTimeForAllTypes;
	}

	private void On_Player_SetImmuneTimeForAllTypes(On_Player.orig_SetImmuneTimeForAllTypes orig, Player self, int time) {
		if (self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			orig(self, (int)(modplayer.Iframe - 1).ApplyTo(time) + time);
		}
		else {
			orig(self, time);
		}
	}

	private void On_Player_GiveImmuneTimeForCollisionAttack(On_Player.orig_GiveImmuneTimeForCollisionAttack orig, Player self, int time) {
		if (self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			orig(self,(int)(modplayer.Iframe - 1).ApplyTo(time) + time);
		}
		else {
			orig(self, time);
		}
	}

	private void On_Player_AddImmuneTime(On_Player.orig_AddImmuneTime orig, Player self, int cooldownCounterId, int immuneTime) {
		if(self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			orig(self, cooldownCounterId, (int)(modplayer.Iframe - 1).ApplyTo(immuneTime) + immuneTime);
		}
		else {
			orig(self, cooldownCounterId, immuneTime);
		}
	}

	private Projectile On_Projectile_NewProjectileDirect(On_Projectile.orig_NewProjectileDirect orig, IEntitySource spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int owner, float ai0, float ai1, float ai2) {
		if (owner < 0 || owner > 255) {
			return orig(spawnSource, position, velocity, type, damage, knockback, owner, ai0, ai1, ai2);
		}
		Player player = Main.player[owner];
		int shootExtra = player.GetModPlayer<PlayerStatsHandle>().requestShootExtra;
		if (shootExtra > 0) {
			player.GetModPlayer<PlayerStatsHandle>().requestShootExtra = 0;
			for (int i = 0; i < shootExtra; i++) {
				Projectile proj = orig(spawnSource, position, velocity.Vector2RotateByRandom(player.GetModPlayer<PlayerStatsHandle>().requestVelocityChange), type, damage, knockback, owner, ai0, ai1, ai2);
				if (CheckProjectile_ScatterShotCondition(player, proj)) {
					proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
				}
			}
		}
		Projectile projectile = orig(spawnSource, position, velocity, type, damage, knockback, owner, ai0, ai1, ai2);
		if (CheckProjectile_ScatterShotCondition(player, projectile)) {
			projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
		}
		return projectile;
	}

	private int On_Projectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float(On_Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2) {
		if (Owner < 0 || Owner > 255) {
			return orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
		}
		Player player = Main.player[Owner];
		int shootExtra = player.GetModPlayer<PlayerStatsHandle>().requestShootExtra;
		if (shootExtra > 0) {
			player.GetModPlayer<PlayerStatsHandle>().requestShootExtra = 0;
			for (int i = 0; i < shootExtra; i++) {
				Vector2 newSpeed = new Vector2(SpeedX, SpeedY).Vector2RotateByRandom(player.GetModPlayer<PlayerStatsHandle>().requestVelocityChange);
				int proj = orig(spawnSource, X, Y, newSpeed.X, newSpeed.Y, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
				if (CheckProjectile_ScatterShotCondition(player, proj)) {
					Main.projectile[proj].GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
				}
			}
		}
		int projectile = orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
		if (CheckProjectile_ScatterShotCondition(player, projectile)) {
			Main.projectile[projectile].GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
		}
		return projectile;
	}

	private int On_Projectile_NewProjectile_IEntitySource_Vector2_Vector2_int_int_float_int_float_float_float(On_Projectile.orig_NewProjectile_IEntitySource_Vector2_Vector2_int_int_float_int_float_float_float orig,
		IEntitySource spawnSource, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2) {
		if (Owner < 0 || Owner > 255) {
			return orig(spawnSource, position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
		}
		Player player = Main.player[Owner];
		int shootExtra = player.GetModPlayer<PlayerStatsHandle>().requestShootExtra;
		if (shootExtra > 0) {
			player.GetModPlayer<PlayerStatsHandle>().requestShootExtra = 0;
			for (int i = 0; i < shootExtra; i++) {
				int proj = orig(spawnSource, position, velocity.Vector2RotateByRandom(player.GetModPlayer<PlayerStatsHandle>().requestVelocityChange), Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
				if (CheckProjectile_ScatterShotCondition(player, proj)) {
					Main.projectile[proj].GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
				}
			}
		}
		int projectile = orig(spawnSource, position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
		if (CheckProjectile_ScatterShotCondition(player, projectile)) {
			Main.projectile[projectile].GetGlobalProjectile<RoguelikeGlobalProjectile>().OnKill_ScatterShot += 2;
		}
		return projectile;
	}
	public static bool CheckProjectile_ScatterShotCondition(Player player, Projectile proj) => player.GetModPlayer<PerkPlayer>().perk_ScatterShot;
	public static bool CheckProjectile_ScatterShotCondition(Player player, int proj) => player.GetModPlayer<PerkPlayer>().perk_ScatterShot;


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
				orig(self, type, (int)modplayer.DebuffBuffTime.ApplyTo(timeToAdd), quiet, foodHack);
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
