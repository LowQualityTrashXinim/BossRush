using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

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
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if(ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                if(spawnInfo.Player.GetModPlayer<ExtraChallengePlayer>().BatJungleANDCave)
                {
                    pool.Add(NPCID.CaveBat, 1);
                    pool.Add(NPCID.JungleBat, 1);
                    pool.Add(NPCID.IceBat, 1);
                }
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                Player player = Main.LocalPlayer;
                if(player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy)
                {
                    npc.defense += 50;
                    npc.defDefense += 50;
                    npc.damage += 50;
                    npc.lifeMax += 400;
                    npc.life += 400;
                    npc.knockBackResist = 0;
                }
                if(player.GetModPlayer<ExtraChallengePlayer>().BatJungleANDCave)
                {
                    npc.lifeMax += 1000;
                    npc.life += 1000;
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
                    player.GetModPlayer<ExtraChallengePlayer>().ChallengeChooser = Main.rand.Next(9);
                    player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount++;
                }
            }
        }
    }
}
