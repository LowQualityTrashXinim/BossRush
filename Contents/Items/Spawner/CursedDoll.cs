using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Spawner
{
    public class CursedDoll : BaseSpawnerItem
    {
        public override int[] NPCtypeToSpawn => new int[] { NPCID.SkeletronHead };
        public override void PostSetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronHand] = true;
        }
        public override void SetSpawnerDefault(out int width, out int height)
        {
            height = 55;
            width = 53;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime;
        }
    }
}