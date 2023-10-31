using Terraria;
using Terraria.ID;
using BossRush.Common.Utils;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Chest {
	internal class IceLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = 5;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);

			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMagic.Add(ItemID.ZapinatorGray);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeEvilBoss);
			itempool.DropItemRange.Add(ItemID.MoltenFury);
			itempool.DropItemRange.Add(ItemID.StarCannon);
			itempool.DropItemRange.Add(ItemID.AleThrowingGlove);
			itempool.DropItemRange.Add(ItemID.Harpoon);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicEvilBoss);
			itempool.DropItemSummon.Add(ItemID.ImpStaff);

			itempool.DropItemRange.Add(ItemID.PewMaticHorn);
			itempool.DropItemMagic.Add(ItemID.WeatherPain);
			itempool.DropItemSummon.Add(ItemID.HoundiusShootius);

			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() {
			List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5 };
			if (NPC.downedQueenBee) {
				list.Add(6);
			}
			if (NPC.downedBoss3) {
				list.Add(7);
			}
			return list;
		}
		public override void ModifyLootAdd(Player player) {
			LootBoxItemPool itempool = LootboxSystem.GetItemPool(Type);
			if (NPC.downedBoss3) {
				itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
				itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
				itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
				itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);
			}
			if(NPC.downedQueenBee) {
				itempool.DropItemMelee.Add(ItemID.BeeKeeper);
				itempool.DropItemRange.Add(ItemID.BeesKnees);
				itempool.DropItemRange.Add(ItemID.Blowgun);
				itempool.DropItemMagic.Add(ItemID.BeeGun);
				itempool.DropItemSummon.Add(ItemID.HornetStaff);
				itempool.DropItemMisc.Add(ItemID.Beenade);
			}
		}
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedQueenBee) {
				int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace, ItemID.WaspGun });
				player.QuickSpawnItem(entitySource, OneRareBeeItem);
			}
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < 5; i++) {
				GetWeapon(player, out int weapon, out int specialAmount);
				AmmoForWeapon(out int ammo, out int num, weapon);
				player.QuickSpawnItem(entitySource, weapon, specialAmount);
				player.QuickSpawnItem(entitySource, ammo, num);
			}
			for (int i = 0; i < 5; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), 3);
			}
		}
	}
}
