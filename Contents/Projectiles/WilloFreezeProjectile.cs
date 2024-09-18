using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
internal class WilloFreezeProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BallofFrost);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
	}
	public bool OnFirstFrame = false;
	public float OffSetPos { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float Index { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public float ProjectileAmount { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
	private float ProjectileDuration = 0;
	private Vector2 startingPosition;
	public override void AI() {
		if (!OnFirstFrame) {
			ProjectileDuration = Projectile.timeLeft;
			startingPosition = Projectile.Center;
			OnFirstFrame = true;
		}
		Projectile.Center = startingPosition + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft * 2 + 360 / (float)ProjectileAmount * Index)) * ((ProjectileDuration - Projectile.timeLeft) / 4f + OffSetPos);
		Dust dust = Dust.NewDustDirect(Projectile.position, 16, 16, DustID.Frost);
		dust.velocity = Vector2.Zero;
		dust.noGravity = true;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(9, 18)));
	}
}
