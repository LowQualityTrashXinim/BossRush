using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
public class WindSlashProjectile : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 56;
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
		Projectile.alpha = 255;
	}
	public override void AI() {
		if (Projectile.ai[0] > 1) {
			Projectile.Kill();
		}
		Projectile.scale += .01f;
		Projectile.ai[0] += .01f;
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return lightColor * (1f - Projectile.ai[0]);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.damage > 1) {
			Projectile.damage = (int)(Projectile.damage * .95f);
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
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);
			if (Projectile.direction == -1) {
				drawPos = drawPos.IgnoreTilePositionOFFSET(Projectile.velocity, -24);
			}
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = 0;
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - k * .02f, SpriteEffects.None, 0);
		}
		return false;
	}
}
public class FlyingSlashProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetTheSameTextureAs<FlyingSlashProjectile>("SlashProjectile2");
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 60;
		Projectile.timeLeft = 120;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = 8;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.light = 10f;
		Projectile.ArmorPenetration = 2;
		Projectile.stopsDealingDamageAfterPenetrateHits = true;
		Projectile.extraUpdates = 3;
	}

	public override void SetStaticDefaults() {
	}
	public Color projectileColor = Color.White;
	public override void AI() {
		Projectile.velocity *= .98f;
		Projectile.spriteDirection = Projectile.direction;
		Projectile.rotation = Projectile.velocity.ToRotation();
		if(Projectile.timeLeft < 30) {
			projectileColor *= .8f;
		}
		if (Projectile.spriteDirection == -1) {
			Projectile.rotation += MathHelper.Pi;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawPos, null, projectileColor, Projectile.rotation, origin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
		return false;
	}
}
