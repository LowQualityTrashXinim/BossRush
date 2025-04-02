using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.SoulBound {
	class DryadSoul : ModSoulBound {
		public override string ModifiedToolTip(Item item) {
			int level = GetLevel(item);
			return Description.FormatWith(new string[] {
			Math.Round((.1f + .05f * level) * 100).ToString(),
			(2 + 2 * GetLevel(item)).ToString(),
			(-100 * GetLevel(item)).ToString(),
		});
		}
		public override void UpdateEquip(Player player, Item item) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1 + .1f * GetLevel(item), Flat: 2 + 2 * GetLevel(item));
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 2 + 2 * GetLevel(item));
			modplayer.AddStatsToPlayer(PlayerStats.EnergyCap, Base: -100 * GetLevel(item));
		}
	}
	class DryadSoulItem : BaseSoulBoundItem {
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Daybloom, 10)
				.AddIngredient(ItemID.Moonglow, 10)
				.AddIngredient(ItemID.Blinkroot, 10)
				.AddIngredient(ItemID.Waterleaf, 10)
				.AddIngredient(ItemID.Deathweed, 10)
				.AddIngredient(ItemID.Fireblossom, 10)
				.AddIngredient(ItemID.Shiverthorn, 10)
				.AddIngredient(ItemID.SoulofLight, 99)
				.AddTile(TileID.LunarCraftingStation)
			.Register();
		}
	}
}
