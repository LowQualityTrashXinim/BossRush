using Terraria;
using Humanizer;
using Terraria.ID;
using System.Linq;
using BossRush.Common.Utils;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;

namespace BossRush.Common.Systems.SpoilSystem;

public class WeaponSpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		SpoilsPlayer spoilsplayer = Main.LocalPlayer.GetModPlayer<SpoilsPlayer>();
		if (chestplayer.weaponShowID == 0 || --chestplayer.counterShow <= 0) {
			chestplayer.weaponShowID = Main.rand.NextFromHashSet(LootboxSystem.GetItemPool(spoilsplayer.LootBoxSpoilThatIsNotOpen.First()).AllItemPool());
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
		if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_SPOIL)) {
			player.QuickSpawnItem(player.GetSource_ItemUse(ContentSamples.ItemsByType[itemsource]), Main.rand.Next(BossRushModSystem.WeaponRarityDB[0]));
			return;
		}
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
		if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_SPOIL)) {
			player.QuickSpawnItem(player.GetSource_ItemUse(ContentSamples.ItemsByType[itemsource]), Main.rand.Next(BossRushModSystem.AccRarityDB[0]));
			return;
		}
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
		if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_SPOIL)) {
			player.QuickSpawnItem(player.GetSource_ItemUse(ContentSamples.ItemsByType[itemsource]), Main.rand.Next(BossRushModSystem.HeadArmorRarityDB[0]));
			player.QuickSpawnItem(player.GetSource_ItemUse(ContentSamples.ItemsByType[itemsource]), Main.rand.Next(BossRushModSystem.BodyArmorRarityDB[0]));
			player.QuickSpawnItem(player.GetSource_ItemUse(ContentSamples.ItemsByType[itemsource]), Main.rand.Next(BossRushModSystem.LegsArmorRarityDB[0]));
			return;
		}
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
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2));
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetRelic(itemsource, player, 2);
	}
}
public class SkillSpoil : ModSpoil {
	public override string FinalDescription() {
		return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2));
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetSkillLootbox(itemsource, player, 2);
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
