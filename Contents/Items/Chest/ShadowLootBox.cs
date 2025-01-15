using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using System.Collections.Generic;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Chest {
	class ShadowLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeHM);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeHM);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicHM);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonHM);
			LootboxSystem.AddItemPool(itempool);
		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			int RandomNumber = Main.rand.Next(7);
			int Random2 = Main.rand.Next(3);
			switch (RandomNumber) {
				case 0:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.CobaltHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.CobaltHat);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.CobaltMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.CobaltBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.CobaltLeggings);
					break;
				case 1:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.MythrilHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.MythrilHat);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.MythrilHood);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.MythrilChainmail);
					player.QuickSpawnItem(entitySource, ItemID.MythrilGreaves);
					break;
				case 2:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.AdamantiteHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.AdamantiteHeadgear);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.AdamantiteMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.AdamantiteBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.AdamantiteLeggings);
					break;
				case 3:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.PalladiumHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.PalladiumHeadgear);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.PalladiumMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.PalladiumBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.PalladiumLeggings);
					break;
				case 4:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.OrichalcumHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.OrichalcumHeadgear);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.OrichalcumMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.OrichalcumBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.OrichalcumLeggings);
					break;
				case 5:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.TitaniumHelmet);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.TitaniumHeadgear);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.TitaniumMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.TitaniumBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.TitaniumLeggings);
					break;
				case 6:
					player.QuickSpawnItem(entitySource, ItemID.SpiderMask);
					player.QuickSpawnItem(entitySource, ItemID.SpiderBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.SpiderGreaves);
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
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
			player.QuickSpawnItem(entitySource, wing);
			player.QuickSpawnItem(entitySource, ItemID.MythrilAnvil);
			player.QuickSpawnItem(entitySource, ItemID.AdamantiteForge);
			if (Main.rand.NextBool()) {
				player.QuickSpawnItem(entitySource, ItemID.AdamantitePickaxe);
			}
			else {
				player.QuickSpawnItem(entitySource, ItemID.TitaniumPickaxe);
			}
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
			}
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.HARDCORE_MODE)) {
				int RandomModdedBuff = Main.rand.Next(TerrariaArrayID.SpecialPotion);
				player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
			}
			if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX)) {
				GetArmorForPlayer(entitySource, player);
				GetWeapon(entitySource, player, 2);
				GetAccessories(Type, player);
				GetPotion(Type, player);
				GetSkillLootbox(Type, player, 1);
			}
		}
	}
}
