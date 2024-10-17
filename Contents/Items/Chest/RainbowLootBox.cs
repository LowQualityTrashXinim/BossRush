using BossRush.Contents.Items.Accessories;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	internal class RainbowLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Purple;
		}
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
						player.QuickSpawnItem(entitySource, ModContent.ItemType<NatureTreasureChest>());
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
