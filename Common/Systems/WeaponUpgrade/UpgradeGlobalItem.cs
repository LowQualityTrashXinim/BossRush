﻿using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Systems.WeaponUpgrade;
public enum WeaponUpgradeID : short {
	ShortSwordThrown,
	UnlimitedThrowable,

}
public class UpgradePlayer : ModPlayer {
	public HashSet<WeaponUpgradeID> Upgrades;
	public override void SaveData(TagCompound tag) {
		tag["WeaponUpgrade"] = Upgrades.ToList();
	}
	public override void LoadData(TagCompound tag) {
		if(tag.TryGet("WeaponUpgrade", out List<WeaponUpgradeID> upgrade)) {
			Upgrades = upgrade.ToHashSet();
		}
	}
}
public class UpgradeGlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon(true);
	}
}
public class UpgradeSystem : ModSystem {
	public static List<WeaponUpgradeID> WeaponUpgradeID = new();
	public override void Load() {
		WeaponUpgradeID = new();
	}
	public override void Unload() {
		WeaponUpgradeID = null;
	}
}
public class UpgradeSerializer : TagSerializer<WeaponUpgradeID, TagCompound> {
	public override TagCompound Serialize(WeaponUpgradeID value) => new TagCompound {
		["WeaponUpgradeID"] = (short)value
	};

	public override WeaponUpgradeID Deserialize(TagCompound tag) => (WeaponUpgradeID)tag.Get<short>("WeaponUpgradeID");
}
