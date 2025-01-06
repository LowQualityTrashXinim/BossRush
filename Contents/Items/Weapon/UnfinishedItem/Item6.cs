using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item6 : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PulseBow)
			.AddIngredient(ItemID.Megashark)
			.Register();
	}
}
