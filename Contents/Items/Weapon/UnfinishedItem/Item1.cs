using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item1 : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Musket)
			.AddIngredient(ItemID.TheUndertaker)
			.Register();
	}
}
