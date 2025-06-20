using Terraria;
using Terraria.ID;
using BossRush.Common.Utils;
using BossRush.Common.Systems;
using System.Collections.Generic;
using Terraria.ModLoader;
using BossRush.Common.General;
using BossRush.Common.Mode.DreamLikeWorldMode;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Chest {
	class WoodenLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.White;
		}
		public override bool CanActivateSpoil => !ModContent.GetInstance<RogueLikeConfig>().RoguelikeMode;
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
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.SpecialPreBoss);
			itempool.DropItemMisc.UnionWith(TerrariaArrayID.Special);
			LootboxSystem.AddItemPool(itempool);
		}
		public override List<int> FlagNumAcc() => new List<int> { 2 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(true), modplayer.potionNumAmount);
			}
			if (player.IsDebugPlayer()) {
				GetArmorForPlayer(entitySource, player);
			}
			else {
				int RandomNumber = Main.rand.Next(9);
				switch (RandomNumber) {
					case 0:
						player.QuickSpawnItem(entitySource, ItemID.WoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.WoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.WoodGreaves);
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ItemID.BorealWoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.BorealWoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.BorealWoodGreaves);
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ItemID.RichMahoganyHelmet);
						player.QuickSpawnItem(entitySource, ItemID.RichMahoganyBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.RichMahoganyGreaves);
						break;
					case 3:
						player.QuickSpawnItem(entitySource, ItemID.EbonwoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.EbonwoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.EbonwoodGreaves);
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ItemID.PalmWoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.PalmWoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.PalmWoodGreaves);
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ItemID.ShadewoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.ShadewoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.ShadewoodGreaves);
						break;
					case 6:
						player.QuickSpawnItem(entitySource, ItemID.CactusHelmet);
						player.QuickSpawnItem(entitySource, ItemID.CactusBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.CactusLeggings);
						break;
					case 7:
						player.QuickSpawnItem(entitySource, ItemID.AshWoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.AshWoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.AshWoodGreaves);
						break;
					case 8:
						player.QuickSpawnItem(entitySource, ItemID.PearlwoodHelmet);
						player.QuickSpawnItem(entitySource, ItemID.PearlwoodBreastplate);
						player.QuickSpawnItem(entitySource, ItemID.PearlwoodGreaves);
						break;
				}
				int RandomAssArmor = Main.rand.Next(new int[] { ItemID.FlinxFurCoat, ItemID.VikingHelmet, ItemID.EmptyBucket, ItemID.NightVisionHelmet, ItemID.DivingHelmet, ItemID.Goggles, ItemID.Gi });
				player.QuickSpawnItem(entitySource, RandomAssArmor);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX)) {
				if (ChaosModeSystem.Chaos()) {
					GetArmorPiece(Type, player);
					for (int i = 0; i < 3; i++) {
						GetAccessories(Type, player, true);
					}
					GetPotion(Type, player);
				}
				else {
					GetWeapon(entitySource, player, 2);
					GetArmorForPlayer(entitySource, player);
					GetAccessories(Type, player);
					GetPotion(Type, player);
					player.QuickSpawnItem(entitySource, ModContent.ItemType<SpecialSkillLootBox>());
				}
			}
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.HARDCORE_MODE)) {
				int RandomModdedBuff = Main.rand.Next(TerrariaArrayID.SpecialPotion);
				player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
			}
			player.QuickSpawnItem(entitySource, ItemID.GrapplingHook);
			player.QuickSpawnItem(entitySource, ItemID.LesserHealingPotion, 5);
		}
	}
}
