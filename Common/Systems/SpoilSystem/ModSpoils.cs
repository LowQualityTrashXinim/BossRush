using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using Humanizer;
using BossRush.Common.Utils;
using System.Collections.Generic;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.aDebugItem;

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
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetAccessories(itemsource, player);
	}
}

public class ArmorSpoil : ModSpoil {
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetArmorForPlayer(player.GetSource_OpenItem(itemsource), player);
	}
}

public class PotionSpoil : ModSpoil {
	public override string FinalDisplayName() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		if (--chestplayer.counterShow <= 0) {
			List<int> potiontotal = [.. TerrariaArrayID.NonMovementPotion, .. TerrariaArrayID.MovementPotion];
			chestplayer.potionShowID = Main.rand.Next(potiontotal);
			chestplayer.counterShow = 6;
		}
		return DisplayName.FormatWith(chestplayer.potionShowID);
	}
	public override string FinalDescription() {
		ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
		chestplayer.GetAmount();
		return Description.FormatWith(chestplayer.potionNumAmount);
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
public class SuppliesPackage : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = ItemRarityID.Blue;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return Main.rand.NextBool(3);
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
		LootBoxBase.GetArmorPiece(itemsource, player, true);
		LootBoxBase.GetSkillLootbox(itemsource, player);
		LootBoxBase.GetRelic(itemsource, player, 1);
	}
}
public class RareArmorPiece : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = ItemRarityID.Yellow;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return Main.rand.NextFloat() <= .1f;
	}
	public override void OnChoose(Player player, int itemsource) {
		LootBoxBase.GetArmorPiece(itemsource, player);
	}
}
public class PerkSpoil : ModSpoil {
	public override void SetStaticDefault() {
		RareValue = ItemRarityID.Purple;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return Main.rand.NextFloat() <= .02f;
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
		RareValue = ItemRarityID.Yellow;
	}
	public override bool IsSelectable(Player player, Item itemsource) {
		return Main.rand.NextFloat() <= .2f;
	}
	public override void OnChoose(Player player, int itemsource) {
		int amount = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1);
		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(BossRushModSystem.LostAccessories));
		}
	}
}
