using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
//EnragedStuff
using BossRush.Contents.Items;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;

namespace BossRush.Common.Global
{
    class GlobalNPCMod : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
            LeadingConditionRule noHit = new LeadingConditionRule(new GitGudMode());
            LeadingConditionRule DropOnce = new LeadingConditionRule(new IsPlayerAlreadyHaveASpawner());
            LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
            IsABoss.OnSuccess(ItemDropRule.Common(ItemID.LifeCrystal));
            IsABoss.OnSuccess(ItemDropRule.Common(ItemID.ManaCrystal));
            if (npc.type == NPCID.KingSlime)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IronLootBox>()));
                npcLoot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingEye));
                //Enraged boss drop
                LeadingConditionRule KSE = new LeadingConditionRule(new BossIsEnragedBySpecialSpawner());
                KSE.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<KingSlimeDelight>()));
                KSE.OnSuccess(ItemDropRule.ByCondition(new SynergyDrop(), ModContent.ItemType<SynergyEnergy>(), 1, 2, 2));
                npcLoot.Add(KSE);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IronLootBox>()));
                npcLoot.Add(ExpertVSnormal);
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SilverLootBox>()));
                DropOnce.OnSuccess(ItemDropRule.ByCondition(new Conditions.IsCorruption(), ItemID.WormFood));
                DropOnce.OnSuccess(ItemDropRule.ByCondition(new Conditions.IsCrimson(), ItemID.BloodySpine));
                npcLoot.Add(DropOnce);
                npcLoot.Add(ExpertVSnormal);
                //Enraged boss drop
                LeadingConditionRule EOCE = new LeadingConditionRule(new BossIsEnragedBySpecialSpawner());
                EOCE.OnSuccess(ItemDropRule.BossBag(ItemID.TheEyeOfCthulhu));
                EOCE.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<EvilEye>()));
                EOCE.OnSuccess(ItemDropRule.ByCondition(new SynergyDrop(), ModContent.ItemType<SynergyEnergy>(), 1, 1, 1));
                npcLoot.Add(EOCE);
                //Expert Mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SilverLootBox>()));
            }
            if (System.Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWNoHitReward>()));
                npcLoot.Add(noHit);
                //Expert mode drop
                IsABoss.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<CorruptionLootBox>()));
                //normal drop
                IsABoss.OnSuccess(ItemDropRule.Common(ItemID.DeerThing));
                IsABoss.OnSuccess(ItemDropRule.Common(ItemID.Abeemination));
                IsABoss.OnSuccess(ItemDropRule.Common(ItemID.GoblinBattleStandard));
                IsABoss.OnSuccess(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<CursedDoll>()));
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CorruptionLootBox>()));
                npcLoot.Add(ExpertVSnormal);
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrimsonLootBox>()));
                npcLoot.Add(ItemDropRule.Common(ItemID.DeerThing));
                npcLoot.Add(ItemDropRule.Common(ItemID.Abeemination));
                npcLoot.Add(ItemDropRule.Common(ItemID.GoblinBattleStandard));
                npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<CursedDoll>()));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrimsonLootBox>()));
            }
            if (npc.type == NPCID.QueenBee)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HoneyTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Enraged boss drop
                LeadingConditionRule EnragedQB = new LeadingConditionRule(new QueenBeeEnranged());
                EnragedQB.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HoneyTreasureChest>(), 1, 3, 3));
                npcLoot.Add(EnragedQB);
                //Expert drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HoneyTreasureChest>()));
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldLootBox>()));
                npcLoot.Add(ItemDropRule.Common(ItemID.GuideVoodooDoll));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<GoldLootBox>()));
            }

            if (npc.type == NPCID.Deerclops)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IceLootBox>()));
                npcLoot.Add(ExpertVSnormal);
                //Enraged boss drop
                LeadingConditionRule hateyoulol = new LeadingConditionRule(new DeerclopHateYou());
                hateyoulol.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DeerclopTreasureChest>()));
                npcLoot.Add(hateyoulol);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IceLootBox>()));
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshNoHitReward>()));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                npcLoot.Add(ItemDropRule.Common(ItemID.MechanicalEye));
                npcLoot.Add(ItemDropRule.Common(ItemID.MechanicalWorm));
                npcLoot.Add(ItemDropRule.Common(ItemID.MechanicalSkull));
                npcLoot.Add(ItemDropRule.Common(ItemID.QueenSlimeCrystal));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BleedingWorm>()));
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ShadowTreasureChest>()));
            }

            if (npc.type == NPCID.BloodNautilus)
            {
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BloodTreasureChest>()));
            }

            if (npc.type == NPCID.QueenSlimeBoss)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrystalTreasureChest>()));
                npcLoot.Add(ItemDropRule.ByCondition(new SynergyDrop(), ModContent.ItemType<SynergyEnergy>()));
            }

            if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MechTreasureChest>()));
            }
            if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer)
            {
                LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
                //NoHit Mode drop
                leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new GitGudMode(), ModContent.ItemType<MechTreasureChest>(), 1, 2, 2));
                //Normal mode drop
                leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsBossAndNotExpert(), ModContent.ItemType<MechTreasureChest>()));
                //Expert mode drop
                leadingConditionRule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<MechTreasureChest>()));
                npcLoot.Add(leadingConditionRule);
            }
            if (npc.type == NPCID.Plantera)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                npcLoot.Add(ItemDropRule.Common(ItemID.LihzahrdPowerCell));
                npcLoot.Add(ItemDropRule.Common(ItemID.LihzahrdAltar));
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<NatureTreasureChest>()));
            }
            if (npc.type == NPCID.Golem)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>()));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaticCultistSpawner>()));
                npcLoot.Add(ItemDropRule.Common(ItemID.TruffleWorm));
                npcLoot.Add(ItemDropRule.Common(ItemID.EmpressButterfly, 1, 5, 5));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LihzahrdTreasureChest>()));
            }
            if (npc.type == NPCID.HallowBoss)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Enraged boss drop
                LeadingConditionRule rule = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());
                rule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<EmpressTreasureChest>()));
                npcLoot.Add(rule);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
            }
            if (npc.type == NPCID.DukeFishron)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DukeTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Enraged boss drop
                LeadingConditionRule rule = new LeadingConditionRule(new DukeIsEnrage());
                rule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DukeTreasureChest>()));
                npcLoot.Add(rule);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
            }
            if (npc.type == NPCID.CultistBoss)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>()));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LunaticLootBox>()));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                //NoHit mode drop
                noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>(), 1, 2, 2));
                npcLoot.Add(noHit);
                //Normal mode drop
                ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>()));
                npcLoot.Add(ExpertVSnormal);
                //Expert mode drop
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonLordEnrage>()));
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MoonTreasureChest>()));
            }
            npcLoot.Add(IsABoss);
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.KingSlime))
            {
                pool.Clear();
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
            if ((spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                pool.Clear();
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
            if ((spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.EaterofWorldsBody) && spawnInfo.Player.ZoneOverworldHeight)
            {
                pool.Add(NPCID.Corruptor, 0.25f);
                pool.Add(NPCID.Slimer, 0.25f);
            }
            if ((spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.BrainofCthulhu) && spawnInfo.Player.ZoneOverworldHeight)
            {
                pool.Add(NPCID.CrimsonBunny, 0.25f);
                pool.Add(NPCID.CrimsonGoldfish, 0.25f);
            }
            if ((spawnInfo.Player.GetModPlayer<ModdedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.QueenBee))
            {
                pool.Clear();
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
            if ((modplayer.Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.KingSlime))
            {
                spawnRate = 70;
                maxSpawns = 150;
            }
            if ((modplayer.Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                spawnRate = 80;
                maxSpawns = 175;
            }
            if ((modplayer.Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                spawnRate = 80;
                maxSpawns = 250;
            }
            if ((modplayer.Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.BrainofCthulhu) && player.ZoneOverworldHeight)
            {
                spawnRate = 80;
                maxSpawns = 250;
            }
            if ((modplayer.Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.QueenBee))
            {
                spawnRate = 75;
                maxSpawns = 290;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if(!target.GetModPlayer<ModdedPlayer>().Enraged)
            {
                return;
            }
            if (npc.type == NPCID.KingSlime)
            {
                target.AddBuff(BuffID.BrokenArmor, 90);
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                target.AddBuff(BuffID.Cursed, 90);
                target.AddBuff(BuffID.Bleeding, 150);
                target.AddBuff(BuffID.Obstructed, 180);
                target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                target.AddBuff(164, 60);
                target.AddBuff(BuffID.Ichor, 180);
            }
            if (npc.type == NPCID.EaterofWorldsHead)
            {
                target.AddBuff(BuffID.Weak, 180);
                target.AddBuff(BuffID.CursedInferno, 300);
                target.AddBuff(BuffID.BrokenArmor, 180);
            }
            if ((npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail))
            {
                target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
            }
            if (npc.type == NPCID.QueenBee)
            {
                target.AddBuff(BuffID.Venom, 180);
                target.AddBuff(BuffID.Bleeding, 180);
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
        public override void SetDefaults(NPC npc)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            if (npc.boss)
            {
                npc.lavaImmune = true;
            }
            if (npc.type == NPCID.CultistBoss)
            {
                npc.lifeMax += 15000;
                npc.defense += 30;
            }
        }
        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            if (npc.type == NPCID.CultistBoss)
            {
                if (npc.ai[0] == 5f)
                {
                    if (npc.ai[1] >= 120f)
                    {
                        npc.chaseable = true;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[3] += 1f;
                        npc.velocity = Vector2.Zero;
                        npc.netUpdate = true;
                    }
                }
                for (int i = 0; i < 300; i++)
                {
                    int dust1 = Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(2000f, 2000f), 0, 0, DustID.SolarFlare);
                    Main.dust[dust1].noGravity = true;
                    Main.dust[dust1].velocity = (Main.dust[dust1].position - npc.Center).SafeNormalize(Vector2.Zero) * 3f;
                }
                if (!BossRushUtils.CompareSquareFloatValue(npc.Center, Main.player[npc.target].Center, 2000))
                {
                    Main.player[npc.target].AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 120);
                    for (int i = 0; i < 3; i++)
                    {
                        int dust = Dust.NewDust(Main.player[npc.target].Center, 0, 0, DustID.SolarFlare);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(7f, 7f);
                        Main.dust[dust].fadeIn = 2f;
                    }
                }
            }
        }
    }
}