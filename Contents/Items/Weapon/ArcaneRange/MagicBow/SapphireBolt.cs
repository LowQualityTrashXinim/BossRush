using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.MagicBow {
	internal class SapphireBolt : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 15;
			Projectile.height = 15;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			Projectile.light = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			for (int i = 0; i < 20; i++) {
				Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, 0, 0, 0, default, Main.rand.NextFloat(1.5f, 2.25f));
				Main.dust[dustnumber].noGravity = true;
				Main.dust[dustnumber].velocity = RandomCircular;
			}
			Projectile.damage += 5;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X) {
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y) {
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
		public override void AI() {
			for (int i = 0; i < 3; i++) {
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
				Main.dust[dustnumber].velocity = new Vector2(Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			for (int i = 0; i < 60; i++) {
				Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, RandomCircular.X, RandomCircular.Y, 0, default, Main.rand.NextFloat(1.5f, 2.25f));
				Main.dust[dustnumber].noGravity = true;
			}
			if (target.life >= (int)(target.lifeMax * 0.5f)) {
				Projectile.Kill();
			}
			else {
				Projectile.damage += 5;
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SapphireGemP>()] < 20) {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(40f, 40f), ModContent.ProjectileType<SapphireGemP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor);
			return true;
		}
	}
}
