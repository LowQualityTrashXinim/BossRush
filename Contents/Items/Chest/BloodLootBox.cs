using BossRush.Common.Global;
using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	internal class BloodLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeHM);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeHM);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicHM);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonHM);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeMech);
			itempool.DropItemRange.Add(ItemID.SuperStarCannon);
			itempool.DropItemRange.Add(ItemID.DD2PhoenixBow);
			itempool.DropItemMagic.Add(ItemID.UnholyTrident);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
		}
	}
}
