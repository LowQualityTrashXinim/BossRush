using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Perks;
using System.Linq;
using Terraria.ID;
using System;
using BossRush.Common.General;
using BossRush.Common.Systems.Mutation;

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
	public StatModifier EnergyRecharge = new StatModifier();
	public StatModifier Iframe = new StatModifier();
	public StatModifier NonCriticalDamage = new StatModifier();
	public StatModifier SkillDuration = new();
	public StatModifier SkillCoolDown = new();
	//public float LuckIncrease = 0; 
	/// <summary>
	/// This is a universal dodge chance that work like <see cref="Player.endurance"/><br/>
	/// Having the chance value over 1f would make it 100% dodge chance<br/>
	/// The dodge immunity frame is <see cref="DodgeTimer"/>
	/// </summary>
	public float DodgeChance = 0;
	/// <summary>
	/// This is the universal dodge timer
	/// </summary>
	public int DodgeTimer = 44;
	/// <summary>
	/// This is a universal life steal that work depend on weapon damage <br/>
	/// This have a forced cool down so that it is not OP <br/>
	/// The cool down are made public and free to be modify cause fun
	/// </summary>
	public StatModifier LifeSteal = StatModifier.Default - 1;
	/// <summary>
	/// This is the public count cool down of <see cref="LifeSteal"/>
	/// </summary>
	public int LifeSteal_CoolDownCounter = 0;
	/// <summary>
	/// This is the count down, by default it set to 1s
	/// </summary>
	public int LifeSteal_CoolDown = 60;
	public int Rapid_LifeRegen = 0;
	public int Rapid_ManaRegen = 0;
	public int Debuff_LifeStruct = 0;
	/// <summary>
	/// This one is a hacky way of ensuring a hit always crit
	/// </summary>
	public bool? ModifyHit_OverrideCrit = null;
	/// <summary>
	/// This only work if no where in the code don't uses <see cref="NPC.HitModifiers.DisableCrit"/>
	/// </summary>
	public bool ModifyHit_Before_Crit = false;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (LifeSteal_CoolDownCounter <= 0 && LifeSteal.Additive > 0 || LifeSteal.ApplyTo(0) > 0) {
			Player.Heal((int)Math.Ceiling(LifeSteal.ApplyTo(hit.Damage)));
			LifeSteal_CoolDownCounter = LifeSteal_CoolDown;
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.CritDamage = modifiers.CritDamage.CombineWith(UpdateCritDamage);
		modifiers.NonCritDamage = modifiers.NonCritDamage.CombineWith(NonCriticalDamage);
		if (target.life >= target.lifeMax) {
			modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(UpdateFullHPDamage);
		}
		if (target.buffType.Where(i => Main.debuff[i]).Any()) {
			modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(DebuffDamage);
		}
		modifiers.ModifyHitInfo += Modifiers_ModifyHitInfo;
	}

	private void Modifiers_ModifyHitInfo(ref NPC.HitInfo info) {
		ModifyHit_Before_Crit = info.Crit;
		if (info.Crit) {
			ModifyHit_Before_Crit = true;
		}
		if (ModifyHit_OverrideCrit == null) {
			return;
		}
		info.Crit = (bool)ModifyHit_OverrideCrit;
	}

	public override bool FreeDodge(Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= DodgeChance) {
			Player.AddImmuneTime(info.CooldownCounter, DodgeTimer);
			return true;
		}
		return base.FreeDodge(info);
	}
	public override void PostUpdate() {
		Player.statLife = Math.Clamp(Player.statLife + Rapid_LifeRegen, 1, Player.statLifeMax2);
		Player.statMana = Math.Clamp(Player.statMana + Rapid_ManaRegen, 0, Player.statManaMax2);
	}
	public override void UpdateLifeRegen() {
		Player.lifeRegen = (int)UpdateHPRegen.ApplyTo(Player.lifeRegen);
	}
	public override void ResetEffects() {
		if (!Player.HasBuff(ModContent.BuffType<LifeStruckDebuff>())) {
			Debuff_LifeStruct = 0;
		}
		SkillHandlePlayer modplayer = Player.GetModPlayer<SkillHandlePlayer>();
		modplayer.EnergyCap = (int)EnergyCap.ApplyTo(1500);
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

		Player.statLifeMax2 = Math.Clamp((int)UpdateHPMax.ApplyTo(Player.statLifeMax2), 1, int.MaxValue);
		Player.statManaMax2 = Math.Clamp((int)UpdateManaMax.ApplyTo(Player.statManaMax2), 1, int.MaxValue);

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
		EnergyRecharge = StatModifier.Default;
		StaticDefense = StatModifier.Default - 1;
		DebuffDamage = StatModifier.Default;
		SynergyDamage = StatModifier.Default;
		Iframe = StatModifier.Default;
		NonCriticalDamage = StatModifier.Default;
		LifeSteal = StatModifier.Default - 1;
		SkillDuration = StatModifier.Default;
		DodgeChance = 0;
		DodgeTimer = 44;
		successfullyKillNPCcount = 0;
		LifeSteal_CoolDown = 60;
		LifeSteal_CoolDown = BossRushUtils.CountDown(LifeSteal_CoolDown);
		ModifyHit_OverrideCrit = null;
		ModifyHit_Before_Crit = false;
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = AttackSpeed.ApplyTo(base.UseSpeedMultiplier(item));
		return useSpeed;
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
			case PlayerStats.EnergyRecharge:
				EnergyRecharge = EnergyRecharge.CombineWith(StatMod);
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
			case PlayerStats.SkillDuration:
				SkillDuration = SkillDuration.CombineWith(StatMod);
				break;
			case PlayerStats.SkillCooldown:
				SkillCoolDown = SkillCoolDown.CombineWith(StatMod);
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
		StatModifier StatMod = new StatModifier();
		StatMod += Additive - 1;
		StatMod *= Multiplicative;
		StatMod.Flat = Flat;
		StatMod.Base = Base;
		AddStatsToPlayer(stat, StatMod);
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
	public static void AddStatsToPlayer(Player player, PlayerStats stat, float Additive = 1, float Multiplicative = 1, float Flat = 0, float Base = 0) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(stat, Additive, Multiplicative, Flat, Base);
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
		On_Player.StrikeNPCDirect += On_Player_StrikeNPCDirect;
	}

	private void On_Player_StrikeNPCDirect(On_Player.orig_StrikeNPCDirect orig, Player self, NPC npc, NPC.HitInfo hit) {
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
			orig(self, (int)(modplayer.Iframe - 1).ApplyTo(time) + time);
		}
		else {
			orig(self, time);
		}
	}

	private void On_Player_AddImmuneTime(On_Player.orig_AddImmuneTime orig, Player self, int cooldownCounterId, int immuneTime) {
		if (self.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
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
		if (Type == ProjectileID.BookOfSkullsSkull) {

		}
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
