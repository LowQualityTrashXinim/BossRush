using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.SoulBound;
internal class SharpshooterGuidance : ModSoulBound {
	public override string ModifiedToolTip(Item item) {
		int level = GetLevel(item);
		return Description.FormatWith(new string[] {
			Math.Round((.35f + .05f * level) * 100).ToString(),
			Math.Round((.7f + .01f * level) * 100).ToString(),
		});
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.CritChance, Base: 2 + 2 * GetLevel(item));
		handle.AddStatsToPlayer(PlayerStats.PureDamage, 1 - (.1f + .02f * GetLevel(item)));
	}
}
internal class SharpshooterGuidanceItem : BaseSoulBoundItem {
	public override short SoulBoundType => ModSoulBound.GetSoulBoundType<SharpshooterGuidance>();
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulofSight, 16)
			.AddIngredient(ItemID.Lens, 30)
			.AddIngredient(ItemID.BlackLens, 5)
			.AddTile(TileID.LunarCraftingStation)
			.Register();
	}
}
