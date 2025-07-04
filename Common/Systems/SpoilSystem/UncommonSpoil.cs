﻿using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Chest;
using Terraria.DataStructures;
using BossRush.Contents.Items.RelicItem;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using BossRush.Common.Global;

namespace BossRush.Common.Systems.SpoilSystem;
public class UncommonSpoil {
	public class RareWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			for (int i = 1; i <= 4; i++) {
				LootBoxBase.GetWeapon(out int returnWeapon, out int amount, i);
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), returnWeapon, amount);
				LootBoxBase.AmmoForWeapon(itemsource, player, returnWeapon);
			}
		}
	}
	public class WeaponPotionSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.Campfire);
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			return Description.FormatWith(
				Math.Ceiling(chestplayer.weaponAmount * .5f),
				chestplayer.potionTypeAmount
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
			LootBoxBase.GetPotion(itemsource, player);
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			int amount = chestplayer.potionTypeAmount;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(TerrariaArrayID.AllFood), chestplayer.potionNumAmount);
			}
		}
	}
	public class UpgradeAccSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override string FinalDisplayName() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			if (chestplayer.accShowID == 0 || --chestplayer.counterShow <= 0) {
				chestplayer.accShowID = Main.rand.Next(TerrariaArrayID.EveryCombatHealtMovehAcc);
				chestplayer.counterShow = 6;
			}
			return DisplayName.FormatWith(chestplayer.accShowID);
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1));
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
			for (int i = 0; i < amount; i++) {
				LootBoxBase.GetAccessories(itemsource, player, Main.rand.NextBool(5));
			}
		}
	}
	public class Tier2RelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1));
		}
		public override void OnChoose(Player player, int itemsource) {
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 2);
				}
			}
		}
	}
}
