using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.NPCs;
using BossRush.Texture;

namespace BossRush.Contents.Items.Spawner
{
    internal class LootboxLordSummon : BaseSpawnerItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override int[] NPCtypeToSpawn => new int[] { ModContent.NPCType<LootBoxLord>() };
        public override void PostSetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronHand] = true;
        }
        public override void SetSpawnerDefault(out int width, out int height)
        {
            height = 32;
            width = 32;
        }
        public override bool UseSpecialSpawningMethod => true;
        public override void SpecialSpawningLogic(Player player)
        {
            int spawnY = 250;
            NPC.SpawnBoss((int)(player.Center.X), (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0], player.whoAmI);
        }
    }
}