using Terraria.ModLoader;

namespace BossRush.Common.Nightmare {
	internal class MasochistPlayer : ModPlayer {
		public override void OnEnterWorld() {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare)
				Player.difficulty = 2;
		}
	}
}
