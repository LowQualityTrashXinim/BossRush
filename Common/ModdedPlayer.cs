﻿//Terraria stuff
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
//Microsoft stuff
using System.Collections.Generic;
//general BossRush stuff
using BossRush.BuffAndDebuff;
using BossRush.Items.Chest;
using BossRush.Items.Note;
using BossRush.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark;
using BossRush.Items.Artifact;
using BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow;
using BossRush.Items.Spawner;
using BossRush.Items.Toggle;
using BossRush.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff;
using BossRush.Items.Potion;
using BossRush.Items.Accessories.GuideToMasterNinja;
using BossRush.Items;

namespace BossRush.Common
{
    class ModdedPlayer : ModPlayer
    {
        //Enraged boss
        public bool KingSlimeEnraged;
        public bool EoCEnraged;
        public bool BrainFuck;
        public bool EaterOfWorldEnraged;
        public bool QueenBeeEnraged;
        public bool MoonLordEnraged;
        /// <summary>
        /// This bool is to check if artifact can be active, use mostly to change the value of item can be drop from chest
        /// </summary>
        public bool ArtifactAllowance = false;
        /// <summary>
        /// This bool is to check whenever if player remove artifact mid fight in boss and then get it back in the game
        /// <br/>Useful to prevent confliction between 2 artifacts that modify player damage
        /// </summary>
        public bool ForceArtifact = true;
        /// <summary>
        /// This is to see if player have more artifact than they need, useful if you want artifact to not contradict each other
        /// </summary>
        public int ArtifactCount = 0;
        //ArtifactList
        int[] ArtifactList = new int[]{
            ModContent.ItemType<TokenofGreed>(),
            ModContent.ItemType<TokenofPride>(),
            ModContent.ItemType<SkillIssuedArtifact>(),
            ModContent.ItemType<GodDice>(),
            ModContent.ItemType<VampirismCrystal>() };
        //NoHiter
        public bool gitGud = false;
        public int HowManyBossIsAlive = 0;
        public bool LookingForBossVanilla()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.boss && npc.active)
                {
                    return true;
                }
                else if ((npc.type == NPCID.EaterofWorldsBody
                    || npc.type == NPCID.EaterofWorldsHead
                    || npc.type == NPCID.EaterofWorldsTail)
                    && npc.active)
                {
                    return true;
                }
            }
            return false;
        }

        private void ArtifactHandle()
        {
            HowManyBossIsAlive++;
            if (ArtifactCount == 1 && ForceArtifact)
            {
                ArtifactAllowance = true;
            }
            else
            {
                ArtifactAllowance = false;
                ForceArtifact = false;
            }
        }

        public override void PostUpdate()
        {
            HowManyBossIsAlive = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if ((npc.boss ||
                    npc.type == NPCID.EaterofWorldsBody
                    || npc.type == NPCID.EaterofWorldsHead
                    || npc.type == NPCID.EaterofWorldsTail)
                    && npc.active)
                {
                    // What happen when boss is alive
                    ArtifactHandle();
                }
                else if (i == Main.maxNPCs - 1 && HowManyBossIsAlive == 0) // What happen when boss is inactive
                {
                    ForceArtifact = true;
                    amountoftimegothit = 0;
                }
            }
            ArtifactCount = 0;
            for (int i = 0; i < ArtifactList.Length; i++)
            {
                if (Player.HasItem(ArtifactList[i])) ArtifactCount++;
            }
            BossRushModConfig configSetting = ModContent.GetInstance<BossRushModConfig>();
            //Enraged here
            if ((MoonLordEnraged || configSetting.Enraged) && NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                Player.AddBuff(ModContent.BuffType<MoonLordWrath>(), 5);
                Player.AddBuff(BuffID.PotionSickness, 5);
            }
            else if (!NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                MoonLordEnraged = false;
            }

            if ((KingSlimeEnraged || configSetting.Enraged) && NPC.AnyNPCs(NPCID.KingSlime))
            {
                Player.AddBuff(BuffID.Slimed, 5);
                Player.AddBuff(ModContent.BuffType<KingSlimeRage>(), 5);
            }
            else if (!NPC.AnyNPCs(NPCID.KingSlime))
            {
                KingSlimeEnraged = false;
            }

            if ((EoCEnraged || configSetting.Enraged) && NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                Player.AddBuff(BuffID.Blackout, 5);
                Player.AddBuff(BuffID.Darkness, 5);
                Player.AddBuff(ModContent.BuffType<EvilPresence>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                EoCEnraged = false;
            }

            if ((BrainFuck || configSetting.Enraged) && NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                Player.AddBuff(ModContent.BuffType<MindBreak>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                BrainFuck = false;
            }

            if ((EaterOfWorldEnraged || configSetting.Enraged) &&
                (NPC.AnyNPCs(NPCID.EaterofWorldsHead)
                || NPC.AnyNPCs(NPCID.EaterofWorldsBody)
                || NPC.AnyNPCs(NPCID.EaterofWorldsTail)))
            {
                if (Player.ZoneOverworldHeight)
                {
                    Player.AddBuff(BuffID.CursedInferno, 120);
                }
                Player.AddBuff(ModContent.BuffType<Rotting>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.EaterofWorldsHead) && !NPC.AnyNPCs(NPCID.EaterofWorldsBody) && !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
            {
                EaterOfWorldEnraged = false;
            }

            if ((QueenBeeEnraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.QueenBee))
            {
                if (Player.ZoneJungle)
                {
                    Player.AddBuff(BuffID.Poisoned, 90);
                }
                Player.AddBuff(ModContent.BuffType<RoyalAntiEscapeTm>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.QueenBee))
            {
                QueenBeeEnraged = false;
            }
            if (!LookingForBossVanilla())
            {
                KingSlimeEnraged = false;
                EoCEnraged = false;
                BrainFuck = false;
                EaterOfWorldEnraged = false;
                QueenBeeEnraged = false;
                MoonLordEnraged = false;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Player.HasBuff(ModContent.BuffType<BerserkBuff>()) && item.DamageType == DamageClass.Melee)
            {
                scale += .3f;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (Player.HeldItem.type == ModContent.ItemType<OvergrownMinishark>())
            {
                target.AddBuff(BuffID.Poisoned, 60);
            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new List<Item>() {
            new Item(ModContent.ItemType<WoodenTreasureChest>()),
            new Item(ModContent.ItemType<LunchBox>()),
            new Item(ModContent.ItemType<CursedSkull>())
            };
            if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                items.Add(new Item(ModContent.ItemType<BrokenArtifact>()));
                items.Add(new Item(ModContent.ItemType<SynergyEnergy>()));
                items.Add(new Item(ModContent.ItemType<PowerEnergy>()));
                items.Add(new Item(ModContent.ItemType<Note1>()));
            }
            if (ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself)//gitgudlol
            {
                items.Add(new Item(ItemID.RedPotion, 10));
            }
            if (Player.name == "LQTXinim")
            {
                items.Add(new Item(ModContent.ItemType<RainbowTreasureChest>()));
            }
            if (Player.name == "FeelingLucky")
            {
                items.Add(new Item(ModContent.ItemType<GodDice>()));
            }
            if (Player.name.ToLower().Trim().Contains("skillissue"))
            {
                items.Add(new Item(ModContent.ItemType<SkillIssuedArtifact>()));
            }
            if (Player.name == "ImNotGud")
            {
                items.Add(new Item(ModContent.ItemType<WoodenTreasureChest>(), 10));
            }
            if (Player.name.ToLower().Trim() == "drugaddict")
            {
                items.Add(new Item(ModContent.ItemType<WonderDrug>(), 99));
            }
            if (Player.name.Contains("Ninja"))
            {
                items.Add(new Item(ItemID.Katana));
                items.Add(new Item(ItemID.Shuriken, 100));
                items.Add(new Item(ItemID.ThrowingKnife, 100));
                items.Add(new Item(ItemID.PoisonedKnife, 100));
                items.Add(new Item(ItemID.BoneDagger, 100));
                items.Add(new Item(ItemID.FrostDaggerfish, 100));
                items.Add(new Item(ItemID.NinjaHood));
                items.Add(new Item(ItemID.NinjaShirt));
                items.Add(new Item(ItemID.NinjaPants));
                items.Add(new Item(ModContent.ItemType<GuideToMasterNinja>()));
                items.Add(new Item(ModContent.ItemType<GuideToMasterNinja2>()));
            }
            if (ModContent.GetInstance<BossRushModConfig>().EasyMode)
            {
                items.Add(new Item(ItemID.LifeCrystal, 3));
                items.Add(new Item(ItemID.ManaCrystal, 3));
                if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
                {
                    items.Add(new Item(Main.rand.Next(new int[] { ModContent.ItemType<AmethystBow>(), ModContent.ItemType<AmethystSwotaff>() })));
                }
            }
            if (Main.rand.NextBool(10))
            {
                items.Add(new Item(ModContent.ItemType<WonderDrug>()));
            }
            return items;
        }
        public int amountoftimegothit = 0;
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (LookingForBossVanilla())
            {
                if (gitGud)
                {
                    Player.KillMe(new PlayerDeathReason(), 9999999, hitDirection);
                    return;
                }
                else
                {
                    amountoftimegothit++;
                }
            }
            if (Player.HasBuff(ModContent.BuffType<GodVision>()))
            {
                Player.ClearBuff(ModContent.BuffType<GodVision>());
            }
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (NPC.AnyNPCs(NPCID.BrainofCthulhu) && BrainFuck)
            {
                Player.AddBuff(BuffID.PotionSickness, 240);
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (NPC.AnyNPCs(NPCID.KingSlime))
            {
                if (KingSlimeEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.SlimeCrown);
                KingSlimeEnraged = false;
            }
            else if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                if (EoCEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.SuspiciousLookingEye);
                EoCEnraged = false;
            }
            else if (NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                if (BrainFuck)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.BloodySpine);
                BrainFuck = false;
            }
            else if (NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                if (EaterOfWorldEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.WormFood);
                EaterOfWorldEnraged = false;
            }
            else if (NPC.AnyNPCs(NPCID.SkeletronHead))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<CursedDoll>());
            }
            else if (NPC.AnyNPCs(NPCID.QueenBee))
            {
                if (QueenBeeEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.Abeemination);
                QueenBeeEnraged = false;
            }
            else if (NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                Player.QuickSpawnItem(null, ItemID.GuideVoodooDoll);
            }
            else if (NPC.AnyNPCs(NPCID.QueenSlimeBoss))
            {
                Player.QuickSpawnItem(null, ItemID.QueenSlimeCrystal);
            }
            else if (NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalEye);
            }
            else if (NPC.AnyNPCs(NPCID.TheDestroyer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalWorm);
            }
            else if (NPC.AnyNPCs(NPCID.SkeletronPrime))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalSkull);
            }
            else if (NPC.AnyNPCs(NPCID.Plantera))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<PlanteraSpawn>());
            }
            else if (NPC.AnyNPCs(NPCID.Golem))
            {
                Player.QuickSpawnItem(null, ItemID.LihzahrdPowerCell);
            }
            else if (NPC.AnyNPCs(NPCID.DukeFishron))
            {
                Player.QuickSpawnItem(null, ItemID.TruffleWorm);
            }
            else if (NPC.AnyNPCs(NPCID.HallowBoss))
            {
                Player.QuickSpawnItem(null, ItemID.EmpressButterfly);
            }
            else if (NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                Player.QuickSpawnItem(null, ItemID.CelestialSigil);
                MoonLordEnraged = false;
            }
        }
    }
}