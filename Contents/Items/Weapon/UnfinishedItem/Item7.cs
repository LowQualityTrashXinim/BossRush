using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item7 : SynergyModItem {

	/*
	 * "The Orbit"
	 * Throw out a special boomerang that have a ball of flame orbit around it
	 * After 4th throw, the next throw will release 3 ball of flame orbit at different radius
	 */
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.EnchantedBoomerang)
			.AddIngredient(ItemID.FlamingMace)
			.Register();
	}
}
