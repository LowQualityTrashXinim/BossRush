using BossRush.Common.Utils;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	internal class DukeTreasureChest : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostPlant);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostPlant);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostPlant);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostPlant);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostGolem);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostGolem);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostGolem);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostGolem);
			LootboxSystem.AddItemPool(itempool);
		}
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount, RNGManage(player, 25, 25, 25, 25, 0));
			for (int i = 0; i < 3; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
		}
	}
}
