using BossRush.Common.General;
using Terraria.ModLoader;

namespace BossRush.Common.Nightmare {
	internal class NightmarePlayer : ModPlayer {
		public override void OnEnterWorld() {
			if (NightmareSystem.IsANightmareWorld())
				Player.difficulty = 2;
		}
	}
}
