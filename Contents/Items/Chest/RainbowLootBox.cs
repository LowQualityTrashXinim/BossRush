using BossRush.Common.Utils;
using BossRush.Contents.Items.Accessories;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	internal class RainbowLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.CommonAxe);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreEoC);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreEoC);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreEoC);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonerPreEoC);

			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMelee.Add(ItemID.BloodLustCluster);
			itempool.DropItemMelee.Add(ItemID.WarAxeoftheNight);
			itempool.DropItemMagic.Add(ItemID.ZapinatorGray);

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

			itempool.DropItemRange.Add(ItemID.PewMaticHorn);
			itempool.DropItemMagic.Add(ItemID.WeatherPain);
			itempool.DropItemSummon.Add(ItemID.HoundiusShootius);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeSkel);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeSkele);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicSkele);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonSkele);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeHM);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangeHM);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicHM);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonHM);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleeMech);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostAllMechs);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostAllMech);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostAllMech);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostAllMech);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostPlant);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostPlant);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostPlant);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostPlant);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostGolem);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostGolem);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostGolem);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostGolem);

			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreLuna);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreLuna);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreLuna);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreLuna);
			LootboxSystem.AddItemPool(itempool);
		}
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Purple;
		}
		public override bool CanActivateSpoil => false;
		public override bool ChestUseOwnLogic => true;
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int chestRanAmount = Main.rand.Next(2, 9);
			for (int i = 0; i < chestRanAmount; i++) {
				switch (Main.rand.Next(11)) {
					case 0:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<WoodenLootBox>());
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<IronLootBox>());
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverLootBox>());
						break;
					case 3:
						if (WorldGen.crimson) {
							player.QuickSpawnItem(entitySource, ModContent.ItemType<CrimsonLootBox>());
						}
						else {
							player.QuickSpawnItem(entitySource, ModContent.ItemType<CorruptionLootBox>());
						}
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldLootBox>());
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<HoneyLootBox>());
						break;
					case 6:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<ShadowLootBox>());
						break;
					case 7:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<CrystalLootBox>());
						break;
					case 8:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<MechLootBox>());
						break;
					case 9:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<NatureLootBox>());
						break;
					case 10:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<LihzahrdLootBox>());
						break;
				}
				if (Main.rand.NextBool(10)) {
					chestRanAmount++;
				}
			}

			int randomAmount = Main.rand.Next(10, 25);
			GetWeapon(entitySource, player, randomAmount);
			int randomRareStuff = Main.rand.Next(10);
			switch (randomRareStuff) {
				case 0:
					player.QuickSpawnItem(entitySource, ItemID.Zenith);
					break;
				case 1:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<UltimatePistol>());
					break;
				case 2:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<EmblemofProgress>());
					break;
			}
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SynergyEnergy>());
			player.QuickSpawnItem(entitySource, ItemID.SlimeCrown);
			player.QuickSpawnItem(entitySource, ItemID.HallowedChest, 99);
		}
	}
}
