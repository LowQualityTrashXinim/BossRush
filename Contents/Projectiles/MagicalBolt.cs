﻿using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
internal class AmethystMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 5000;
		Projectile.penetrate = 99;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		Projectile.penetrate = 1;
		return false;
	}
	public override bool? CanDamage() {
		return null;
	}
	public override bool PreAI() {
		Projectile.velocity *= 10f;
		return base.PreAI();
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemAmethyst);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemAmethyst);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.penetrate != 99) {
			if (Projectile.timeLeft > 60) {
				Projectile.timeLeft = 60;
			}
			Projectile.ProjectileAlphaDecay(60);
		}
		if (++Projectile.ai[0] >= 120) {
			Projectile.velocity *= .998f;
			if (Projectile.velocity.Y < 12) {
				Projectile.velocity.Y += .035f;
			}
		}
	}
	public override void PostAI() {
		Projectile.velocity /= 10f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 100, 255, 150) * Projectile.Opacity, .02f);
		return false;
	}
}

internal class TopazMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000; Projectile.penetrate = 99;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		Projectile.penetrate = 1;
		return false;
	}
	public override bool? CanDamage() {
		return null;
	}
	public override bool PreAI() {
		Projectile.velocity *= 10f;
		return base.PreAI();
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemTopaz);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemTopaz);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.penetrate != 99) {
			if (Projectile.timeLeft > 40) {
				Projectile.timeLeft = 40;
			}
			Projectile.ProjectileAlphaDecay(40);
		}
		if (++Projectile.ai[0] >= 120) {
			Projectile.velocity *= .995f;
		}
	}
	public override void PostAI() {
		Projectile.velocity /= 10f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(225, 205, 255, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
internal class SapphireMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000; Projectile.penetrate = 1;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		return false;
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemSapphire);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemSapphire);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.timeLeft > 400) {
			Projectile.timeLeft = 400;
			Projectile.ai[1] = Main.rand.Next(10, 90);
		}
		Projectile.ProjectileAlphaDecay(400);
		if (++Projectile.ai[0] >= Projectile.ai[1]) {
			Projectile.ai[1] = Main.rand.Next(50, 90);
			Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt();
		}
		Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[2]));
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(100, 100, 255, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
internal class EmeraldMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000; Projectile.penetrate = 3;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		return false;
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemEmerald);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemEmerald);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.timeLeft > 800) {
			Projectile.timeLeft = 800;
			Projectile.ai[1] = Main.rand.Next(90, 150);
			Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt() * .35f;
		}
		Projectile.ProjectileAlphaDecay(800);
		if (++Projectile.ai[0] >= Projectile.ai[1]) {
			Projectile.ai[1] = Main.rand.Next(200, 300);
			Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * .02f;
		}
		Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[2]));
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(100, 255, 100, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
internal class RubyMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000;
		Projectile.penetrate = 5;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		return false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.velocity = (Projectile.Center - target.Center).SafeNormalize(Vector2.Zero) * 2;
		Projectile.velocity = Projectile.velocity.Vector2RotateByRandom(30);
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemRuby);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemRuby);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.timeLeft > 800) {
			Projectile.timeLeft = 800;
			Projectile.ai[1] = Main.rand.Next(90, 150);
		}
		Projectile.ProjectileAlphaDecay(800);
		if (++Projectile.ai[0] >= Projectile.ai[1]) {
			Projectile.ai[1] = Main.rand.Next(200, 300);
			Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * .02f;
		}
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2.5f);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 100, 100, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
internal class DiamondMagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.timeLeft = 1000; Projectile.penetrate = 3;
		Projectile.extraUpdates = 7;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -99999;
		return false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if(Projectile.penetrate == 1) {
			return;
		}
		Projectile.Center = target.Center + Main.rand.NextVector2CircularEdge(200 + target.width, 200 + target.height);
		Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
		for (int i = 0; i < 5; i++) {
			Dust dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GemDiamond);
			dust2.noGravity = true;
			dust2.velocity = Main.rand.NextVector2Circular(10, 10);
		}
	}
	public override void AI() {
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemDiamond);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
		}
		if (Projectile.timeLeft > 800) {
			Projectile.timeLeft = 800;
			Projectile.ai[1] = Main.rand.Next(90, 150);
			Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt() * .35f;
		}
		Projectile.ProjectileAlphaDecay(800);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 255, 255, 110) * Projectile.Opacity, .02f);
		return false;
	}
}
