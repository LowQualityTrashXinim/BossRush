using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;

class DrainingVeil : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulDrain)
			.AddIngredient(ItemID.ClingerStaff)
			.Register();
	}
}
