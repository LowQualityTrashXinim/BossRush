//Terraria stuff
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
//general BossRush stuff
using BossRush.Chest;
using BossRush.BuffAndDebuff;
using BossRush.CustomPotion;
using BossRush.ExtraItem;
using BossRush.Accessories;
using BossRush.Artifact;
using BossRush.Note;
using BossRush.Weapon.MagicSynergyWeapon.Swotaff;
using BossRush.Weapon.RangeSynergyWeapon.MagicBow;
using BossRush.Weapon.RangeSynergyWeapon.OvergrownMinishark;
//Microsoft stuff
using System.Collections.Generic;

namespace BossRush
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
        //ArtifactList
        public bool ArtifactAllowance = false;
        public bool ForceArtifact = true;
        int ArtifactCount = 0;
        int[] ArtifactList = new int[]{ModContent.ItemType<TokenofGreed>(), ModContent.ItemType<TokenofQuality>()};
        //NoHiter
        public bool gitGud = false;
        public int HowManyBossIsAlive;

        public static bool LookingForBoss()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].boss && Main.npc[i].active)
                {
                    return true;
                }
            }
            return false;
        }

        public override void PostUpdate()
        {
            HowManyBossIsAlive = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].boss && Main.npc[i].active)
                {
                    // What happen when boss is alive
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
                else // What happen when boss is inactive
                {
                    ForceArtifact = true;
                }
            }
            ArtifactCount = 0;
            for (int i = 0; i < ArtifactList.Length; i++)
            {
                if (Player.HasItem(ArtifactList[i])) ArtifactCount++;
            }

            //Enraged here
            if ((MoonLordEnraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                Player.AddBuff(ModContent.BuffType<MoonLordWrath>(), 5);
                Player.AddBuff(BuffID.PotionSickness, 5);
            }
            else if (!NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                MoonLordEnraged = false;
            }

            if ((KingSlimeEnraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.KingSlime))
            {
                Player.AddBuff(BuffID.Slimed, 5);
                Player.AddBuff(ModContent.BuffType<KingSlimeRage>(), 5);
            }
            else if (!NPC.AnyNPCs(NPCID.KingSlime))
            {
                KingSlimeEnraged = false;
            }

            if ((EoCEnraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                Player.AddBuff(BuffID.Blackout, 5);
                Player.AddBuff(BuffID.Darkness, 5);
                Player.AddBuff(ModContent.BuffType<EvilPresence>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                EoCEnraged = false;
            }

            if ((BrainFuck || ModContent.GetInstance<BossRushModConfig>().Enraged) && NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                Player.AddBuff(ModContent.BuffType<MindBreak>(), 5);
            }
            if (!NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                BrainFuck = false;
            }

            if ((EaterOfWorldEnraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && (NPC.AnyNPCs(NPCID.EaterofWorldsHead) || NPC.AnyNPCs(NPCID.EaterofWorldsBody) || NPC.AnyNPCs(NPCID.EaterofWorldsTail)))
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
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            if(Player.HasBuff(ModContent.BuffType<BerserkBuff>()) && item.DamageType == DamageClass.Melee)
            {
                scale *= 1.3f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (ArtifactCount <= 1)
            {
                if (Player.HasItem(ModContent.ItemType<TokenofGreed>()))
                {
                    damage *= 0.6f;
                }
                if (Player.HasItem(ModContent.ItemType<TokenofQuality>()))
                {
                    damage *= 1.35f;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(Player.HeldItem.type == ModContent.ItemType<OvergrownMinishark>())
            {
                target.AddBuff(BuffID.Poisoned, 60);
            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new List<Item>();
            items.Add(new Item(ModContent.ItemType<WoodenTreasureChest>()));
            items.Add(new Item(ModContent.ItemType<LunchBox>()));
            items.Add(new Item(ModContent.ItemType<GitGudToggle>()));
            if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                items.Add(new Item(ModContent.ItemType<BrokenArtifact>()));
                items.Add(new Item(ModContent.ItemType<SynergyEnergy>()));
                items.Add(new Item(ModContent.ItemType<PowerEnergy>()));
                items.Add(new Item(ModContent.ItemType<Note1>()));
            }
            if(ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself)//gitgudlol
            {
                items.Add(new Item (ItemID.RedPotion, 10));
            }
            if (Player.name == "LQTXinim") 
            {
                items.Add(new Item(ModContent.ItemType<RainbowTreasureChest>()));
            }
            if(Player.name.ToLower().Trim() == "skillissue")
            {
                items.Add(new Item(ModContent.ItemType<SkillIssuedArtifact>()));
            }
            if(Player.name == "ImNotGud")
            {
                items.Add(new Item(ModContent.ItemType<WoodenTreasureChest>(),10));
            }
            if(Player.name.Contains("DrugAddict"))
            {
                items.Add(new Item(ModContent.ItemType<WonderDrug>(), 99));
            }
            if(Player.name.Contains("Ninja"))
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
            return items;
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (LookingForBoss())
            {
                if (gitGud)
                {
                    Player.KillMe(new PlayerDeathReason(), 99999, 0);
                    return;
                }
            }
            if(Player.HasBuff(ModContent.BuffType<GodVision>()))
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
                return;
            }
            if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                if (EoCEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.SuspiciousLookingEye);
                EoCEnraged = false;
                return;
            }
            if (NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                if (BrainFuck)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.BloodySpine);
                BrainFuck = false;
                return;
            }
            if (NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                if (EaterOfWorldEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.WormFood);
                EaterOfWorldEnraged = false;
                return;
            }
            if (NPC.AnyNPCs(NPCID.SkeletronHead))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<CursedDoll>());
                return;
            }
            if (NPC.AnyNPCs(NPCID.QueenBee))
            {
                if (QueenBeeEnraged)
                {
                    Player.QuickSpawnItem(null, ModContent.ItemType<PowerEnergy>());
                }
                Player.QuickSpawnItem(null, ItemID.Abeemination);
                QueenBeeEnraged = false;
                return;
            }
            if (NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                Player.QuickSpawnItem(null, ItemID.GuideVoodooDoll);
                return;
            }
            if (NPC.AnyNPCs(NPCID.QueenSlimeBoss))
            {
                Player.QuickSpawnItem(null, ItemID.QueenSlimeCrystal);
                return;
            }
            if (NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalEye);
                return;
            }
            if (NPC.AnyNPCs(NPCID.TheDestroyer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalWorm);
                return;
            }
            if (NPC.AnyNPCs(NPCID.SkeletronPrime))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalSkull);
                return;
            }
            if (NPC.AnyNPCs(NPCID.Plantera))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<PlanteraSpawn>());
                return;
            }
            if (NPC.AnyNPCs(NPCID.Golem))
            {
                Player.QuickSpawnItem(null, ItemID.LihzahrdPowerCell);
                return;
            }
            if (NPC.AnyNPCs(NPCID.DukeFishron))
            {
                Player.QuickSpawnItem(null, ItemID.TruffleWorm);
                return;
            }
            if (NPC.AnyNPCs(NPCID.HallowBoss))
            {
                Player.QuickSpawnItem(null, ItemID.EmpressButterfly);
                return;
            }
            if (NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                Player.QuickSpawnItem(null, ItemID.CelestialSigil);
                MoonLordEnraged = false;
            }
        }
    }
}