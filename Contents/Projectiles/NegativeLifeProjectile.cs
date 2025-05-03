using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	internal class NegativeLifeProjectile : ModProjectile {
		public override string Texture => BossRushTexture.SMALLWHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 15;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.Center.IsCloseToPosition(player.Center, 30)) {
				if (player.statLife - 20 < 0) {
					player.statLife = 1;
				}
				else {
					player.statLife -= 20;
				}
				Projectile.Kill();
			}
		}
		public override Color? GetAlpha(Color lightColor) {
			return Color.Black;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor);
			return base.PreDraw(ref lightColor);
		}
	}
}
