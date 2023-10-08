using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace BossRush.Common.Nightmare {
	internal class MasochistSystem : ModSystem {
		public override string WorldCanBePlayedRejectionMessage(PlayerFileData playerData, WorldFileData worldData) {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return "Master difficulty world required";
			}
			return base.WorldCanBePlayedRejectionMessage(playerData, worldData);
		}
		public override bool CanWorldBePlayed(PlayerFileData playerData, WorldFileData worldFileData) {
			BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
			if (config.Nightmare) {
				return worldFileData.ForTheWorthy;
			}
			return base.CanWorldBePlayed(playerData, worldFileData);
		}
		public override void PreWorldGen() {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare && !Main.getGoodWorld) {
				Main.getGoodWorld = true;
			}
		}
	}
}