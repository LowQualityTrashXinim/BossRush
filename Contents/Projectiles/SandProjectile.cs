using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	internal class SandProjectile : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SandBallGun);
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.SandBallGun);
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			if (Main.rand.NextBool(10))
				Dust.NewDust(Projectile.Center, 10, 10, DustID.Sand);
			if (Projectile.ai[0] >= 50) {
				if (Projectile.velocity.Y < 16) {
					Projectile.velocity.Y += .25f;
				}
			}
			Projectile.ai[0]++;
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.Sand);
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
			}
		}
	}
}