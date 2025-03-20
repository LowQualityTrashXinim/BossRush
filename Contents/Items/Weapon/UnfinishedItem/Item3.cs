using BossRush.Texture;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item3 : SynergyModItem {
	/*
	 * "Genshin ahh weapon"
	 * Each attack grant you "name 1" stack
	 * for each "name 1" stack, increases your base damage by 2
	 * At 10th stack, you will unleash "name 2" for short while
	 * During "name 2" your attack cause "name 3" to appear
	 */
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.TheRottedFork)
			.AddIngredient(ItemID.DemonScythe)
			.Register();
	}
}
public class Item3_ModPlayer : ModPlayer {
	public int name1 = 0;
	public override void ResetEffects() {
		if (!Player.IsHeldingModItem<Item3>()) {
			name1 = 0;
		}
	}
}
