using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class Item9 : SynergyModItem {
	//Aquatic Tornado
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 12, 2f, 5, 5, ItemUseStyleID.Shoot, ProjectileID.Bubble, 10, true);
	}
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SnowmanCannon)
			.AddIngredient(ItemID.AquaScepter)
			.Register();
	}
}
