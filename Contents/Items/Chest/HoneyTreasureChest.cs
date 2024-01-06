using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class HoneyTreasureChest : LootBoxBase {
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

			itempool.DropItemMelee.Add(ItemID.BeeKeeper);
			itempool.DropItemRange.Add(ItemID.BeesKnees);
			itempool.DropItemRange.Add(ItemID.Blowgun);
			itempool.DropItemMagic.Add(ItemID.BeeGun);
			itempool.DropItemSummon.Add(ItemID.HornetStaff);
			itempool.DropItemMisc.Add(ItemID.Beenade);

			LootboxSystem.AddItemPool(itempool);
		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void ModifyLootAdd(Player player) {
			LootBoxItemPool itempool = LootboxSystem.GetItemPool(Type);
			if (NPC.downedBoss3) {
				itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
				itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
				itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
				itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);
			}
		}
		//public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 4, 5 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			for (int i = 0; i < 3; i++) {
				switch (Main.rand.Next(30)) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.BeeCloak);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.HoneyedGoggles);
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ItemID.QueenBeeBossBag);
						break;
					case 10:
						player.QuickSpawnItem(entitySource, ItemID.HoneyBalloon);
						break;
					case 15:
						player.QuickSpawnItem(entitySource, ItemID.SweetheartNecklace);
						break;
				}
			}
			GetWeapon(entitySource, player, 5);
			player.QuickSpawnItem(entitySource, ItemID.Honeyfin, 10);
			player.QuickSpawnItem(entitySource, GetPotion(), 3);
		}
	}
}
