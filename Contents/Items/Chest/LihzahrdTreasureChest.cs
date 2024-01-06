using BossRush.Common;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Potion;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class LihzahrdTreasureChest : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Red;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostPlant);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostPlant);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostPlant);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostPlant);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePostGolem);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePostGolem);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPostGolem);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPostGolem);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			for (int i = 0; i < 2; i++) {
				int Accessory = Main.rand.Next(new int[] { ItemID.MasterNinjaGear, ItemID.FireGauntlet, ItemID.NecromanticScroll, ItemID.CelestialEmblem, ItemID.CelestialShell, ItemID.AvengerEmblem, ItemID.CharmofMyths, ItemID.DestroyerEmblem, ItemID.SniperScope, ItemID.StarCloak, ItemID.StarVeil, ItemID.CelestialCuffs });
				player.QuickSpawnItem(entitySource, Accessory);
			}
			int wing = Main.rand.Next(new int[] { ItemID.BeeWings, ItemID.BeetleWings, ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
			player.QuickSpawnItem(entitySource, wing);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount, RNGManage(player, 25, 25, 25, 25, 0));
			for (int i = 0; i < 3; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			player.QuickSpawnItem(entitySource, ItemID.GoldenFishingRod);
			if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode && player.difficulty == PlayerDifficultyID.Hardcore) {
				int RandomModdedBuff = Main.rand.Next(new int[] {
					ModContent.ItemType<BerserkerElixir>(),
					ModContent.ItemType<GunslingerElixir>(),
					ModContent.ItemType<SageElixir>(),
					ModContent.ItemType<CommanderElixir>(),
					ModContent.ItemType<TitanElixir>() });
				player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
			}
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
		}
	}
}
