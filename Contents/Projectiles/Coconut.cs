using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
internal class CoconutProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Coconut);
	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		Projectile.frame = 0;
		Projectile.velocity *= .98f;
		if (Projectile.velocity.Y < 20) {
			Projectile.velocity.Y += .75f;
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X)
			Projectile.velocity.X = -oldVelocity.X;
		if (Projectile.velocity.Y != oldVelocity.Y && (Projectile.velocity.Y > .2f || Projectile.velocity.Y < -.2f)) {
			Projectile.velocity.Y = -oldVelocity.Y * .9f;
		}
		return false;
	}
}
