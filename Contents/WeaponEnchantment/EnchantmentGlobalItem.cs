using System;
using Terraria;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentGlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot;
	public string GetWeaponModificationStats() =>
		$"Item's modification slot : {EnchantmenStlot.Length}";
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.damage > 0 && EnchantmenStlot != null) {
			tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification slot : {EnchantmenStlot.Length}"));
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		if (ModContent.GetInstance<BossRushModConfig>().SynergyMode || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature) {
			tag.Add("EnchantmentSlot", EnchantmenStlot);
		}
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (ModContent.GetInstance<BossRushModConfig>().SynergyMode || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature) {
			if (tag.TryGet("EnchantmentSlot", out int[] TypeValue))
				EnchantmenStlot = TypeValue;
		}
	}
}
