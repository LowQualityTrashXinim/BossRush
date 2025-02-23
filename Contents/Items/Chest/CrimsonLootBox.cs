using Terraria;
using Terraria.ID;
using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Chest {
	class CrimsonLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Orange;
		}
		public override void LootPoolSetStaticDefaults() {
			LootBoxItemPool itempool = new LootBoxItemPool(Type);
			itempool.DropItemMelee.UnionWith(TerrariaArrayID.MeleePreBoss);
			itempool.DropItemRange.UnionWith(TerrariaArrayID.RangePreBoss);
			itempool.DropItemMagic.UnionWith(TerrariaArrayID.MagicPreBoss);
			itempool.DropItemSummon.UnionWith(TerrariaArrayID.SummonPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);

			itempool.DropItemMelee.Add(ItemID.Code1);
			itempool.DropItemMelee.Add(ItemID.BloodLustCluster);
			itempool.DropItemMelee.Add(ItemID.WarAxeoftheNight);
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

			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2, 3, 4, 5 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			else {
				switch (Main.rand.Next(5)) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.CrimsonHelmet);
						player.QuickSpawnItem(entitySource, ItemID.CrimsonScalemail);
						player.QuickSpawnItem(entitySource, ItemID.CrimsonGreaves);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
						player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
						player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.FossilHelm);
						player.QuickSpawnItem(entitySource, ItemID.FossilShirt);
						player.QuickSpawnItem(entitySource, ItemID.FossilPants);
						break;
					case 3:
						player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
						player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ItemID.ObsidianHelm);
						player.QuickSpawnItem(entitySource, ItemID.ObsidianShirt);
						player.QuickSpawnItem(entitySource, ItemID.ObsidianPants);
						break;
				}
			}
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedBoss2) {
				player.QuickSpawnItem(entitySource, ItemID.TinkerersWorkshop);
				player.QuickSpawnItem(entitySource, ItemID.Hellforge);
				player.QuickSpawnItem(entitySource, Main.rand.Next(new int[] { ItemID.DiamondHook, ItemID.RubyHook }));
			}
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystalStand);
		}
	}
}
