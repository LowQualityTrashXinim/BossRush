using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles;
internal class SpiritProjectile : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 25;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.light = 1f;
		Projectile.friendly = true;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = BossRushUtils.ToSecond(100);
	}
	public override void AI() {
		if (Projectile.ai[0] % 2 == 0) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.scale = Main.rand.NextFloat(.85f, 1.25f);
		}
		if (++Projectile.ai[0] > 300) {
			float progress = MathHelper.Lerp(0, 2, Math.Clamp(++Projectile.ai[1] / 300f, 0, 1));
			Projectile.Center.LookForHostileNPC(out NPC npc, 1500, true);
			if (npc != null) {
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * progress;
			}
		}
		if (--Projectile.ai[2] <= 0) {
			Projectile.ai[2] = Main.rand.Next(1, 10) * 50;
			Projectile.spriteDirection *= -1;
		}
		if (Projectile.velocity.IsLimitReached(4))
			Projectile.velocity -= Projectile.velocity * .99f;
		Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(1, 3) * Projectile.spriteDirection));
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 10; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .04f);
		return base.PreDraw(ref lightColor);
	}
}
