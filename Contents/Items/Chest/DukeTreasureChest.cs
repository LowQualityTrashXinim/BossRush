using BossRush.Common.Utils;
using Terraria;

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
			Item.rare = 10;
		}
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			for (int i = 0; i < modplayer.weaponAmount; i++) {
				GetWeapon(player, out int weapon, out int specialAmount, RNGManage(player, 25, 25, 25, 25, 0));
				AmmoForWeapon(out int ammo, out int num, weapon, 3.5f);
				player.QuickSpawnItem(entitySource, weapon, specialAmount);
				player.QuickSpawnItem(entitySource, ammo, num);
			}
			for (int i = 0; i < 3; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
		}
	}
}
