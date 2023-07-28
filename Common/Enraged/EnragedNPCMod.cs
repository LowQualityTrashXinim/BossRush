using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Common.Enraged
{
    internal class EnragedNPCMod : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged && !ModContent.GetInstance<BossRushModConfig>().Enraged)
            {
                return;
            }
            if (!BossRushUtils.IsAnyVanillaBossAlive())
            {
                return;
            }
            pool.Clear();
            if (NPC.AnyNPCs(NPCID.KingSlime))
            {
                //Slime
                pool.Add(NPCID.GreenSlime, 1.75f);
                pool.Add(NPCID.BlueSlime, 1.75f);
                pool.Add(NPCID.PurpleSlime, 1.75f);
                pool.Add(NPCID.RedSlime, 1.75f);
                pool.Add(NPCID.YellowSlime, 1.75f);
                pool.Add(NPCID.BlackSlime, 1.75f);
                pool.Add(NPCID.MotherSlime, 1.75f);
                pool.Add(NPCID.SpikedJungleSlime, 1.55f);
                pool.Add(NPCID.SpikedIceSlime, 1.55f);
                pool.Add(NPCID.UmbrellaSlime, 1.75f);
                pool.Add(NPCID.SlimeSpiked, 1.75f);
                if (Main.getGoodWorld)
                {
                    pool.Add(NPCID.LavaSlime, 0.75f);
                }
            }
            if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                //eye
                pool.Add(NPCID.DemonEye, 0.75f);
                pool.Add(NPCID.DemonEye2, 0.75f);
                pool.Add(NPCID.DemonEyeOwl, 0.75f);
                pool.Add(NPCID.DemonEyeSpaceship, 0.75f);
                pool.Add(NPCID.CataractEye, 0.75f);
                pool.Add(NPCID.CataractEye2, 0.75f);
                pool.Add(NPCID.DialatedEye, 0.75f);
                pool.Add(NPCID.DialatedEye2, 0.75f);
                pool.Add(NPCID.GreenEye, 0.75f);
                pool.Add(NPCID.GreenEye2, 0.75f);
                pool.Add(NPCID.PurpleEye, 0.75f);
                pool.Add(NPCID.PurpleEye2, 0.75f);
                pool.Add(NPCID.WanderingEye, 0.65f);
                pool.Add(NPCID.EyeballFlyingFish, 0.45f);
            }
            if (NPC.AnyNPCs(NPCID.EaterofWorldsBody) && spawnInfo.Player.ZoneOverworldHeight)
            {
                pool.Add(NPCID.Corruptor, 0.25f);
                pool.Add(NPCID.Slimer, 0.25f);
            }
            if (NPC.AnyNPCs(NPCID.BrainofCthulhu) && spawnInfo.Player.ZoneOverworldHeight)
            {
                pool.Add(NPCID.CrimsonBunny, 0.25f);
                pool.Add(NPCID.CrimsonGoldfish, 0.25f);
            }
            if (NPC.AnyNPCs(NPCID.QueenBee))
            {
                //bee
                pool.Add(NPCID.Bee, 0.8f);
                pool.Add(NPCID.BeeSmall, 0.8f);
                //Hornet
                pool.Add(NPCID.Hornet, 0.7f);
                pool.Add(NPCID.HornetFatty, 0.7f);
                pool.Add(NPCID.HornetHoney, 0.7f);
                pool.Add(NPCID.HornetLeafy, 0.7f);
                pool.Add(NPCID.HornetSpikey, 0.7f);
                pool.Add(NPCID.HornetStingy, 0.7f);
                //MossHornet
                pool.Add(NPCID.MossHornet, 0.5f);
                pool.Add(NPCID.BigMossHornet, 0.5f);
                pool.Add(NPCID.GiantMossHornet, 0.5f);
                pool.Add(NPCID.LittleMossHornet, 0.5f);
                pool.Add(NPCID.TinyMossHornet, 0.5f);
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            ModdedPlayer modplayer = player.GetModPlayer<ModdedPlayer>();
            if (modplayer.Enraged && !ModContent.GetInstance<BossRushModConfig>().Enraged)
            {
                return;
            }
            if (!BossRushUtils.IsAnyVanillaBossAlive())
            {
                return;
            }
            if (NPC.AnyNPCs(NPCID.KingSlime))
            {
                spawnRate = 70;
                maxSpawns = 150;
            }
            if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                spawnRate = 80;
                maxSpawns = 175;
            }
            if (NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                spawnRate = 80;
                maxSpawns = 250;
            }
            if (NPC.AnyNPCs(NPCID.BrainofCthulhu) && player.ZoneOverworldHeight)
            {
                spawnRate = 80;
                maxSpawns = 250;
            }
            if (NPC.AnyNPCs(NPCID.QueenBee))
            {
                spawnRate = 75;
                maxSpawns = 290;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (!target.GetModPlayer<ModdedPlayer>().Enraged)
            {
                return;
            }
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    target.AddBuff(BuffID.BrokenArmor, 90);
                    break;
                case NPCID.EyeofCthulhu:
                    target.AddBuff(BuffID.Cursed, 90);
                    target.AddBuff(BuffID.Bleeding, 150);
                    target.AddBuff(BuffID.Obstructed, 180);
                    target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
                    break;
                case NPCID.BrainofCthulhu:
                    target.AddBuff(164, 60);
                    target.AddBuff(BuffID.Ichor, 180);
                    break;
                case NPCID.EaterofWorldsHead:
                    target.AddBuff(BuffID.Weak, 180);
                    target.AddBuff(BuffID.CursedInferno, 300);
                    target.AddBuff(BuffID.BrokenArmor, 180);
                    break;
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
                    break;
                case NPCID.QueenBee:
                    target.AddBuff(BuffID.Venom, 180);
                    target.AddBuff(BuffID.Bleeding, 180);
                    break;
                default:
                    break;
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.boss)
            {
                int playerIndex = npc.lastInteraction;
                if (!Main.player[playerIndex].active || Main.player[playerIndex].dead)
                {
                    playerIndex = npc.FindClosestPlayer();
                }
                Player player = Main.player[playerIndex];
                player.GetModPlayer<GamblePlayer>().Roll++;
            }
        }
    }
}