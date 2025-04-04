using BossRush.Common.Utils;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	class DeerclopTreasureChest : LootBoxBase {
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

			itempool.DropItemRange.Add(ItemID.PewMaticHorn);
			itempool.DropItemMagic.Add(ItemID.WeatherPain);
			itempool.DropItemSummon.Add(ItemID.HoundiusShootius);

			LootboxSystem.AddItemPool(itempool);
		}
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
					player.QuickSpawnItem(entitySource, rand);
					AmmoForWeapon(entitySource, player, rand, 2.75f);
				}
			}
		}
	}
}
