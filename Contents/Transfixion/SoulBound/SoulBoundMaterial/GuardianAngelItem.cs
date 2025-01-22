using BossRush.Contents.Transfixion.Arguments;
using Terraria.ID;

namespace BossRush.Contents.Transfixion.SoulBound.SoulBoundMaterial;
internal class GuardianAngelItem : BaseSoulBoundItem {
	public override short SoulBoundType => ModSoulBound.GetSoulBoundType<GuardianAngel>();
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulofLight, 15)
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddIngredient(ItemID.SilverBar, 40)
			.AddIngredient(ItemID.LightShard, 3)
			.AddIngredient(ItemID.PaladinsShield)
			.AddTile(TileID.LunarCraftingStation)
			.Register();
	}
}
