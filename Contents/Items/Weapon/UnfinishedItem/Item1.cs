using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;

class TwilightNight : SynergyModItem {
	//Shoot out blade and magic missile near the player that slightly aim toward your cursor that explode when time is met or on hit, the sword will sometime appear to fall down from the sky dealing 2 time the damage and the magic missile will sometime spawn out from sword when hit a enemy
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicMissile)
			.AddIngredient(ItemID.SkyFracture)
			.Register();
	}
}
