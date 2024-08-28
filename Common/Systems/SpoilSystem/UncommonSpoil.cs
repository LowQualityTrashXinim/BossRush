using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Chest;

namespace BossRush.Common.Systems.SpoilSystem;
public class UncommonSpoil {
	public class RareWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop();
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
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
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
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			chestplayer.GetAmount();
			int amount = chestplayer.potionTypeAmount;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(TerrariaArrayID.AllFood), chestplayer.potionNumAmount);
			}
		}
	}
}
