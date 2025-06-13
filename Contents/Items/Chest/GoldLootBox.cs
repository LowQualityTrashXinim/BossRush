using BossRush.Common.Global;
using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class GoldLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);

			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMagic.Add(ItemID.ZapinatorGray);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.SummonerPreEoC);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeEvilBoss);
			itempool.DropItemRange.Add(ItemID.MoltenFury);
			itempool.DropItemRange.Add(ItemID.StarCannon);
			itempool.DropItemRange.Add(ItemID.AleThrowingGlove);
			itempool.DropItemRange.Add(ItemID.Harpoon);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicEvilBoss);
			itempool.DropItemSummon.Add(ItemID.ImpStaff);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);
			LootboxSystem.AddItemPool(itempool);
		}
		public override void ModifyLootAdd(Player player) {
			LootBoxItemPool itempool = LootboxSystem.GetItemPool(Type);
			if (NPC.downedQueenBee) {
				itempool.DropItemMelee.Add(ItemID.BeeKeeper);
				itempool.DropItemRange.Add(ItemID.BeesKnees);
				itempool.DropItemRange.Add(ItemID.Blowgun);
				itempool.DropItemMagic.Add(ItemID.BeeGun);
				itempool.DropItemSummon.Add(ItemID.HornetStaff);
				itempool.DropItemMisc.Add(ItemID.Beenade);
			}
		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Pink;
		}
		public override List<int> FlagNumAcc() {
			List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 7 };
			if (NPC.downedQueenBee) {
				list.Add(6);
			}
			return list;
		}

		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			else {
				int RandomNumber = Main.rand.Next(3);
				switch (RandomNumber) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.NecroHelmet);
						player.QuickSpawnItem(entitySource, ItemID.NecroBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.NecroGreaves);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
						player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
						player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
						player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
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
			player.QuickSpawnItem(entitySource, ItemID.CalmingPotion, 10);
		}
	}
}
