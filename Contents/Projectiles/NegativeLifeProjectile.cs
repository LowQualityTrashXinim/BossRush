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
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor);
			return base.PreDraw(ref lightColor);
		}
	}
}