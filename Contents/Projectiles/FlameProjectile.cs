using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace BossRush.Contents.Projectiles;

public class FlameProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 7;
	}
	public Color FlameColor = Color.White;
	public int DebuffType = 0;
	public override Color? GetAlpha(Color lightColor) {
		return FlameColor;
	}
	public override void SetDefaults() {
		Projectile.width = 6;
		Projectile.height = 6;
		Projectile.friendly = true;
		Projectile.penetrate = 4;
		Projectile.extraUpdates = 2;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
	}
	public void SelectFrame() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}
	//Straight outta terraria source code
	public override void AI() {
		SelectFrame();
		Projectile.localAI[0] += 1f;
		int num = 60;
		int num2 = 12;
		int num3 = num + num2;
		if (Projectile.localAI[0] >= num3)
			Projectile.Kill();

		if (Projectile.localAI[0] >= num)
			Projectile.velocity *= 0.95f;
		int num4 = 50;
		int num5 = num4;

		if (Projectile.localAI[0] < num5 && Main.rand.NextBool(4)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(60f, 60f) * Utils.Remap(Projectile.localAI[0], 0f, 72f, 0.5f, 1f), 4, 4, DustID.WhiteTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
			if (Main.rand.NextBool(4)) {
				dust.noGravity = true;
				dust.scale *= 3f;
				dust.velocity.X *= 2f;
				dust.velocity.Y *= 2f;
			}
			else {
				dust.scale *= 1.5f;
			}
			dust.color = FlameColor;
			dust.scale *= 1.5f;
			dust.velocity *= 1.2f;
			dust.velocity += Projectile.velocity * 1f * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.75f, 1f, 0.1f) * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.1f, 0.1f, 1f);
			dust.customData = 1;
		}

		if (num4 > 0 && Projectile.localAI[0] >= num4 && Main.rand.NextFloat() < 0.5f) {
			Vector2 center = Main.player[Projectile.owner].Center;
			Vector2 vector = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
			Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, DustID.Smoke, 0f, 0f, 150, FlameColor);
			dust2.noGravity = true;
			dust2.velocity = vector;
			dust2.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
			dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (DebuffType <= 0) {
			return;
		}
		target.AddBuff(DebuffType, BossRushUtils.ToSecond(4));
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile proj = Projectile;
		float num = 60f;
		float num2 = 12f;
		float fromMax = num + num2;
		Texture2D value = TextureAssets.Projectile[Type].Value;
		float num6 = (proj.localAI[0] > num - 10f) ? 0.175f : 0.2f;
		Color transparent;
		Color color = Projectile.GetAlpha(lightColor);
		Color color2 = BossRushUtils.FakeHueShift(color, 10);
		Color color3 = Color.Lerp(BossRushUtils.FakeHueShift(color, 20), color2, 0.25f);
		Color color4 = new Color(80, 80, 80, 100);
		float num3 = 0.35f;
		float num4 = 0.7f;
		float num5 = 0.85f;

		int verticalFrames = 7;
		float num9 = Utils.Remap(proj.localAI[0], num, fromMax, 1f, 0f);
		float num10 = Math.Min(proj.localAI[0], 20f);
		float num11 = Utils.Remap(proj.localAI[0], 0f, fromMax, 0f, 1f);
		float num12 = Utils.Remap(num11, 0.2f, 0.5f, 0.25f, 1f);
		Rectangle rectangle = value.Frame(1, verticalFrames, 0, 3);
		if (!(num11 < 1f))
			return false;

		for (int i = 0; i < 2; i++) {
			for (float num13 = 1f; num13 >= 0f; num13 -= num6) {
				transparent = ((num11 < 0.1f) ? Color.Lerp(Color.Transparent, color, Utils.GetLerpValue(0f, 0.1f, num11, clamped: true)) : ((num11 < 0.2f) ? Color.Lerp(color, color2, Utils.GetLerpValue(0.1f, 0.2f, num11, clamped: true)) : ((num11 < num3) ? color2 : ((num11 < num4) ? Color.Lerp(color2, color3, Utils.GetLerpValue(num3, num4, num11, clamped: true)) : ((num11 < num5) ? Color.Lerp(color3, color4, Utils.GetLerpValue(num4, num5, num11, clamped: true)) : ((!(num11 < 1f)) ? Color.Transparent : Color.Lerp(color4, Color.Transparent, Utils.GetLerpValue(num5, 1f, num11, clamped: true))))))));
				float num14 = (1f - num13) * Utils.Remap(num11, 0f, 0.2f, 0f, 1f);
				Vector2 vector = proj.Center - Main.screenPosition + proj.velocity * (0f - num10) * num13;
				Color color5 = transparent * num14;
				Color color6 = color5;

				float num15 = 1f / num6 * (num13 + 1f);
				float num16 = proj.rotation + num13 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num15 * 2f;
				float num17 = proj.rotation - num13 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num15 * 2f;
				Main.EntitySpriteDraw(value, vector + proj.velocity * (0f - num10) * num6 * 0.5f, rectangle, color6 * num9 * 0.25f, num16 + MathHelper.PiOver4, rectangle.Size() / 2f, num12, SpriteEffects.None);
				Main.EntitySpriteDraw(value, vector, rectangle, color6 * num9, num17, rectangle.Size() / 2f, num12, SpriteEffects.None);

			}
		}
		return false;
	}
}
