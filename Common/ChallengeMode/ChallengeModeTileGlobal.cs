using BossRush.Common.Systems;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.ChallengeMode {
	internal class ChallengeModeTileGlobal : GlobalTile {
		int[] pots = new int[] { TileID.Pots, TileID.PotsSuspended, TileID.PottedCrystalPlants, TileID.PottedLavaPlants, TileID.PottedLavaPlantTendrils, TileID.PottedPlants1, TileID.PottedPlants2 };
		public override bool CanDrop(int i, int j, int type) {
			if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				foreach (int a in pots) {
					return type != a;
				}
			}
			return base.CanDrop(i, j, type);
		}
	}
}
