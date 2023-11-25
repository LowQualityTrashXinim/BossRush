using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles;
internal class WoodSwordProjectile : ModProjectile {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.scale = 1.5f;
		Projectile.usesLocalNPCImmunity = true;
	}
	public int ItemIDtextureValue = ItemID.WoodenSword;
	Vector2 directionToMouse = Vector2.Zero;
	Player player;
	float outrotation = 0;
	int directionLooking = 1;
	public override void AI() {
		EnergySword_Code1AI();
	}
	private void EnergySword_Code1AI() {
		if (Projectile.timeLeft > 30) {
			player = Main.player[Projectile.owner];
			directionToMouse = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
			Projectile.timeLeft = 30;
			directionLooking = player.direction;
		}
		float percentDone = Projectile.timeLeft / 30f;
		percentDone = Math.Clamp(BossRushUtils.InExpo(percentDone), 0, 1);
		Projectile.spriteDirection = directionLooking;
		float baseAngle = directionToMouse.ToRotation();
		float angle = MathHelper.ToRadians(150) * directionLooking;
		float start = baseAngle + angle;
		float end = baseAngle - angle;
		float rotation = MathHelper.Lerp(start, end, percentDone);
		outrotation = rotation;
		Projectile.rotation = rotation + MathHelper.PiOver4;
		Projectile.velocity.X = directionLooking;
		Projectile.Center = player.Center + Vector2.UnitX.RotatedBy(rotation) * 60f;
	}
	public override void ModifyDamageHitbox(ref Rectangle hitbox) {
		BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, player, outrotation, Projectile.width, Projectile.height, 10f);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
