using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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
internal class StarWarSwordProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BluePhaseblade);
	public int ItemTextureID = ItemID.BluePhaseblade;
	public Color ColorOfSaber = Color.White;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = 48;
		Projectile.height = 48;
		Projectile.timeLeft = BossRushUtils.ToSecond(.5f) * 30;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 30;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		float length = Projectile.width * .5f;
		Projectile.rotation = MathHelper.ToRadians(-Projectile.timeLeft) * 5;
		if (Projectile.timeLeft <= BossRushUtils.ToSecond(0.4f) * 30) {
			Projectile.velocity *= .99f;
		}
		Projectile.scale = (float)Math.Clamp((1 * (Projectile.timeLeft / 200f)), 0, 1);
		ColorOfSaber.A = 0;
		Dust dust = Dust.NewDustDirect(Projectile.Center.PositionOFFSET((Projectile.rotation - MathHelper.PiOver4).ToRotationVector2(), Projectile.width * .7f * Projectile.scale) - Vector2.One * 2.5f, 0, 0, DustID.WhiteTorch, newColor: ColorOfSaber);
		dust.noGravity = true;
		dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(2, 3);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.immune[Projectile.owner] = 5;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(ItemTextureID);
		Texture2D texture = TextureAssets.Item[ItemTextureID].Value;
		Vector2 origin = texture.Size() * .5f;
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * .5f;
			color.A = (byte)Projectile.alpha;
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.None, 0);
		}
		return true;
	}
}
