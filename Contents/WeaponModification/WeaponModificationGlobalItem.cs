using BossRush.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.WeaponModification {
	internal class WeaponModificationGlobalItem : GlobalItem {
		public override bool InstancePerEntity => true;
		public int Delay = 0;
		public int Recharge = 0;
		public int[] ModWeaponSlotType;

		public override void OnCreated(Item item, ItemCreationContext context) {
			if (!Main.LocalPlayer.IsDebugPlayer())
				return;
			if (item.damage > 0 && ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
				Delay = item.useTime + Main.rand.Next(-item.useTime, item.useTime * 2);
				Recharge = item.useAnimation + Main.rand.Next(-item.useAnimation, item.useAnimation * 3);
				ModWeaponSlotType = new int[Main.rand.Next(2, 10)];
				Array.Fill(ModWeaponSlotType, -1);
			}
		}
		public override void UpdateInventory(Item item, Player player) {
			if (!player.IsDebugPlayer())
				return;
			if (item.damage > 0 && ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
				if (ModWeaponSlotType == null) {
					Delay = item.useTime + Main.rand.Next(-item.useTime, item.useTime);
					Recharge = item.useAnimation + Main.rand.Next(-item.useAnimation, item.useAnimation);
					ModWeaponSlotType = new int[Main.rand.Next(2, 10)];
					Array.Fill(ModWeaponSlotType, -1);
				}
			}
		}
		public string GetWeaponModificationStats() =>
			$"Item's modification delay : {Math.Round(Delay / 60f, 2)}s\n" +
			$"Item's modification recharge : {Math.Round(Recharge / 60f, 2)}s\n" +
			$"Item's modification slot : {ModWeaponSlotType.Length}";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (item.damage > 0 && ModWeaponSlotType != null && Main.LocalPlayer.IsDebugPlayer()) {
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification delay : {Math.Round(Delay / 60f, 2)}s"));
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification recharge : {Math.Round(Recharge / 60f, 2)}s"));
				tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Item's modification slot : {ModWeaponSlotType.Length}"));
			}
		}
		public override void SaveData(Item item, TagCompound tag) {
			if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
				tag.Add("ItemDelay", Delay);
				tag.Add("ItemRecharge", Recharge);
				tag.Add("ModWeaponSlotType", ModWeaponSlotType);
			}
		}
		public override void LoadData(Item item, TagCompound tag) {
			if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
				if (tag.TryGet("ItemDelay", out int DelayValue))
					Delay = DelayValue;
				if (tag.TryGet("ItemRecharge", out int RechargeValue))
					Recharge = RechargeValue;
				if (tag.TryGet("ModWeaponSlotType", out int[] TypeValue))
					ModWeaponSlotType = TypeValue;
			}
		}
	}
	class WeaponModificationGlobalProjectile : GlobalProjectile {
		public override bool InstancePerEntity => true;
		private int[] ModWeaponSlotTypeProjectile;
		public override void OnSpawn(Projectile projectile, IEntitySource source) {
			base.OnSpawn(projectile, source);
			if(source is IEntitySource_WithStatsFromItem {Item : not null} parent) {
				if(parent.Item.TryGetGlobalItem(out WeaponModificationGlobalItem globalitem)) {
					if(globalitem.ModWeaponSlotType == null) {
						return;
					}
					ModWeaponSlotTypeProjectile = globalitem.ModWeaponSlotType;
					for (int i = 0; i < globalitem.ModWeaponSlotType.Length; i++) {
						if (globalitem.ModWeaponSlotType[i] == -1)
							continue;
						ModifierWeaponLoader.GetWeaponMod(globalitem.ModWeaponSlotType[i]).ProjectileOnSpawn(projectile);
					}
				}
			}
		}
		public override bool PreAI(Projectile projectile) {
			return base.PreAI(projectile);
		}
		public override void AI(Projectile projectile) {
			base.AI(projectile);
		}
	}
}
