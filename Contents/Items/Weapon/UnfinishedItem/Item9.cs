using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item9 : SynergyModItem {
	//Aqua Rifle
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Musket)
			.AddIngredient(ItemID.AquaScepter)
			.Register();
	}
}
