using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class IronLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Blue;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonerPreEoC);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);
			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMagic.Add(ItemID.ZapinatorGray);

			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			else {
				int RandomNumber = Main.rand.Next(6);
				switch (RandomNumber) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.CopperHelmet);
						player.QuickSpawnItem(entitySource, ItemID.CopperChainmail);
						player.QuickSpawnItem(entitySource, ItemID.CopperGreaves);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.TinHelmet);
						player.QuickSpawnItem(entitySource, ItemID.TinChainmail);
						player.QuickSpawnItem(entitySource, ItemID.TinGreaves);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.IronHelmet);
						player.QuickSpawnItem(entitySource, ItemID.IronChainmail);
						player.QuickSpawnItem(entitySource, ItemID.IronGreaves);
						break;
					case 3:
						player.QuickSpawnItem(entitySource, ItemID.LeadHelmet);
						player.QuickSpawnItem(entitySource, ItemID.LeadChainmail);
						player.QuickSpawnItem(entitySource, ItemID.LeadGreaves);
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ItemID.NinjaHood);
						player.QuickSpawnItem(entitySource, ItemID.NinjaPants);
						player.QuickSpawnItem(entitySource, ItemID.NinjaShirt);
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ItemID.JungleHat);
						player.QuickSpawnItem(entitySource, ItemID.JungleShirt);
						player.QuickSpawnItem(entitySource, ItemID.JunglePants);
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
			player.QuickSpawnItem(entitySource, ItemID.IronAnvil);
		}
	}
}
