using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class DeerclopTreasureChest : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 42;
			Item.height = 36;
			Item.rare = ItemRarityID.Pink;
		}
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ItemID.SnowGlobe);
			for (int i = 0; i < 4; i++) {
				if (Main.rand.NextBool(3)) {
					int acceAdd = Main.rand.Next(new int[] { ItemID.ArcaneFlower, ItemID.CelestialCuffs, ItemID.CelestialEmblem, ItemID.MagnetFlower, ItemID.ManaCloak, ItemID.BerserkerGlove, ItemID.FireGauntlet, ItemID.CelestialShell, ItemID.FrozenShield, ItemID.HeroShield, ItemID.MoltenQuiver, ItemID.StalkersQuiver, ItemID.ReconScope, ItemID.MoltenSkullRose, ItemID.AmphibianBoots, ItemID.FrogGear, ItemID.BundleofBalloons, ItemID.TerrasparkBoots });
					player.QuickSpawnItem(entitySource, acceAdd);
				}
			}
			for (int i = 0; i < 4; i++) {
				if (Main.rand.NextBool(3)) {
					int rand = Main.rand.Next(new int[] { ItemID.NorthPole, ItemID.Amarok, ItemID.Frostbrand, ItemID.SnowmanCannon, ItemID.CoolWhip, ItemID.IceBlade, ItemID.SnowballCannon, ItemID.IceBoomerang, ItemID.FrostDaggerfish, ItemID.FrostStaff, ItemID.FlowerofFrost, ItemID.IceBow, ItemID.BlizzardStaff, ItemID.IceSickle, ItemID.IceRod, ItemID.StaffoftheFrostHydra });
					player.QuickSpawnItem(entitySource, rand);
					AmmoForWeapon(out int ammo, out int amount, rand, 2.75f);
					player.QuickSpawnItem(entitySource, rand);
					player.QuickSpawnItem(entitySource, ammo, amount);
				}
			}
		}
	}
}
