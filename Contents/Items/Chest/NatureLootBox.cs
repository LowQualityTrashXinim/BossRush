using BossRush.Common.Global;
using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class NatureLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Cyan;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostAllMechs);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostAllMech);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostAllMech);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostAllMech);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostPlant);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostPlant);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostPlant);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostPlant);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 8, 9, 10 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			int RandomNumber = Main.rand.Next(5); int Random2 = Main.rand.Next(3);
			switch (RandomNumber) {
				case 0:
					player.QuickSpawnItem(entitySource, ItemID.TurtleHelmet);
					player.QuickSpawnItem(entitySource, ItemID.TurtleScaleMail);
					player.QuickSpawnItem(entitySource, ItemID.TurtleLeggings);
					break;
				case 1:
					player.QuickSpawnItem(entitySource, ItemID.SpookyHelmet);
					player.QuickSpawnItem(entitySource, ItemID.SpookyBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.SpookyLeggings);
					break;
				case 2:
					switch (Random2) {
						case 0:
							player.QuickSpawnItem(entitySource, ItemID.ShroomiteHeadgear);
							break;
						case 1:
							player.QuickSpawnItem(entitySource, ItemID.ShroomiteHelmet);
							break;
						case 2:
							player.QuickSpawnItem(entitySource, ItemID.ShroomiteMask);
							break;
					}
					player.QuickSpawnItem(entitySource, ItemID.ShroomiteBreastplate);
					player.QuickSpawnItem(entitySource, ItemID.ShroomiteLeggings);
					break;
				case 3:
					player.QuickSpawnItem(entitySource, ItemID.SpectreHood);
					player.QuickSpawnItem(entitySource, ItemID.SpectreRobe);
					player.QuickSpawnItem(entitySource, ItemID.SpectrePants);
					break;
				case 4:
					player.QuickSpawnItem(entitySource, ItemID.SpectreMask);
					player.QuickSpawnItem(entitySource, ItemID.SpectreRobe);
					player.QuickSpawnItem(entitySource, ItemID.SpectrePants);
					break;
			}
			modplayer.GetAmount();
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
			int wing = Main.rand.Next(new int[] { ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
			player.QuickSpawnItem(entitySource, wing);
			player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
			player.QuickSpawnItem(entitySource, ItemID.NaughtyPresent);
			player.QuickSpawnItem(entitySource, ItemID.PumpkinMoonMedallion);
			player.QuickSpawnItem(entitySource, ItemID.LihzahrdAltar);

		}
	}
}
