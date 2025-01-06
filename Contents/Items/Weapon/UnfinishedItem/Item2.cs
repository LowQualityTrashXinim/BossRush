using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item2 : SynergyModItem {
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.CobaltSword)
			.AddIngredient(ItemID.ShadowFlameKnife)
			.Register();
	}
}
