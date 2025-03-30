using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol {
	internal class HeartPistol : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(26, 52, 16, 3f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<HeartP>(), 10, false, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			position = position.PositionOFFSET(velocity, 30);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HeartP>(), damage, knockback, player.whoAmI);
			CanShootItem = false;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-2, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.BandofRegeneration)
			.Register();
		}
	}
}
