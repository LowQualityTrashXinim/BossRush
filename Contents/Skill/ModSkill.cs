using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.Skill;
public class HellFireArrowRain : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 75;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
	}
	public override void Update(Player player) {
		if (!Main.rand.NextBool(3)) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(28);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(2);
		Vector2 position = Main.MouseWorld;
		position.Y -= 500;
		position.X += Main.rand.NextFloat(-75, 75);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.UnitY * Main.rand.NextFloat(20, 24), ProjectileID.HellfireArrow, damage, knockback, player.whoAmI);
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 180;
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}
public class Increases_3xDamage : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 530;
		Skill_Duration = 8;
		Skill_CoolDown = BossRushUtils.ToSecond(15);
	}
	public override void Update(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 4);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerBankDebuff>(), BossRushUtils.ToMinute(1));
	}
}
public class PowerBankDebuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, -.5f);
	}
}
public class PowerSaver : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 560;
		Skill_Duration = 0;
		Skill_CoolDown = BossRushUtils.ToMinute(1);
		Skill_EnergyRequirePercentage = -.5f;
	}
}
public class FastForward : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = BossRushUtils.ToSecond(20);
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration, out StatModifier cooldown) {
		energy = new();
		duration = new();
		cooldown = new();
		duration -= .5f;
		cooldown -= .5f;
	}
}
public class TranquilMind : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 400;
		Skill_Duration = 5;
		Skill_CoolDown = BossRushUtils.ToSecond(60);
	}
}
public class SpiritBurst : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 210;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(7);
	}
	public override void Update(Player player) {
		if (!Main.rand.NextBool(2)) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(40);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<SpiritProjectile>(), damage, knockback, player.whoAmI);
	}
}
public class Icicle : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = BossRushUtils.ToSecond(1f);
		Skill_CoolDown = BossRushUtils.ToSecond(9);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 10 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(18);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		float rotation = MathHelper.ToRadians(Main.rand.NextFloat(90));
		for (int i = 0; i < 6; i++) {
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenlyPlus(6, 360, i).RotatedBy(rotation) * 50;
			for (int l = 0; l < 6; l++) {
				int dust = Dust.NewDust(pos, 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(1, 2));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
			}
			Vector2 vel = (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero) * (10 + Main.rand.NextFloat(-3, 3));
			Projectile.NewProjectile(player.GetSource_FromThis(), pos, vel, ProjectileID.Blizzard, damage, knockback, player.whoAmI);

		}
	}
}
public class InfiniteManaSupply : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
	}
	public override void Update(Player player) {
		if (player.statMana < player.statManaMax2) {
			player.statMana += 10;
		}
	}
}
public class GuaranteedCrit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 425;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SetCrit();
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SetCrit();
	}
}
public class FireBall : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 45;
		Skill_Duration = 7;
		Skill_CoolDown = BossRushUtils.ToSecond(2);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 10 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(43);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(4);
		Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity, ProjectileID.Flamelash, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].timeLeft = 120;
	}
}
public class RapidHealing : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 345;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(30);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 6 != 0) {
			return;
		}
		player.Heal(6);
	}
}
public class StarFury : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 225;
		Skill_Duration = 6;
		Skill_CoolDown = BossRushUtils.ToSecond(20);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 120 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Generic).ApplyTo(1000);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
		Vector2 position = player.Center.Subtract(0, 1000);
		Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 20f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ProjectileID.StarWrath, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 600;
	}
}
public class AdAstra : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = BossRushUtils.ToSecond(3);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
	}
	public override void Update(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 5f);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<AdAstraDebuff>(), BossRushUtils.ToSecond(15));
	}
}
public class AdAstraDebuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Multiplicative: .1f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: .1f);
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: .1f);
	}
}
public class MeteorShower : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_CanBeSelect = false;
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 5 != 0) {
			return;
		}
		int damage = (int)(player.GetTotalDamage(DamageClass.Magic).ApplyTo(44) + player.GetWeaponDamage(player.HeldItem) * .44f);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
		Vector2 position = player.Center.Subtract(Main.rand.Next(-1000, 1000), 1000);
		Vector2 velocity = (Main.MouseWorld - position + Main.rand.NextVector2Circular(200, 200)).SafeNormalize(Vector2.Zero) * 10f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, Main.rand.Next(new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 }), damage, knockback, player.whoAmI, ai1: Main.rand.NextFloat(1, 2));
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 600;
	}
}
public class BloodToPower : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 570;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(9);
	}
	public override void OnTrigger(Player player) {
		int blood = player.statLife / 2;
		player.statLife -= blood;
		player.GetModPlayer<SkillHandlePlayer>().BloodToPower = blood;
	}
	public override void Update(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + player.GetModPlayer<SkillHandlePlayer>().BloodToPower * .01f);
	}
}
public class Overclock : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 635;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(9);
	}
	public override void Update(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, 3);
	}
}
public class TerrorForm : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 900;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
	}
	public override void Update(Player player) {
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.UltraBrightTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(3);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		player.statLife = Math.Clamp(player.statLife - 1, 1, player.statLifeMax2);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		float percentage = (1 - player.statLife / (float)player.statLifeMax2);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Multiplicative: 1 + percentage, Base: 25);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.35f, Multiplicative: 1 + percentage);
	}
}
public class BulletStorm : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 255;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(19);
	}
	public override void Update(Player player) {
		Vector2 spawn = player.Center.Subtract(0, 1000);
		int damage = (int)player.GetDamage(DamageClass.Ranged).ApplyTo(30);
		for (int i = 0; i < 3; i++) {
			float RandomizeX = Main.rand.NextFloat(-300, 300);
			Projectile.NewProjectile(player.GetSource_Misc("Skill"), spawn.Add(RandomizeX, 0) + Main.rand.NextVector2Circular(300, 300), Vector2.UnitY * Main.rand.Next(10, 14), Main.rand.Next(TerrariaArrayID.Bullet), damage, 2);
		}
	}
}
public class AllOrNothing : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 1500;
		Skill_Duration = 1;
		Skill_CoolDown = BossRushUtils.ToMinute(15);
		Skill_CanBeSelect = false;
	}
	public override void OnTrigger(Player player) {
		player.AddBuff(ModContent.BuffType<AllOrNothingBuff>(), BossRushUtils.ToSecond(5));
	}
}
public class AllOrNothingBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		if (player.buffTime[buffIndex] <= 0) {
			player.Center.LookForHostileNPC(out List<NPC> npclist, 1000);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X > npc.Center.X ? 1 : -1;
				int originDmg = (int)(npc.lifeMax * .1f);
				int dmg = originDmg;
				for (int i = 2; i < 17; i++) {
					if (Main.rand.NextBool(i)) {
						dmg += (int)(originDmg * Main.rand.NextFloat(.85f, 1.15f));
					}
				}
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(dmg, direction));
			}
		}
	}
}
//Summon skill
public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(3);
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordProjectile3>()] < 1) {
			for (int i = 0; i < 3; i++) {
				SummonSword(player, i);
			}
		}
	}
	private void SummonSword(Player player, int index) {
		int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(34);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(3);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile3>(), damage, knockback, player.whoAmI, 0, 0, index);
		if (Main.projectile[proj].ModProjectile is SwordProjectile3 woodproj)
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
	}
}
public class WoodSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = BossRushUtils.ToSecond(3);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
		Skill_CanBeSelect = false;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		ShootingSword(player);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		ShootingSword(player);
	}
	private void ShootingSword(Player player) {
		Vector2 position = player.Center + Main.rand.NextVector2Circular(50, 50);
		Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero);
		int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(24);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(10);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI);
		Main.projectile[proj].timeLeft = Skill_Duration;
		if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
	}
}
