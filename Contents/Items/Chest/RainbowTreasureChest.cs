using BossRush.Contents.Items.Accessories;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	internal class RainbowTreasureChest : LootBoxBase {
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Rainbow Treasure chest");
			// Tooltip.SetDefault("Blessed");
		}
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = 11;
		}
		public override bool ChestUseOwnLogic => true;
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
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
						player.QuickSpawnItem(entitySource, ModContent.ItemType<HoneyTreasureChest>());
						break;
					case 6:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<ShadowTreasureChest>());
						break;
					case 7:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<CrystalTreasureChest>());
						break;
					case 8:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<MechTreasureChest>());
						break;
					case 9:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<NatureTreasureChest>());
						break;
					case 10:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<LihzahrdTreasureChest>());
						break;
				}
				if (Main.rand.NextBool(10)) {
					chestRanAmount++;
				}
			}

			int randomAmount = Main.rand.Next(10, 25);
			for (int i = 0; i < randomAmount; i++) {
				GetWeapon(player, out int Weapon, out int specialAmount);
				AmmoForWeapon(out int Ammo, out int Amount, Weapon, 10);
				player.QuickSpawnItem(entitySource, Weapon, specialAmount);
				player.QuickSpawnItem(entitySource, Ammo, Amount);
				if (Main.rand.NextBool(10)) {
					randomAmount++;
				}
			}
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