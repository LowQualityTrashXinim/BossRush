using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
public class WindSlashProjectile : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.friendly = true;
		Projectile.penetrate = 5;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 6;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}
	public override void AI() {
		Projectile.alpha += 255 / 50;
		Projectile.Size += new Vector2(0.05f, 0.05f);
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.damage > 1) {
			Projectile.damage = (int)(Projectile.damage * .8f);
		}
		target.immune[Projectile.owner] = 4;
	}
	bool hittile = false;
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (!hittile) {
			Projectile.position += Projectile.velocity;
		}
		hittile = true;
		Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
		Projectile.velocity = Vector2.Zero;
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .02f);
		return true;
	}
}
