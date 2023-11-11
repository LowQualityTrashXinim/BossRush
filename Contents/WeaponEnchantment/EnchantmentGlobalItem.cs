using Terraria;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentGlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot = new int[3];
	public override void OnCreated(Item item, ItemCreationContext context) {
	}
	public string GetWeaponModificationStats() =>
		$"Item's modification slot : {EnchantmenStlot.Length}";
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE))
			return;
		if (item.damage > 0 && EnchantmenStlot != null) {
			tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification slot : {EnchantmenStlot.Length}"));
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		if (UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE)) {
			tag.Add("EnchantmentSlot", EnchantmenStlot);
		}
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE)){
			if (tag.TryGet("EnchantmentSlot", out int[] TypeValue))
				EnchantmenStlot = TypeValue;
		}
	}
}
