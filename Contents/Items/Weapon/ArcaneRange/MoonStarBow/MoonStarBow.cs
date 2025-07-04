﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.MoonStarBow {
	internal class MoonStarBow : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(18, 32, 33, 1f, 2, 10, ItemUseStyleID.Shoot, ModContent.ProjectileType<MoonStarProjectile>(), 5f, true);
			Item.reuseDelay = 5;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item75;
			Item.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
		}
		int count = 0;
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			count++;
			position.Y += Main.rand.Next(-900, -800);
			position.X += Main.rand.Next(-300, 300);
			velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			if (count >= 5) {
				Projectile.NewProjectile(source, position, velocity, type, damage * 2, knockback, player.whoAmI);
				count = 0;
			}
			Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(100f, 200f), velocity * .5f, ModContent.ProjectileType<MoonStarProjectileSmaller>(), damage, knockback, player.whoAmI);
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.MoonCharm)
				.AddIngredient(ItemID.DaedalusStormbow)
				.AddIngredient(ItemID.PulseBow)
				.Register();
		}
	}
	class MoonStarProjectile : SynergyModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 46;
			Projectile.wet = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.light = 1f;
			Projectile.extraUpdates = 6;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 3000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 100;
		}
		int ExtraUpdaterReCounter = 0;
		float speedMultiplier = 2;
		int AlphaAdditionalCounter = 255;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			ExtraUpdateRecounter();
			Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speedMultiplier;
			Projectile.rotation += MathHelper.ToRadians(.5f);
		}
		private void ExtraUpdateRecounter() {
			ExtraUpdaterReCounter -= ExtraUpdaterReCounter > 0 ? 1 : 0;
			if (ExtraUpdaterReCounter == 0) {
				Projectile.damage += (int)(Math.Abs(Projectile.velocity.X + Projectile.velocity.Y) * .35f);
				Projectile.alpha++;
				AlphaAdditionalCounter -= AlphaAdditionalCounter > 0 ? -2 : 0;
				if (Projectile.alpha >= 255) Projectile.Kill();
				ExtraUpdaterReCounter = 6;
				speedMultiplier += .01f;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<MoonStarProjectileSmaller>("MoonStarProjectileTrail"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
			Vector2 FullOrigin = new Vector2(Projectile.width, Projectile.height);
			Vector2 threehalfOrigin = origin * .5f;
			Vector2 halfTexture = new Vector2(texture.Width, texture.Height) * .5f * .5f;
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
				Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + (FullOrigin - threehalfOrigin + halfTexture) + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(0, 0, 255, Math.Abs(AlphaAdditionalCounter) / k));
				Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, Projectile.scale - (k - 1) * .01f, SpriteEffects.None, 0);
			}
			for (int k = 1; k < (int)(Projectile.oldPos.Length * .5f) + 1; k++) {
				Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + (FullOrigin - threehalfOrigin - halfTexture) + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(255, 255, 255, Math.Abs(AlphaAdditionalCounter) / k));
				Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, (Projectile.scale - (k - 1) * .02f) * .5f, SpriteEffects.None, 0);
			}
			Texture2D thisProjectiletexture = TextureAssets.Projectile[Projectile.type].Value;
			Color fullwhite = new Color(255, 255, 255, 30);
			Vector2 thisProjectiledrawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(thisProjectiletexture, thisProjectiledrawPos, null, fullwhite, -Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(thisProjectiletexture, thisProjectiledrawPos, null, fullwhite, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			Vector2 Rotate;
			float randomRotation = Main.rand.NextFloat(90);
			for (int i = 0; i < 25; i++) {
				Rotate = Main.rand.NextVector2CircularEdge(5.5f, 5.5f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Rotate.X, Rotate.Y, 0, Color.Blue, Main.rand.NextFloat(1f, 2.5f));
				Main.dust[dustnumber].noGravity = true;
			}
			for (int i = 0; i < 15; i++) {
				Rotate = Main.rand.NextVector2CircularEdge(3f, 3f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Rotate.X, Rotate.Y, 0, Color.Blue, Main.rand.NextFloat(1.25f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
			}
			for (int i = 0; i < 100; i++) {
				if (i % 2 == 0) {
					Rotate = Main.rand.NextVector2CircularEdge(.5f, 10f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 2;
				}
				else {
					Rotate = Main.rand.NextVector2CircularEdge(10f, .5f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 2;
				}
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Rotate.X, Rotate.Y, 0, Color.Blue, Main.rand.NextFloat(1.25f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
	}
	class MoonStarProjectileSmaller : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAs<MoonStarBow>("MoonStarProjectile");
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			base.SetStaticDefaults();
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 46;
			Projectile.light = .5f;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1500;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.wet = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 5;
		}
		int ExtraUpdaterReCounter = 0;
		float speedMultiplier = 2;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.scale = .5f;
			ExtraUpdateRecounter();
			AttackHomeIn();
		}
		int changedirection = Main.rand.Next(new int[] { 1, -1 });
		private void AttackHomeIn() {
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 250)) {
				if (npc == null) {
					return;
				}
				speedMultiplier = Projectile.velocity.Length();
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * speedMultiplier;
			}
			else {
				if (count >= 12) {
					float rand = MathHelper.ToRadians(Main.rand.NextFloat(1, 5)) * changedirection * .1666f;
					Projectile.velocity = Projectile.velocity.RotatedBy(rand);
				}
			}
		}
		int count = 0;
		private void ExtraUpdateRecounter() {
			ExtraUpdaterReCounter -= ExtraUpdaterReCounter > 0 ? 1 : 0;
			if (ExtraUpdaterReCounter == 0) {
				ExtraUpdaterReCounter = 6;
				speedMultiplier += .01f;
				count++;
				if (count % 20 == 0) {
					changedirection *= -1;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<MoonStarProjectileSmaller>("MoonStarProjectileTrail"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 origin = new Vector2(Projectile.width * .5f, Projectile.height * .5f);
			Vector2 offsetOriginbyQuad = origin * .33f;
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
				Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + offsetOriginbyQuad + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(0, 0, 255, 255 / k));
				Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, Projectile.scale - (k - 1) * .5f * .02f, SpriteEffects.None, 0);
			}
			Texture2D textureThis = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPosThis = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(textureThis, drawPosThis, null, Projectile.GetAlpha(new Color(255, 255, 255)), 0, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			for (int i = 0; i < 30; i++) {
				Vector2 vec = Main.rand.NextVector2Unit(MathHelper.PiOver4, MathHelper.PiOver2) * Main.rand.NextFloat(3, 5) * -1f;
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0, 0, 0, Color.Blue, Main.rand.NextFloat(.9f, 1.1f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = vec.RotatedBy(MathHelper.ToRadians(90 * (i % 4)));
			}
			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Vortex, 0, 0, 0, Color.Blue, Main.rand.NextFloat(1.35f, 1.5f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(7f, 7f);
			}
		}
	}
}
