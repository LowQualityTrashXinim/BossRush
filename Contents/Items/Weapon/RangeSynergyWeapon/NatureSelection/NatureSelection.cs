using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection {
	internal class NatureSelection : SynergyModItem {
		static int counter = 0;
		public override void SetDefaults() {
			Item.BossRushDefaultRange(32, 66, 40, 3f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 20, true, AmmoID.Arrow);
			Item.rare = 2;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item5;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 RotatePos = Main.rand.NextVector2Circular(75f, 75f) * 2 + position;
			Vector2 AimPos = Main.MouseWorld - RotatePos;
			Vector2 safeAim = AimPos.SafeNormalize(Vector2.UnitX) * Main.rand.Next(14, 21);
			int bowType = 0;
			switch (counter) {
				case 0:
					bowType = ModContent.ProjectileType<WoodBowP>();
					break;
				case 1:
					bowType = ModContent.ProjectileType<BorealWoodBowP>();
					break;
				case 2:
					bowType = ModContent.ProjectileType<RichMahoganyBowP>();
					break;
				case 3:
					bowType = ModContent.ProjectileType<PalmWoodBowP>();
					break;
				case 4:
					bowType = ModContent.ProjectileType<EbonwoodBowP>();
					break;
				case 5:
					bowType = ModContent.ProjectileType<ShadewoodBowP>();
					break;
			}
			Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), bowType, damage, knockback, player.whoAmI);
			counter++;
			if (counter > 5) {
				counter = 0;
			}
			return true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-4, 0);
		}
		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.WoodenBow, 1)
			.AddIngredient(ItemID.BorealWoodBow, 1)
			.AddIngredient(ItemID.RichMahoganyBow, 1)
			.AddIngredient(ItemID.PalmWoodBow, 1)
			.AddIngredient(ItemID.EbonwoodBow, 1)
			.AddIngredient(ItemID.ShadewoodBow, 1)
			.AddIngredient(ItemID.PearlwoodBow, 1)
			.Register();
		}
	}
}