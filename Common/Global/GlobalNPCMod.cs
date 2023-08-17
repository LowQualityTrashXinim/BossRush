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
using BossRush.Contents.Perks;

namespace BossRush.Common.Global
{
    class GlobalNPCMod : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int lifecrystal = 1;
            int manacrystal = 1;
            LeadingConditionRule ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
            LeadingConditionRule noHit = new LeadingConditionRule(new GitGudMode());
            LeadingConditionRule DropOnce = new LeadingConditionRule(new IsPlayerAlreadyHaveASpawner());
            LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
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
                lifecrystal += 2;
                manacrystal += 2;
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
                lifecrystal += 2;
                manacrystal += 2;
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
                lifecrystal += 5;
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
            IsABoss.OnSuccess(ItemDropRule.Common(ItemID.LifeCrystal, 1,lifecrystal, lifecrystal));
            IsABoss.OnSuccess(ItemDropRule.Common(ItemID.ManaCrystal, 1,manacrystal, manacrystal));
            IsABoss.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PerkChooser>()));
            npcLoot.Add(IsABoss);
        }
    }
}