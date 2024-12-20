﻿using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SuperFlareGun {
	internal class SuperFlareP : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 100;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = -20f;
			return false;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<SuperFlareGun>(), ItemID.BluePhaseblade))
				if (Projectile.timeLeft > 50)
					Projectile.timeLeft = 50;
			int RandomDust = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemTopaz });
			for (int i = 0; i < 7; i++) {
				Vector2 Rotate = Main.rand.NextVector2Circular(5f, 5f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			float randomRotation = Main.rand.NextFloat(90);
			int RandomDust = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz });
			Vector2 Rotate;
			for (int i = 0; i < 150; i++) {
				Rotate = Main.rand.NextVector2Circular(25f, 25f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 2.5f));
				Main.dust[dustnumber].noGravity = true;
			}
			int RandomDust2 = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz });
			for (int i = 0; i < 300; i++) {
				if (i % 2 == 0) {
					Rotate = Main.rand.NextVector2CircularEdge(10f, 30f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 1.5f;
				}
				else {
					Rotate = Main.rand.NextVector2CircularEdge(30f, 10f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 1.5f;
				}
				int dustnumber1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, RandomDust2, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.75f, 2f));
				Main.dust[dustnumber1].noGravity = true;
			}
			Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 600);
			foreach (NPC npc in npclist) {
				int direction = Projectile.Center.X - npc.Center.X > 0 ? -1 : 1;
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage * 5, direction, false, 10));
			}
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<SuperFlareGun>(), ItemID.Boomstick) && Main.rand.NextBool(10)) {
				for (int i = 0; i < 36; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * 5;
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, ProjectileID.Bullet, Projectile.damage, Projectile.knockBack, Projectile.owner);

				}
			}
		}
	}
	internal class ExplodeProjectile : ModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.timeLeft = 55;
		}
		public override void AI() {
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 50) {
				Projectile.damage = 50;
			}
		}
		public override void OnKill(int timeLeft) {
			int RandomDust = Main.rand.Next();
			for (int i = 0; i < 55; i++) {
				Vector2 Rotate = Main.rand.NextVector2CircularEdge(10f, 10f);
				int dustnumber = Dust.NewDust(Projectile.Center, 10, 10, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.immune[Projectile.owner] = 4;
		}
	}
}
