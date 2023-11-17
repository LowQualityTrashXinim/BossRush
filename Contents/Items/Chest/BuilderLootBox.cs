﻿using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using BossRush.Contents.Items.BuilderItem;

namespace BossRush.Contents.Items.Chest {
	internal class BuilderLootBox : ModItem {
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Gray;
		}
		public override bool CanRightClick() => true;

		public override void RightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ModContent.ItemType<ArenaMaker>());
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SuperBuilderTool>());
			if (!ModContent.GetInstance<BossRushModConfig>().NoMoreChestFromBuilderLootbox) {
				player.QuickSpawnItem(entitySource, ItemID.SteampunkChest, 6);
				player.QuickSpawnItem(entitySource, ItemID.StardustChest);
				player.QuickSpawnItem(entitySource, ItemID.VortexChest);
				player.QuickSpawnItem(entitySource, ItemID.SolarChest);
				player.QuickSpawnItem(entitySource, ItemID.NebulaChest);
			}
			player.QuickSpawnItem(entitySource, ItemID.MagicConch);
			player.QuickSpawnItem(entitySource, ItemID.MagicMirror);
			player.QuickSpawnItem(entitySource, ItemID.DemonConch);
		}
	}
}
