using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Mode.DreamLikeWorld;

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
