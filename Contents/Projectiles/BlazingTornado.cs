using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
internal class BlazingTornado : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.MonkStaffT2);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = 116;
		Projectile.height = 120;
		Projectile.timeLeft = BossRushUtils.ToSecond(12) * 50;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 30;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		float length = Projectile.width * .5f;
		Projectile.rotation = MathHelper.ToRadians(-Projectile.timeLeft) * 5;
		Vector2 rotationVec = Projectile.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver4);
		Projectile.Center = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(-Projectile.timeLeft / 4)) * 100;
		Vector2 pos = Projectile.Center.PositionOFFSET(rotationVec, Main.rand.NextFloat(-length, length));
		if (Projectile.timeLeft % 800 == 0) {
			Projectile.NewProjectile(
				Projectile.GetSource_FromAI(),
				pos,
				Main.rand.NextVector2CircularEdge(10, 10),
				ProjectileID.BallofFire, (int)(Projectile.damage * .34f), 3f, Projectile.owner);
		}
		Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FlameBurst);
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor);
		return base.PreDraw(ref lightColor);
	}
}
