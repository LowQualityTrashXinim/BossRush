﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.KnifeRevolver {
	internal class KnifeRevolverSpearProjectile : ModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<KnifeRevolver>();
		public float CollisionWidth => 10f * Projectile.scale;
		public override void SetDefaults() {
			Projectile.Size = new Vector2(30);
			Projectile.scale = 0.85f;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.hide = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 50f;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.Center;
			int duration = (int)(player.itemAnimationMax * .5f);
			DrawOffsetX = -30;
			player.heldProj = Projectile.whoAmI;

			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			if (Projectile.timeLeft > duration) {
				Projectile.timeLeft = duration;
				for (int i = 0; i < 20; i++) {
					int dust = Dust.NewDust(Projectile.Center + Projectile.velocity.Vector2DistributeEvenly(20, 30, i), 0, 0, DustID.GemDiamond);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Projectile.velocity * 5f;
				}
			}
			float halfDuration = duration * 0.5f;
			float progress;
			if (Projectile.timeLeft < halfDuration) {
				progress = Projectile.timeLeft / halfDuration;
			}
			else {
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * 30, Projectile.velocity * 46, progress);
			Projectile.rotation = Projectile.velocity.ToRotation() + (player.direction != 1 ? MathHelper.Pi : 0);
			return false;
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Player player = Main.player[Projectile.owner];
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			SpriteEffects sprite = player.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, sprite, 0);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			if (!player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady) {
				player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady = true;

			}
		}
	}

	public class KnifeRevolverPlayer : ModPlayer {
		public bool SpecialShotReady = false;
		public override void PreUpdate() {
			if (Player.HeldItem.type != ModContent.ItemType<KnifeRevolver>()) {
				return;
			}
			if (Main.mouseRight) {
				Player.HeldItem.UseSound = SoundID.Item1;
			}
			else {
				Player.HeldItem.UseSound = SoundID.Item40;
			}
		}
		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (SpecialShotReady && Player.HeldItem.type == ModContent.ItemType<KnifeRevolver>() && Player.altFunctionUse != 2) {
				type = ModContent.ProjectileType<KnifeRevolverP>();
				damage *= 5;
				SpecialShotReady = false;
			}
		}
	}
}
