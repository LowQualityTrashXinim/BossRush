using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles;
internal class SwordProjectile : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
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
	Vector2 oldCenter = Vector2.Zero;
	public override void AI() {
		EnergySword_Code1AI();
	}
	private void EnergySword_Code1AI() {
		if (Projectile.timeLeft > 30) {
			player = Main.player[Projectile.owner];
			directionToMouse = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
			oldCenter = Projectile.Center.PositionOFFSET(directionToMouse, -30);
			Projectile.timeLeft = 30;
			directionLooking = Main.rand.NextBool().ToDirectionInt();
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
		Projectile.Center = oldCenter + Vector2.UnitX.RotatedBy(rotation) * 60f;
	}
	public override void ModifyDamageHitbox(ref Rectangle hitbox) {
		BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, oldCenter, outrotation, Projectile.width, Projectile.height, 10f);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
internal class SwordProjectile2 : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.extraUpdates = 5;
	}
	public int ItemIDtextureValue = ItemID.WoodenSword;
	Vector2 vel = Vector2.Zero;
	public float Counter { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float State { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public override bool? CanDamage() {
		return State != 1;
	}
	public override void AI() {
		if (State == 1) {
			if (Projectile.timeLeft > 300) {
				Projectile.timeLeft = 300;
			}
			Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / 300f);
			Projectile.velocity = Vector2.Zero;
			return;
		}
		if (Projectile.timeLeft > 900 && Counter >= 0) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			vel = Projectile.velocity;
			Projectile.timeLeft = 900;
			Counter = 120 + Projectile.ai[2];
			Projectile.velocity = Vector2.Zero;
		}
		if (--Counter < 0) {
			Projectile.timeLeft = 900;
			Projectile.velocity += vel * .005f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(9);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		State = 1;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (State != 1) {
			State = 1;
			Projectile.position += Projectile.velocity * 2;
			Projectile.velocity = Vector2.Zero;
		}
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = texture.Size() * .5f;
		if (State != 1) {
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos2 = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * .5f;
				Main.EntitySpriteDraw(texture, drawPos2, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}
		}
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
internal class SwordProjectile3 : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.timeLeft = 999;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.scale = 1.5f;
		Projectile.usesLocalNPCImmunity = true;
	}
	public int ItemIDtextureValue = ItemID.WoodenSword;
	public float ProjectileIndex { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
	public float RotationValue { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public override void AI() {
		if (Projectile.timeLeft == 999) {
			Projectile.velocity = Vector2.Zero;
		}
		RotationValue += 10;
		Vector2 RotationPos = Vector2.One.RotatedBy(MathHelper.ToRadians(ProjectileIndex * 120 + RotationValue)) * 50f;
		Vector2 NewPos = Main.player[Projectile.owner].Center + RotationPos;
		Projectile.Center = NewPos;
		Projectile.rotation = RotationPos.ToRotation() + MathHelper.PiOver4;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
