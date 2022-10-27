using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.ExtraChallengeConfig
{
    internal class ExtraChallengeGlobalNPCModifier : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if(player.GetModPlayer<ExtraChallengePlayer>().spawnRatex3)
            {
                spawnRate = 30;
                maxSpawns += 400;
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                Player player = Main.LocalPlayer;
                if(player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy)
                {
                    npc.defense += 30;
                    npc.defDefense += 30;
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.boss)
            {
                Player player = Main.LocalPlayer;
                if (ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
                {
                    player.GetModPlayer<ExtraChallengePlayer>().ChallengeChooser = Main.rand.Next(8);
                    player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount++;
                }
            }
        }
    }
}
