using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class CrystalTreasureChest : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeHM);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeHM);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicHM);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonHM);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeQS);
			itempool.DropItemSummon.Add(ItemID.Smolstar);
			LootboxSystem.AddItemPool(itempool);
		}
		public override void ModifyLootAdd(Player player) {
			LootBoxItemPool itempool = LootboxSystem.GetItemPool(Type);
			if (NPC.downedMechBossAny) {
				itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeMech);
				itempool.DropItemRange.Add(ItemID.SuperStarCannon);
				itempool.DropItemRange.Add(ItemID.DD2PhoenixBow);
				itempool.DropItemMagic.Add(ItemID.UnholyTrident);
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostAllMechs);
				itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostAllMech);
				itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostAllMech);
				itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostAllMech);
			}
		}
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
			player.QuickSpawnItem(entitySource, wing);
			switch (Main.rand.Next(3)) {
				case 0:
					player.QuickSpawnItem(entitySource, ItemID.FrostHelmet);
					player.QuickSpawnItem(entitySource, ItemID.FrostBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.FrostLeggings);
					break;
				case 1:
					player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorHat);
					player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorShirt);
					player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorPants);
					break;
				case 2:
					player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaHelmet);
					player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaChestplate);
					player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaLeggings);
					break;
			}
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < 2; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			if (Main.rand.NextBool(5)) {
				player.QuickSpawnItem(entitySource, ItemID.QueenSlimeBossBag);
			}
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
			}
		}
	}
}
