﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
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
