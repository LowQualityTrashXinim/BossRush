using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class GoldLootBox : LootBoxBase {
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

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);
			LootboxSystem.AddItemPool(itempool);
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

		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedQueenBee) {
				int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace, ItemID.WaspGun });
				player.QuickSpawnItem(entitySource, OneRareBeeItem);
			}
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
			for (int i = 0; i < modplayer.weaponAmount; i++) {
				GetWeapon(player, out int weapon, out int specialAmount);
				AmmoForWeapon(out int ammo, out int num, weapon);
				player.QuickSpawnItem(entitySource, weapon, specialAmount);
				player.QuickSpawnItem(entitySource, ammo, num);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			player.QuickSpawnItem(entitySource, ItemID.CalmingPotion, 10);
		}
	}
}
