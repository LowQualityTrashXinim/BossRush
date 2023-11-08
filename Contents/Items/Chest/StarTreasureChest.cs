using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class StarTreasureChest : ModItem {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes() {
			Recipe recipe = Recipe.Create(ItemID.FallenStar, 100);
			recipe.AddIngredient(ModContent.ItemType<StarTreasureChest>());
			recipe.Register();
		}
	}
}
