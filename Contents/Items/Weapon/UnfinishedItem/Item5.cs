using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item5 : SynergyModItem {
	//Keybroad Hero's Weapon
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicalHarp)
			.AddIngredient(ItemID.Keybrand)
			.Register();
	}
}
