using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
		Projectile.timeLeft = 3600;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
		Projectile.extraUpdates = 25;
	}
	public override void AI() {
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 3;
		if(Projectile.timeLeft % 25 == 0) {
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(1));
		}
		Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 1 / (float)Projectile.extraUpdates;
	}
	public override bool PreDraw(ref Color lightColor) {
		lightColor = new(255, 255, 255, 0);
		lightColor *= (Projectile.timeLeft / 3600f);
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = (byte)Projectile.alpha;
			float scale = k / (float)(Projectile.oldPos.Length);
			if (k > Projectile.oldPos.Length * .5f) {
				scale = 1 - k / (float)(Projectile.oldPos.Length);
			}
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0);
		}
		return false;
	}
}
