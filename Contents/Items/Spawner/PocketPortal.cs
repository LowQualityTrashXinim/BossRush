using Terraria.ID;

namespace BossRush.Contents.Items.Spawner
{
    public class PocketPortal : BaseSpawnerItem
    {
        public override int[] NPCtypeToSpawn => new int[] { NPCID.DD2Bartender};
        public override void SetSpawnerDefault(out int width, out int height)
        {
            height = 55;
            width = 53;
        }
    }
}