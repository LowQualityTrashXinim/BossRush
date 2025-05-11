using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
internal class WindShot : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 1200;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
		Projectile.extraUpdates = 10;
	}
	public override void AI() {
		if (Projectile.velocity.LengthSquared() < 25) {
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 5f;
		}
		if (Projectile.timeLeft % 2 == 0) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(.2f, .4f));
			dust.velocity = Main.rand.NextVector2Circular(2, 2);
			dust.noGravity = true;
			dust.rotation = Main.rand.NextFloat();
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Color color = new(255, 255, 255, 0);
		Projectile.DrawTrail(color, .02f);
		return base.PreDraw(ref lightColor);
	}
}
