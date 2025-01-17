using System;
using Terraria;
using System.Linq;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using BossRush.Contents.Perks;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
internal class UnlimitedThrowable_GlobalItem : GlobalItem {
	public override bool CanReforge(Item item) {
		if (UpgradePlayer.Check_Upgrade(Main.LocalPlayer, WeaponUpgradeID.UnlimitedThrowable)) {
			if (TerrariaArrayID.SpecialPreBoss.Contains(item.type) || TerrariaArrayID.Special.Contains(item.type)) return true;
		}
		return base.CanReforge(item);
	}
	//This shit is so fuckery that idk how to properly implement it so eh, screw it
	public override void SetDefaults(Item entity) {
		if (UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.UnlimitedThrowable)) {
			if (TerrariaArrayID.SpecialPreBoss.Contains(entity.type) || TerrariaArrayID.Special.Contains(entity.type)) {
				entity.consumable = false;
				entity.damage += 10;
				entity.maxStack = 1;
				entity.stack = 1;
				entity.AllowReforgeForStackableItem = true;
				entity.crit += 4;
				entity.shootSpeed += 5;
				entity.Set_ItemCriticalDamage(.5f);
			}
		}
	}
	public override bool ConsumeItem(Item item, Player player) {
		if (UpgradePlayer.Check_Upgrade(Main.LocalPlayer, WeaponUpgradeID.UnlimitedThrowable)) {
			if (TerrariaArrayID.SpecialPreBoss.Contains(item.type) || TerrariaArrayID.Special.Contains(item.type)) {
				return false;
			}
		}
		return base.ConsumeItem(item, player);
	}
}
public class UnlimitedThrowable : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		switch (StackAmount(player)) {
			case 1:
				UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.UnlimitedThrowable);
				break;
			case 2:
				break;
		}
		Mod.Reflesh_GlobalItem(player);
	}
}
