using System;
using Terraria;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
//TODO : remove the current weapon modification system
namespace BossRush.Contents.WeaponModification {
	internal class WeaponModificationGlobalItem : GlobalItem {
		public override bool InstancePerEntity => true;
		public int Delay = 0;
		public int Recharge = 0;
		public int[] ModWeaponSlotType;

		public string GetWeaponModificationStats() =>
			$"Item's modification delay : {Math.Round(Delay / 60f, 2)}s\n" +
			$"Item's modification recharge : {Math.Round(Recharge / 60f, 2)}s\n" +
			$"Item's modification slot : {ModWeaponSlotType.Length}";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (item.damage > 0 && ModWeaponSlotType != null) {
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification delay : {Math.Round(Delay / 60f, 2)}s"));
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification recharge : {Math.Round(Recharge / 60f, 2)}s"));
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification slot : {ModWeaponSlotType.Length}"));
			}
		}
		public override void SaveData(Item item, TagCompound tag) {
			if (ModContent.GetInstance<BossRushModConfig>().SynergyMode || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature) {
				tag.Add("ItemDelay", Delay);
				tag.Add("ItemRecharge", Recharge);
				tag.Add("ModWeaponSlotType", ModWeaponSlotType);
			}
		}
		public override void LoadData(Item item, TagCompound tag) {
			if (ModContent.GetInstance<BossRushModConfig>().SynergyMode || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature) {
				if (tag.TryGet("ItemDelay", out int DelayValue))
					Delay = DelayValue;
				if (tag.TryGet("ItemRecharge", out int RechargeValue))
					Recharge = RechargeValue;
				if (tag.TryGet("ModWeaponSlotType", out int[] TypeValue))
					ModWeaponSlotType = TypeValue;
			}
		}
	}
}
