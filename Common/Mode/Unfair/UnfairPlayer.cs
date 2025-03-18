using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.General;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.Mutation;

namespace BossRush.Common.Mode.Unfair;
class UnfairPlayer : ModPlayer {
	public bool UnfairMode = false;
	public override void OnEnterWorld() {
		if (!UnfairMode) {
			UnfairMode = ModContent.GetInstance<RogueLikeConfig>().UnfairMode;
		}
	}
	public override void UpdateEquips() {
		if (UnfairMode) {
			Player.GetModPlayer<ChestLootDropPlayer>().DropModifier *= 0;
			ModContent.GetInstance<MutationSystem>().MutationChance += 1;
		}
	}
	public override void SaveData(TagCompound tag) {
		tag["UnfairMode"] = UnfairMode;
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("UnfairMode", out bool mode)) {
			UnfairMode = mode;
		}
	}
}
