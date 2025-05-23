﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.MagicBow {
	internal class RubyGemP : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Ruby);
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 300;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
		}
		int count = 0;
		public override void AI() {
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 20) {
				Projectile.velocity -= Projectile.velocity * 0.1f;
				if (Math.Abs(Projectile.velocity.X) < 1 && Math.Abs(Projectile.velocity.Y) < 1 || count == 1) {
					if (CheckNearByProjectile() && count == 0) {
						Projectile.penetrate = 1;
						count++;
						Projectile.timeLeft = 900;
					}
					if (count == 1) {
						if (CheckNearByProjectile(false)) {
							Projectile.damage += 10;
						}
						Vector2 RandomCir = Main.rand.NextVector2Circular(5f, 5f);
						int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
						Main.dust[dustnumber].noGravity = true;
						Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 3f;
						Projectile.rotation = Projectile.velocity.ToRotation();
					}
				}
			}
		}
		public bool CheckNearByProjectile(bool CheckItSelf = true) {
			int count = 0;
			for (int i = 0; i < Main.maxProjectiles; i++) {
				if (Main.projectile[i].ModProjectile is RubyGemP && CheckItSelf) {
					float Distance = Vector2.Distance(Projectile.Center, Main.projectile[i].Center);
					if (Distance <= 30) {
						count++;
						if (count >= 2) {
							return true;
						}
					}
				}
				if (Main.projectile[i].ModProjectile is RubyBolt) {
					float Distance = Vector2.Distance(Projectile.Center, Main.projectile[i].Center);
					if (Distance <= 30) {
						return true;
					}
				}
			}
			return false;
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 50; i++) {
				Vector2 RandomCir = Main.rand.NextVector2Circular(10f, 10f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
	}
}
