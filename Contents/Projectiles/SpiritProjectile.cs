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
		if (++Projectile.ai[0] > 300) {
			float progress = MathHelper.Lerp(0, 2, Math.Clamp(++Projectile.ai[1] / 300f, 0, 1));
			Projectile.Center.LookForHostileNPC(out NPC npc, 1500, true);
			if (npc != null) {
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * progress;
			}
		}
		Projectile.velocity *= .99f;
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
