﻿using BossRush.Common.Global;
using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class SilverLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Green;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonerPreEoC);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);

			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMagic.Add(ItemID.ZapinatorGray);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			else {
				int RandomNumber = Main.rand.Next(6);
				switch (RandomNumber) {
					case 0:
						int RandomAssArmor = Main.rand.Next(new int[] { ItemID.MagicHat, ItemID.WizardHat });
						if (RandomAssArmor == ItemID.WizardHat || RandomAssArmor == ItemID.MagicHat) {
							player.QuickSpawnItem(entitySource, RandomAssArmor);
							int RobeWiz = Main.rand.Next(new int[] { ItemID.AmethystRobe, ItemID.DiamondRobe, ItemID.RubyRobe, ItemID.SapphireRobe, ItemID.EmeraldRobe, ItemID.TopazRobe, ItemID.GypsyRobe });
							player.QuickSpawnItem(entitySource, RobeWiz);
						}
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.SilverHelmet);
						player.QuickSpawnItem(entitySource, ItemID.SilverChainmail);
						player.QuickSpawnItem(entitySource, ItemID.SilverGreaves);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.TungstenHelmet);
						player.QuickSpawnItem(entitySource, ItemID.TungstenChainmail);
						player.QuickSpawnItem(entitySource, ItemID.TungstenGreaves);
						break;
					case 3:
						player.QuickSpawnItem(entitySource, ItemID.GoldHelmet);
						player.QuickSpawnItem(entitySource, ItemID.GoldChainmail);
						player.QuickSpawnItem(entitySource, ItemID.GoldGreaves);
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ItemID.PlatinumHelmet);
						player.QuickSpawnItem(entitySource, ItemID.PlatinumChainmail);
						player.QuickSpawnItem(entitySource, ItemID.PlatinumGreaves);
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ItemID.PumpkinHelmet);
						player.QuickSpawnItem(entitySource, ItemID.PumpkinBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.PumpkinLeggings);
						break;
				}
			}
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);

			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, Main.rand.Next(new int[] { ItemID.AmethystHook, ItemID.TopazHook, ItemID.SapphireHook, ItemID.EmeraldHook }));
		}
	}
}
