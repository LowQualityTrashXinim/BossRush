using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item7 : SynergyModItem {
	//Crying Night Sky
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Starfury)
			.AddIngredient(ItemID.BloodRainBow)
			.AddIngredient(ItemID.DaedalusStormbow)
			.Register();
	}
}
