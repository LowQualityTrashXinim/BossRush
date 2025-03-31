using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang {
	internal class Bowmarang : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.WoodYoyo);
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(32, 64, 15, 3f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<BowmarangP>(), 20f, false, AmmoID.Arrow);
			Item.crit = 10;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item7;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WoodYoyo)) {
				tooltips.Add(new(Mod, Set_TooltipName(ItemID.WoodYoyo), $"[i:{ItemID.WoodYoyo}] Bowmerang are accompany with a wood yoyo"));
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BowmarangP>(), damage, knockback, player.whoAmI, 1, type);
			CanShootItem = false;
		}
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<BowmarangP>()] < 1;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.WoodenBoomerang)
				.AddRecipeGroup("Wood Bow")
				.Register();
		}
	}
	public class BowmarangP : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<Bowmarang>();
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 999;
			Projectile.scale = .65f;
		}
		float MaxLengthX = 0;
		float MaxLengthY = 0;

		int MouseXPosDirection;
		int maxProgress = 60;
		int progression = 0;

		bool TileCollideJustHappen = false;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (progression % player.itemAnimationMax == 0) {
				float offSetRotate = Projectile.rotation - MathHelper.PiOver4;
				Vector2 aimto = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
				int type = Projectile.ai[1] == ModContent.ProjectileType<BowmarangP>() ? ProjectileID.WoodenArrowFriendly : (int)Projectile.ai[1];
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + offSetRotate.ToRotationVector2(), aimto * 15, type, Projectile.damage, 1f, Projectile.owner);
				Main.projectile[proj].extraUpdates += 1;
			}
			if (Projectile.timeLeft == 999) {
				if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<Bowmarang>(), ItemID.WoodYoyo)) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Bowmarang_WoodYoyo_Projectile>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.whoAmI);
				}
				if (Projectile.ai[0] == 0)
					Projectile.ai[0] = Main.rand.NextBool().ToDirectionInt();
				MaxLengthX = (Main.MouseWorld - player.Center).Length() * 1.5f;
				MaxLengthX = Math.Clamp(MaxLengthX, 600, 750);
				maxProgress += (int)(MaxLengthX * .1f);
				progression = maxProgress;
				MouseXPosDirection = (int)Projectile.ai[0] * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
				MaxLengthY = -(MaxLengthX + Main.rand.NextFloat(-10, 80)) * .35f * MouseXPosDirection;
			}
			Projectile.rotation += MathHelper.ToRadians(35);
			if (TileCollideJustHappen) {
				Projectile.tileCollide = false;
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
				progression--;
				if (Projectile.Center.IsCloseToPosition(player.Center, 50))
					Projectile.Kill();
				return;
			}
			if (player.dead || !player.active || progression <= 0) {
				Projectile.Kill();
			}
			int halfmaxProgress = (int)(maxProgress * .5f);
			int quadmaxProgress = (int)(maxProgress * .25f);
			float progress;
			if (progression > halfmaxProgress) {
				progress = (maxProgress - progression) / (float)halfmaxProgress;
			}
			else {
				progress = progression / (float)halfmaxProgress;
			}
			float X = MathHelper.SmoothStep(-20, MaxLengthX, progress);
			ProgressYHandle(progression, halfmaxProgress, quadmaxProgress, out float Y);
			Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
			Projectile.Center = player.Center + VelocityPosition;
			progression--;
			Projectile.ai[2] = progress;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			TileCollideJustHappen = true;
			return false;
		}
		private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y) {
			if (timeleft > progressMaxHalf + progressMaxQuad) {
				float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
				Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
				return;
			}
			if (timeleft > progressMaxQuad) {
				float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
				Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
				return;
			}
			else {
				float progressY = 1 - timeleft / progressMaxQuad;
				Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
				return;
			}
		}
	}
	public class Bowmarang_WoodYoyo_Projectile : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.WoodYoyo);
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 25;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 999999;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 25;
		}
		public override void AI() {
			if (Projectile.ai[0] < 0 && Projectile.ai[0] > 255) {
				Projectile.Kill();
				return;
			}
			Projectile.spriteDirection = 1;
			Projectile pro = Main.projectile[(int)Projectile.ai[0]];
			if (!pro.active || pro.timeLeft < 0) {
				Projectile.Kill();
				return;
			}
			if (Projectile.timeLeft <= 10) {
				Projectile.timeLeft += 360 * 10;
			}
			Projectile.position = pro.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft)) * pro.ai[2] * 200;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutAlpha(lightColor, .04f);

			return base.PreDraw(ref lightColor);
		}
	}
}
