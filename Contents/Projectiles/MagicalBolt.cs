using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
internal class AmethystMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 5000;
		Projectile.penetrate = 99;
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
	public override bool PreAI() {
		Projectile.velocity *= 10f;
		return base.PreAI();
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemAmethyst);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10,10), 0, 0, DustID.GemAmethyst);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.penetrate != 99) {
			if (Projectile.timeLeft > 60) {
				Projectile.timeLeft = 60;
			}
			Projectile.ProjectileAlphaDecay(60);
		}
		if (++Projectile.ai[0] >= 120) {
			Projectile.velocity *= .998f;
			if (Projectile.velocity.Y < 12) {
				Projectile.velocity.Y += .035f;
			}
		}
	}
	public override void PostAI() {
		Projectile.velocity /= 10f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 100, 255, 150) * Projectile.Opacity, .02f);
		return false;
	}
}

internal class TopazMagicalBolt : ModProjectile {
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
	public override bool PreAI() {
		Projectile.velocity *= 10f;
		return base.PreAI();
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemTopaz);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemTopaz);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.penetrate != 99) {
			if (Projectile.timeLeft > 40) {
				Projectile.timeLeft = 40;
			}
			Projectile.ProjectileAlphaDecay(40);
		}
		if (++Projectile.ai[0] >= 120) {
			Projectile.velocity *= .995f;
		}
	}
	public override void PostAI() {
		Projectile.velocity /= 10f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(225, 205, 255, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
