using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	internal class LifeOrb : ModProjectile {
		public override string Texture => BossRushTexture.SMALLWHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.timeLeft = 999;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}
		public override bool? CanDamage() => false;
		public override Color? GetAlpha(Color lightColor) {
			return new Color(0, 255, 0);
		}
		Player player;
		public override void AI() {
			if (Projectile.timeLeft == 999) {
				player = Main.player[Projectile.owner];
				for (int i = 0; i < 50; i++) {
					int startdust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemEmerald);
					Main.dust[startdust].velocity = Main.rand.NextVector2CircularEdge(6, 6);
					Main.dust[startdust].noGravity = true;
				}
			}
			int dust = Dust.NewDust(Projectile.Center - new Vector2(5, 5) + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemEmerald);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			if (Projectile.Center.IsCloseToPosition(player.Center, 125)) {
				Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .1f;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
			}
			else {
				Projectile.velocity *= .98f;
			}
			if (player is not null & Projectile.Center.IsCloseToPosition(player.Center, 25)) {
				player.Heal(2);
				Projectile.Kill();
			}
		}
	}
}