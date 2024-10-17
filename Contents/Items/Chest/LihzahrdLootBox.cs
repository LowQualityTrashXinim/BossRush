using BossRush.Common;
using BossRush.Common.Systems;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Consumable.Potion;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	class LihzahrdLootBox : LootBoxBase {
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
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount, RNGManage(player, 25, 25, 25, 25, 0));
			for (int i = 0; i < 3; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			GetArmorForPlayer(entitySource, player);
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.BeeWings, ItemID.BeetleWings, ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
			player.QuickSpawnItem(entitySource, wing);
			player.QuickSpawnItem(entitySource, ItemID.GoldenFishingRod);
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.SpecialPotion));
			}
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
		}
	}
}
