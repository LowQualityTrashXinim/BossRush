using BossRush.Texture;
using Terraria.ID;

namespace BossRush.Contents.Items.Spawner
{
    internal class LunaticCultistSpawner : BaseSpawnerItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override int[] NPCtypeToSpawn => new int[] { NPCID.CultistBoss };
    }
}
