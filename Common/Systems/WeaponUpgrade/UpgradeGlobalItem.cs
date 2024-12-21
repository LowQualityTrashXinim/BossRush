using Terraria;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using BossRush.Texture;

namespace BossRush.Common.Systems.WeaponUpgrade;
public enum WeaponUpgradeID : short {
	None,
	ShortSwordThrown,
	UnlimitedThrowable,
	NaturalUpgrade,
}
public class UpgradePlayer : ModPlayer {
	public HashSet<WeaponUpgradeID> Upgrades = new();
	public static bool Check_Upgrade(Player player, WeaponUpgradeID id) {
		if (player.TryGetModPlayer(out UpgradePlayer modplayer)) {
			return modplayer.Upgrades.Contains(id);
		}
		return false;
	}
	public static void Add_Upgrade(Player player, WeaponUpgradeID id) {
		if (player.TryGetModPlayer(out UpgradePlayer modplayer)) {
			if (!modplayer.Upgrades.Contains(id)) {
				modplayer.Upgrades.Add(id);
			}
		}
	}
	public override void SaveData(TagCompound tag) {
		if (Upgrades != null)
			tag["WeaponUpgrade"] = Upgrades.ToList();
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("WeaponUpgrade", out List<WeaponUpgradeID> upgrade)) {
			Upgrades = upgrade.ToHashSet();
		}
	}
}
public class UpgradeReset : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		if(player.ItemAnimationJustStarted) {
			player.GetModPlayer<UpgradePlayer>().Upgrades.Clear();
		}
		return false;
	}
}
public class UpgradeSerializer : TagSerializer<WeaponUpgradeID, TagCompound> {
	public override TagCompound Serialize(WeaponUpgradeID value) => new TagCompound {
		["WeaponUpgradeID"] = (short)value
	};

	public override WeaponUpgradeID Deserialize(TagCompound tag) => (WeaponUpgradeID)tag.Get<short>("WeaponUpgradeID");
}
