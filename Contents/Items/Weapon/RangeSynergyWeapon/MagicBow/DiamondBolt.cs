using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow {
	internal class DiamondBolt : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
			Projectile.light = 1f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			//to avoid having too much projectile, we capping this at 50
			if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondGemP>()] <= 50) {
				Vector2 Rotate = Projectile.Center + Main.rand.NextVector2CircularEdge(300, 300) + Main.rand.NextVector2Circular(30, 30) * (10 + Main.rand.NextFloat(5));
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Rotate, Vector2.Zero, ModContent.ProjectileType<DiamondGemP>(), 0, 0, Projectile.owner);
				player.GetModPlayer<DiamondGemProjectilePlayerTracker>().list_DiamondProjectile.Add(Main.projectile[proj]);
			}
		}
		public override void AI() {
			int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].fadeIn = 1f;
			if (RicochetOff(Main.player[Projectile.owner], out Vector2 pos2)) {
				Projectile.damage += 10;
				Projectile.CritChance += 5;
				if (pos2 != Vector2.Zero) {
					Projectile.velocity = (pos2 - Projectile.position).SafeNormalize(Vector2.UnitX) * 10;
				}
				for (int i = 0; i < 15; i++) {
					Vector2 ReverseVelSpread = -Projectile.velocity * 2 + Main.rand.NextVector2Circular(5f, 5f);
					int dustnumber2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, ReverseVelSpread.X, ReverseVelSpread.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
					Main.dust[dustnumber2].noGravity = true;
				}
			}
		}
		public bool RicochetOff(Player player, out Vector2 Pos2) {
			List<Projectile> list = player.GetModPlayer<DiamondGemProjectilePlayerTracker>().list_DiamondProjectile;
			bool Check = list.Count > 1;
			if (Check) {
				Vector2 Pos1;
				foreach (var pos in list.Where(x => Vector2.DistanceSquared(Projectile.Center, x.Center) <= 225)) {
					Pos1 = pos.Center;
					int failsafe = 0;
					do {
						Pos2 = Main.rand.Next(list).Center;
						failsafe++;
					}
					while (Pos2 == Pos1 && failsafe <= 100);
					if(failsafe == 100) {
						Pos2 = Vector2.Zero;
					}
					pos.Kill();
					return true;
				}
			}
			else {
				if (list.Count == 1) {
					if (Vector2.DistanceSquared(Projectile.Center, list[0].Center) <= 225) {
						list[0].Kill();
						Pos2 = Main.player[Projectile.owner].Center.LookForHostileNPCPositionClosest(1000);
						if (Pos2 == Vector2.Zero)
							Pos2 = Projectile.Center.LookForHostileNPCPositionClosest(1000);
						return true;
					}
				}
			}
			Pos2 = Vector2.Zero;
			return false;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(Projectile.GetAlpha(lightColor), 0.01f);
			return true;
		}
	}
	public class DiamondGemProjectilePlayerTracker : ModPlayer {
		public List<Projectile> list_DiamondProjectile = new();
		public override void ResetEffects() {
			for (int i = list_DiamondProjectile.Count - 1; i >= 0; i--) {
				Projectile projectile = list_DiamondProjectile[i];
				if (projectile == null) {
					list_DiamondProjectile.RemoveAt(i);
					continue;
				}
				if (!projectile.active) {
					list_DiamondProjectile.RemoveAt(i);
				}
			}
		}
	}
}
