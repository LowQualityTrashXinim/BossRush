using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.BuilderItem;
using BossRush.Common.General;

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
			player.QuickSpawnItem(entitySource, ItemID.Safe);
			player.QuickSpawnItem(entitySource, ItemID.MoneyTrough);
			player.QuickSpawnItem(entitySource, ItemID.DemonConch);
			player.QuickSpawnItem(entitySource, ItemID.MagicMirror);
		}
	}
}
