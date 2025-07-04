﻿using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class BlackLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Purple;
		}
		public override bool CanActivateSpoil => false;
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedEmpressOfLight) {
				int ran1 = Main.rand.Next(10);
				switch (ran1) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.EmpressBlade);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.FairyQueenBossBag);
						break;
				}
			}
			if (NPC.downedFishron) {
				int ran1 = Main.rand.Next(10);
				switch (ran1) {
					case 0:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<DukeLootBox>());
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.FishronBossBag);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.FishronWings);
						break;
				}
			}
			if (NPC.downedMoonlord) {
				int ran1 = Main.rand.Next(10);
				switch (ran1) {
					case 0:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<MoonLootBox>());
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.MoonLordBossBag);
						break;
				}
			}
			if (Main.rand.NextBool(25) && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) || Main.rand.NextBool(100)) {
				player.QuickSpawnItem(entitySource, ModContent.ItemType<UltimatePistol>());
			}
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

		}
	}
}
