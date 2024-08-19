using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using Humanizer;
using BossRush.Common.Utils;
using System.Collections.Generic;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.aDebugItem;
using System;

namespace BossRush.Common.Systems.SpoilSystem;

public class WeaponSpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		if (chestplayer.weaponShowID == 0 || --chestplayer.counterShow <= 0) {
			chestplayer.weaponShowID = Main.rand.NextFromHashSet(LootboxSystem.GetItemPool(SpoilsUIState.Current_OpenLootBox).AllItemPool());
			chestplayer.counterShow = 6;
		}
		return DisplayName.FormatWith(chestplayer.weaponShowID);
	}
	public override string FinalDescription() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		chestplayer.GetAmount();
		return Description.FormatWith(chestplayer.weaponAmount);
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player);
	}
}

public class AccessorySpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		if (chestplayer.accShowID == 0 || --chestplayer.counterShow <= 0) {
			chestplayer.accShowID = Main.rand.Next(TerrariaArrayID.EveryCombatHealtMovehAcc);
			chestplayer.counterShow = 6;
		}
		return DisplayName.FormatWith(chestplayer.accShowID);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1));
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1);
		for (int i = 0; i < amount; i++) {
			LootBoxBase.GetAccessories(itemsource, player);
		}
	}
}

public class ArmorSpoil : ModSpoil {
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.ArmorStatue);
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetArmorForPlayer(player.GetSource_OpenItem(itemsource), player);
	}
}

public class PotionSpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		if (--chestplayer.counterShow <= 0 || chestplayer.potionShowID == ItemID.None) {
			List<int> potiontotal = [.. TerrariaArrayID.NonMovementPotion, .. TerrariaArrayID.MovementPotion];
			chestplayer.potionShowID = Main.rand.Next(potiontotal);
			chestplayer.counterShow = 6;
		}
		return DisplayName.FormatWith(chestplayer.potionShowID);
	}
	public override string FinalDescription() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		chestplayer.GetAmount();
		return Description.FormatWith(chestplayer.potionTypeAmount);
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetPotion(itemsource, player);
	}
}

public class RelicSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(3));
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetRelic(itemsource, player, 3);
	}
}
public class SkillSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(3));
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetSkillLootbox(itemsource, player, 3);
	}
}
public class FoodSpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		if (--chestplayer.counterShow <= 0 || chestplayer.foodshowID == ItemID.None) {
			chestplayer.foodshowID = Main.rand.Next(TerrariaArrayID.AllFood);
			chestplayer.counterShow = 6;
		}
		return DisplayName.FormatWith(chestplayer.foodshowID);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(6));
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(6);
		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(TerrariaArrayID.AllFood));
		}
	}
}
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
		return SpoilDropRarity.UncommonDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
		LootBoxBase.GetArmorPiece(itemsource, player, true);
		LootBoxBase.GetSkillLootbox(itemsource, player);
		LootBoxBase.GetRelic(itemsource, player);
	}
}
public class RareArmorPiece : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.Rare;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return SpoilDropRarity.RareDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetArmorPiece(itemsource, player);
	}
}
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
		}
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
public class LostAccessorySpoil : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.Rare;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return SpoilDropRarity.RareDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2);
		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(BossRushModSystem.LostAccessories));
		}
	}
}
public class SSRPerkSpoil : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.SSR;
	}
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.FallenStar);
	}
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1), Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2));
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return SpoilDropRarity.SSRDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		int type = ModContent.ItemType<WorldEssence>();
		if (Main.rand.NextFloat() <= .01f) {
			type = ModContent.ItemType<PerkDebugItem>();
		}
		player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), type);
		LootBoxBase.GetSkillLootbox(itemsource, player);
		LootBoxBase.GetRelic(itemsource, player, 2);
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
public class ArmorAccessorySpoil : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.Rare;
	}
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.ArmorStatue);
	}
	public override string FinalDescription() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		return Description.FormatWith(
			chestplayer.ModifyGetAmount(1),
			chestplayer.ModifyGetAmount(2)
			);
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return SpoilDropRarity.RareDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2);
		for (int i = 0; i < amount; i++) {
			LootBoxBase.GetAccessories(itemsource, player);
		}
		int amount2 = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1);
		for (int i = 0; i < amount2; i++) {
			LootBoxBase.GetArmorPiece(itemsource, player);
		}
	}
}

public class RoguelikeSpoil : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.Rare;
	}
	public override string FinalDisplayName() {
		return DisplayName.FormatWith(ItemID.FallenStar);
	}
	public override string FinalDescription() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		return Description.FormatWith(
			chestplayer.ModifyGetAmount(2),
			chestplayer.ModifyGetAmount(4)
			);
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return SpoilDropRarity.RareDrop();
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount1 = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2);
		int amount2 = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(4);
		for (int i = 0; i < amount2; i++) {
			LootBoxBase.GetRelic(itemsource, player);
		}
		LootBoxBase.GetSkillLootbox(itemsource, player, amount1);
	}
}

public class WeaponUpgrade : ModSpoil {
	public override bool IsSelectable(Player player, Item itemsource) {
		return false;
	}
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.Rare;
	}
}

public class ArtifactUpgrade : ModSpoil {
	public override bool IsSelectable(Player player, Item itemsource) {
		return false;
	}
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.SSR;
	}
}
public class Curses : ModSpoil {
	public override bool IsSelectable(Player player, Item itemsource) {
		return false;
	}
	public override void SetStaticDefault() {
		RareValue = SpoilDropRarity.SuperRare;
	}
}
