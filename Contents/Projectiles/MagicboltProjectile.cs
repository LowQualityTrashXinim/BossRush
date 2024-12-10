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
internal class EnergyBoltProjectile : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000; Projectile.penetrate = 99;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		Projectile.penetrate = 1;
		return false;
	}
	public override bool? CanDamage() {
		return null;
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemSapphire);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemSapphire);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.penetrate != 99) {
			if (Projectile.timeLeft > 40) {
				Projectile.timeLeft = 40;
			}
			Projectile.ProjectileAlphaDecay(40);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(205, 225, 255, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
