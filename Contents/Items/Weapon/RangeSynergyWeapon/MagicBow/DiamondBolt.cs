using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow {
	internal class DiamondBolt : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			Projectile.light = 1f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			Vector2 Rotate = Main.rand.NextVector2CircularEdge(15, 15);
			if(!Projectile.Center.IsCloseToPosition(player.Center, 750f)) {
				Rotate += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * ((player.Center - Projectile.Center).Length() -750f) * .1f;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Rotate, ModContent.ProjectileType<DiamondGemP>(), 0, 0, Projectile.owner);
		}
		public override void AI() {
			int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].fadeIn = 1f;
			if (RicochetOff(out Vector2 pos2)) {
				Projectile.netUpdate = true;
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
		public bool CheckActiveAndCon(Projectile projectileThatNeedtoCheck) {
			Player player = Main.player[Projectile.owner];
			if (projectileThatNeedtoCheck.ModProjectile is DiamondGemP
				&& projectileThatNeedtoCheck.active
				&& (!projectileThatNeedtoCheck.velocity.IsLimitReached(4) || player.ownedProjectileCounts[ModContent.ProjectileType<DiamondGemP>()] > 10)
				&& Vector2.DistanceSquared(player.Center, projectileThatNeedtoCheck.Center) < 2250000) {
				return true;
			}
			return false;
		}
		public List<Projectile> GetListOfActiveProj(out bool Check) {
			List<Projectile> list = new List<Projectile>();
			for (int i = 0; i < Main.maxProjectiles; i++) {
				if (CheckActiveAndCon(Main.projectile[i])) {
					list.Add(Main.projectile[i]);
				}
			}
			Check = list.Count > 1;
			return list;
		}
		public bool RicochetOff(out Vector2 Pos2) {
			List<Projectile> list = GetListOfActiveProj(out bool Check);
			if (Check) {
				Vector2 Pos1;
				foreach (var pos in list.Where(x => Vector2.DistanceSquared(Projectile.Center, x.Center) <= 225)) {
					Pos1 = pos.Center;
					do {
						Pos2 = Main.rand.Next(list).Center;
					}
					while (Pos2 == Pos1);
					pos.Kill();
					return true;
				}
			}
			else {
				if (list.Count == 1) {
					if (Vector2.DistanceSquared(Projectile.Center, list[0].Center) <= 225) {
						list[0].Kill();
						Pos2 = Main.player[Projectile.owner].Center.LookForHostileNPCPositionClosest(1000);
						if(Pos2 == Vector2.Zero)
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
}
