using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.WeaponUpgrade;
public enum UpgradeWeaponID : ushort {
	ShortSwordThrown
}
public class UpgradeGlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon(true);
	}
}
public class UpgradeSystem : ModSystem {
	public static List<UpgradeWeaponID> WeaponUpgradeID = new();
	public override void Load() {
		WeaponUpgradeID = new();
	}
	public override void Unload() {
		WeaponUpgradeID = null;
	}
}
