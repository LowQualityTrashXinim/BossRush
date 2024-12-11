using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang {
	internal class Bowmarang : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(32, 64, 15, 3f, 15, 15, ItemUseStyleID.Shoot, ModContent.ProjectileType<BowmarangP>(), 20f, false, AmmoID.Arrow);
			Item.crit = 10;
			Item.noUseGraphic = true;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			base.ModifySynergyToolTips(ref tooltips, modplayer);
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			base.HoldSynergyItem(player, modplayer);
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
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + offSetRotate.ToRotationVector2(), aimto * 9, type, Projectile.damage, 1f, Projectile.owner);
			}
			if (Projectile.timeLeft == 999) {
				if (Projectile.ai[0] == 0)
					Projectile.ai[0] = Main.rand.NextBool().ToDirectionInt();
				MaxLengthX = (Main.MouseWorld - player.Center).Length() * 1.5f;
				maxProgress += (int)(MaxLengthX * .05f);
				progression = maxProgress;
				MouseXPosDirection = (int)Projectile.ai[0] * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
				MaxLengthY = -(MaxLengthX + Main.rand.NextFloat(-10, 80)) * .25f * MouseXPosDirection;
			}
			Projectile.rotation += MathHelper.ToRadians(25);
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
}
