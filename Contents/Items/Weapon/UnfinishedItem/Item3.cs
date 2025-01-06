using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item3 : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.TheRottedFork)
			.AddIngredient(ItemID.JoustingLance)
			.Register();
	}
}
