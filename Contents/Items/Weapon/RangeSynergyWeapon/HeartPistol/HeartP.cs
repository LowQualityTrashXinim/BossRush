﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol {
	class HeartP : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 22;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.light = 0.45f;
			Projectile.timeLeft = 45;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (npc.lifeMax > 5 && !npc.friendly && npc.type != NPCID.TargetDummy) {
				player.Heal(Main.rand.Next(1, 3));
			}
			if (Main.rand.NextBool(50)) {
				Item.NewItem(Projectile.GetSource_OnHit(npc), npc.Hitbox, ItemID.Heart);
			}
		}

		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			Projectile.position += new Vector2(11, 11);
			int projectileType = ModContent.ProjectileType<smallerHeart>();
			int damage = (int)(Projectile.damage * 0.5f);
			float knockback = Projectile.knockBack;
			float Rotation = Projectile.rotation;
			Vector2 leftsideofheartshape1 = new Vector2(-5, 0);
			Vector2 leftsideofheartshape2 = new Vector2(-5, -2.5f);
			Vector2 leftsideofheartshape3 = new Vector2(-2.5f, -5);
			Vector2 leftsideofheartshape4 = new Vector2(-2.5f, 2.5f);
			Vector2 bottomheartshape = new Vector2(0, 5);
			Vector2 topheartshape = new Vector2(0, -2.5f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, bottomheartshape.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, topheartshape.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			for (int i = 0; i < 2; i++) {
				if (i == 1) {
					leftsideofheartshape1.X = -leftsideofheartshape1.X;
					leftsideofheartshape2.X = -leftsideofheartshape2.X;
					leftsideofheartshape3.X = -leftsideofheartshape3.X;
					leftsideofheartshape4.X = -leftsideofheartshape4.X;
				}
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape1.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape2.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape3.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape4.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			}

		}
	}
	internal class smallerHeart : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 30;
			Projectile.light = .25f;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.velocity -= Projectile.velocity * 0.05f;
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		}
	}
}
