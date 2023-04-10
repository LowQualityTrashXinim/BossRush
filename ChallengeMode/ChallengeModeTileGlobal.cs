using BossRush.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.ChallengeMode
{
    internal class ChallengeModeTileGlobal : GlobalTile
    {
        int[] pots = new int[] { TileID.Pots, TileID.PotsSuspended, TileID.PottedCrystalPlants, TileID.PottedLavaPlants, TileID.PottedLavaPlantTendrils, TileID.PottedPlants1, TileID.PottedPlants2 };
        public override bool Drop(int i, int j, int type)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                foreach (int a in pots)
                {
                    return type != a;
                }
            }
            return base.Drop(i, j, type);
        }
    }
}
