﻿using BossRush.Common.Global;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Consumable.Spawner;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class MechLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Lime;
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
		public override void ModifyLootAdd(Player player) {
			LootBoxItemPool itempool = LootboxSystem.GetItemPool(Type);
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostAllMechs);
				itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostAllMech);
				itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostAllMech);
				itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostAllMech);
			}
		}
		public override List<int> FlagNumAcc() => new List<int> { 8, 9, 10 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				int randChooser = Main.rand.Next(4);
				switch (randChooser) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteHelmet);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteHeadgear);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteMask);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
						player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
						break;
					default:
						break;
				}
			}
			GetWeapon(entitySource, player, modplayer.weaponAmount, RNGManage(player, 25, 25, 25, 25, 0));
			for (int i = 0; i < 2; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.ButterflyWings, ItemID.FlameWings, ItemID.FrozenWings, ItemID.SteampunkWings, ItemID.Jetpack });
			player.QuickSpawnItem(entitySource, wing);
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				player.QuickSpawnItem(entitySource, ItemID.ChlorophytePickaxe);
			}
			player.QuickSpawnItem(entitySource, ModContent.ItemType<PlanteraEssence>());
			player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
		}
	}
}
