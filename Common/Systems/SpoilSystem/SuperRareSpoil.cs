using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Terraria.DataStructures;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Contents.Items.aDebugItem.UIdebug;

namespace BossRush.Common.Systems.SpoilSystem;
internal class SuperRareSpoil {
	public class SuppliesPackage : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override string FinalDescription() {
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			chestplayer.GetAmount();
			return Description.FormatWith(
				Math.Ceiling(chestplayer.weaponAmount * .5f),
				chestplayer.ModifyGetAmount(1)
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
			LootBoxBase.GetAccessories(itemsource, player, true);
			LootBoxBase.GetArmorPiece(itemsource, player, true);
			LootBoxBase.GetSkillLootbox(itemsource, player);
			LootBoxBase.GetRelic(itemsource, player);
		}
	}
	public class PerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int type = ModContent.ItemType<WorldEssence>();
			if (Main.rand.NextFloat() <= .01f) {
				type = ModContent.ItemType<PerkDebugItem>();
			}
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), type);
		}
	}
	public class SuperRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			Item item = player.QuickSpawnItemDirect(player.GetSource_OpenItem(itemsource), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				relic.AutoAddRelicTemplate(player, 4);
			}
		}
	}
	public class TrinketSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			player.QuickSpawnItem(entitySource, Main.rand.Next(BossRushModSystem.TrinketAccessories));
		}
	}
	public class DivineWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			player.GetModPlayer<EnchantmentModplayer>().SafeRequest_EnchantItem(1, 3);
			LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, 0, 0);
		}
	}
}
