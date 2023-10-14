using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BossRush.Contents.Projectiles;
internal class MagicboltProjectile : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.friendly  = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 600;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.MagicMirror);
		Main.dust[dust].noGravity = true;
	}
	public override Color? GetAlpha(Color lightColor) {
		return new Color(150, 150, 255);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .05f);
		return base.PreDraw(ref lightColor);
	}
}
