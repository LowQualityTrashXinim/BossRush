using BossRush.Common.Systems;
using BossRush.Contents.Items.Chest;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModePlayer : ModPlayer {
	public bool HasReceivedWeapon = false;
	public override void OnEnterWorld() {
		ChaosModeSystem system = ModContent.GetInstance<ChaosModeSystem>();
		if(!system.ChaosMode) {
			return;
		}
		if (!HasReceivedWeapon) {
			int type = Main.rand.Next(ModContent.GetInstance<ChaosModeSystem>().Dict_Chaos_Weapon.Keys.ToList());
			IEntitySource source = Player.GetSource_None();
			Player.QuickSpawnItem(source, type);
			LootBoxBase.AmmoForWeapon(BossRushModSystem.ListLootboxType[0], Player, type);
			HasReceivedWeapon = true;
		}
		//Check for chaos weapon
		for (int i = 0; i < 50; i++) {
			int type = Player.inventory[i].type;
			if (system.Dict_Chaos_Weapon.ContainsKey(type)) {
				system.Dict_Chaos_Weapon[type].ApplyInfo(ref Player.inventory[i]);
			}
		}
	}
	public override void UpdateEquips() {
		if(!ChaosModeSystem.Chaos()) {
			return;
		}
		PlayerStatsHandle handle = Player.GetModPlayer<PlayerStatsHandle>();
		handle.AugmentationChance += .3f;
		handle.RandomizeChanceEnchantment += .2f;
	}
	public override void SaveData(TagCompound tag) {
		tag["HasReceivedWeapon"] = HasReceivedWeapon;
	}
	public override void LoadData(TagCompound tag) {
		if(tag.TryGet("HasReceivedWeapon", out bool result)) {
			HasReceivedWeapon = result;
		}
	}
}
