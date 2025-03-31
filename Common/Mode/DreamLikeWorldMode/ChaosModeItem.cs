using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModeItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public override void SetDefaults(Item entity) {
		base.SetDefaults(entity);
		ChaosModeSystem system = ModContent.GetInstance<ChaosModeSystem>();
		if (!system.ChaosMode) {
			return;
		}
		else {
			if (system.Dict_Chaos_Weapon.ContainsKey(entity.type)) {
				system.Dict_Chaos_Weapon[entity.type].ApplyInfo(ref entity);
			}
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ChaosModeSystem system = ModContent.GetInstance<ChaosModeSystem>();
		if (system.List_Ban_ItemID.Contains(item.type)) {
			tooltips.Add(new(Mod, "BannedWeapon", $"[c/{Color.Red.Hex3()}:Banned]"));
		}
		if (system.Dict_Chaos_Weapon.Keys.Contains(item.type)) {
			tooltips.Add(new(Mod, "BannedWeapon", $"[c/{Main.DiscoColor.Hex3()}:Chaotic]"));
		}
	}
	public override bool CanUseItem(Item item, Player player) {
		ChaosModeSystem system = ModContent.GetInstance<ChaosModeSystem>();
		if (!system.ChaosMode) {
			return base.CanUseItem(item, player);
		}
		if (system.List_Ban_ItemID.Contains(item.type)) {
			return false;
		}
		return base.CanUseItem(item, player);
	}
}
