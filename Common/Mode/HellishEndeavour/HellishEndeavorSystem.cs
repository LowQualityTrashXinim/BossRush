using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using BossRush.Common.Global;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Common.Mode.HellishEndeavour;
internal class HellishEndeavorSystem : ModSystem {
	/// <returns>
	/// return <b>True</b> if the world is nightmare difficulty
	/// </returns>
	public static bool Hellish() => ModContent.GetInstance<HellishEndeavorSystem>().HellishWorld;
	public bool HellishWorld = false;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (UniversalSystem.CanAccessContent(UniversalSystem.HELLISH_MODE)) {
			HellishWorld = true;
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag.Add("HellishWorld", HellishWorld);
	}
	public override void LoadWorldData(TagCompound tag) {
		if (tag.TryGet("HellishWorld", out bool HellishWorld)) {
			this.HellishWorld = HellishWorld;
		}
	}
}
public class HellishEndeavourPlayer : ModPlayer {
	public override void UpdateEquips() {
		if (HellishEndeavorSystem.Hellish()) {
			PlayerStatsHandle.AddStatsToPlayer(Player, PlayerStats.LootDropIncrease, Multiplicative: 0);
		}
	}
	public override void OnHurt(Player.HurtInfo info) {
		if (HellishEndeavorSystem.Hellish()) {
			PlayerDeathReason reason = new PlayerDeathReason();
			reason.SourceCustomReason = $"{Player.name} has fail the challenge";
			Player.KillMe(reason, 9999999999, info.HitDirection);
			return;
		}
	}
}
