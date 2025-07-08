using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles;
public class MaceBallProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Mace);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X * 0.85f;
		}
		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
		}
		return false;
	}
	public override void AI() {
		Projectile.rotation = Projectile.direction * MathHelper.ToRadians(Projectile.timeLeft * -10 - Projectile.velocity.Length());
		if (++Projectile.ai[0] <= 10) {
			return;
		}
		if (!Projectile.wet) {
			if (Projectile.velocity.Y <= 20)
				Projectile.velocity.Y += .5f;
		}
		else {
			if (Projectile.velocity.Y >= -10)
				Projectile.velocity.Y -= .5f;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		int type = (int)Projectile.ai[2];
		if (type < 0 || type > ProjectileID.Count) {
			return base.PreDraw(ref lightColor);
		}
		Main.instance.LoadProjectile(type);
		Texture2D texture = TextureAssets.Projectile[type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.position - Main.screenPosition + origin;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return base.PreDraw(ref lightColor);
	}
}
