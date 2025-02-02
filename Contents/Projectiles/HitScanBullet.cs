using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles;
internal class HitScanBullet : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.penetrate = 3;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 10;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if (Projectile.timeLeft < 9 || Projectile.penetrate <= 1) {
			return false;
		}
		return BossRushUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 1000));
	}
	public override void AI() {
		if (Projectile.timeLeft == 10) {
			Vector2 toMouse = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.velocity = Vector2.Zero;
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] = toMouse.X;
			Projectile.ai[1] = toMouse.Y;
			Projectile.Center = player.Center;
			Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;
		}
		Projectile.scale -= .1f;
	}
	Vector2 ToMouseDirection => new(Projectile.ai[0], Projectile.ai[1]);
	public override bool PreDraw(ref Color lightColor) {
		//Ain't the best way
		for (int i = 0; i < 4; i++) {
			Vector2 drawpos = Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 200 * i) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.White, Projectile.rotation, Vector2.One * .5f, Projectile.scale, SpriteEffects.None);
		}
		return false;
	}
}
