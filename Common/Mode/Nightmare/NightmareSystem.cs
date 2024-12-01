using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Common.Mode.Nightmare {
	internal class NightmareSystem : ModSystem {
		/// <returns>
		/// return <b>True</b> if the world is nightmare difficulty
		/// </returns>
		public static bool IsANightmareWorld() => ModContent.GetInstance<NightmareSystem>().NightmareWorld;
		public bool NightmareWorld = false;
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			if (UniversalSystem.CanAccessContent(UniversalSystem.NIGHTMARE_MODE)) {
				NightmareWorld = true;
			}
		}
		public override void SaveWorldData(TagCompound tag) {
			tag.Add("NightmareWorld", NightmareWorld);
		}
		public override void LoadWorldData(TagCompound tag) {
			if (tag.TryGet("NightmareWorld", out bool Nightmare)) {
				NightmareWorld = Nightmare;
			}
		}
	}
}
