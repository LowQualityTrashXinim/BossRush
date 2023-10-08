using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using BossRush.Contents.Perks;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.aDebugItem;
using BossRush.Contents.Items.Accessories.GuideToMasterNinja;

namespace BossRush.Common
{
    class ModdedPlayer : ModPlayer
    {
        //NoHiter
        public int gitGud = 0;
        public int HowManyBossIsAlive = 0;
        public override void OnEnterWorld()
        {
            Main.NewText("Currently the mod are extremely incompleted, the development currently only focusing on pre hardmode, so hardmode will be lacking a lot");
            Main.NewText("The dev is currently applying bandage fixes so the development could actually move on to important stuff");
            if (Main.ActiveWorldFileData.GameMode == 0)
            {
                Main.NewText("Yo this guys playing on easy mode lol, skill issues spotted !");
            }
        }
        public override void PreUpdate()
        {
            CheckHowManyHit();
        }
        private void CheckHowManyHit()
        {
            HowManyBossIsAlive = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if ((npc.boss || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) && npc.active)
                {
                    HowManyBossIsAlive++;
                }
                if (i == Main.maxNPCs - 1 && HowManyBossIsAlive == 0) // What happen when boss is inactive
                {
                    amountoftimegothit = 0;
                }
            }
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default; mana = StatModifier.Default;
            if (Main.ActiveWorldFileData.GameMode == 0)
            {
                health.Base = 100;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Player.HasBuff(ModContent.BuffType<BerserkBuff>()) && item.DamageType == DamageClass.Melee)
            {
                scale += .3f;
            }
        }
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new List<Item>() {
                        new Item(ModContent.ItemType<BundleHelper>()),
                        new Item(ModContent.ItemType<LunchBox>()),
                        new Item(ItemID.Safe),
                        new Item(ItemID.MoneyTrough),
                        new Item(ItemID.PlatinumPickaxe),
                        new Item(ItemID.PlatinumAxe),
                        new Item(ModContent.ItemType<BuilderLootBox>())
            };
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                items.Add(new Item(ItemID.ManaCrystal, 5));
                items.Add(new Item(ModContent.ItemType<DayTimeCycle>()));
                items.Add(new Item(ModContent.ItemType<CursedSkull>()));
                items.Add(new Item(ModContent.ItemType<BiomeToggle>()));
            }
            if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                //items.Add(new Item(ModContent.ItemType<StarterPerkChooser>()));
                //items.Add(new Item(ModContent.ItemType<BrokenArtifact>()));
                items.Add(new Item(ModContent.ItemType<SynergyEnergy>()));
            }
            if (ModContent.GetInstance<BossRushModConfig>().Nightmare)//gitgudlol
            {
                items.Add(new Item(ModContent.ItemType<PremiumCardPacket>()));
                items.Add(new Item(ItemID.RedPotion, 10));
            }
            if (Player.name == "LQTXinim" || Player.name == "LowQualityTrashXinim")
            {
                items.Add(new Item(ModContent.ItemType<RainbowTreasureChest>()));
            }
            if (Player.name == "FeelingLucky")
            {
                items.Add(new Item(ModContent.ItemType<GodDice>()));
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
            if (Player.name == "AccessToHiddenBoss")
            {
                items.Add(new Item(ModContent.ItemType<IronLootBox>()));
                items.Add(new Item(ModContent.ItemType<SilverLootBox>()));
                items.Add(new Item(ModContent.ItemType<GoldLootBox>()));
                items.Add(new Item(ModContent.ItemType<CorruptionLootBox>()));
                items.Add(new Item(ModContent.ItemType<CrimsonLootBox>()));
                items.Add(new Item(ModContent.ItemType<IceLootBox>()));
                items.Add(new Item(ModContent.ItemType<HoneyTreasureChest>()));
                items.Add(new Item(ModContent.ItemType<LootboxLordSummon>()));
                items.Add(new Item(ModContent.ItemType<PerkChooser>(), 8));
                items.Add(new Item(ItemID.LifeCrystal, 15));
                items.Add(new Item(ItemID.ManaCrystal, 4));
                items.Add(new Item(ItemID.KingSlimeBossBag));
                items.Add(new Item(ItemID.EyeOfCthulhuBossBag));
                items.Add(new Item(ItemID.EaterOfWorldsBossBag));
                items.Add(new Item(ItemID.BrainOfCthulhuBossBag));
                items.Add(new Item(ItemID.SkeletronBossBag));
                items.Add(new Item(ItemID.QueenBeeBossBag));
                items.Add(new Item(ItemID.DeerclopsBossBag));
            }
            if (Player.name == "I want to die" || Player.name == "Give me hell")
            {
                items.Clear();
                items.Add(new Item(ModContent.ItemType<WoodenLootBox>()));
                items.Add(new Item(ModContent.ItemType<CursedSkull>()));
                items.Add(new Item(ModContent.ItemType<CardPacket>()));
                items.Add(new Item(ModContent.ItemType<PowerEnergy>()));
            }
            if (Player.IsDebugPlayer())
            {
                items.Add(new Item(ModContent.ItemType<ModStatsDebugger>()));
                items.Add(new Item(ModContent.ItemType<ShowPlayerStats>()));
            }
            return items;
        }
        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            itemsByMod["Terraria"].Clear();
        }
        public int amountoftimegothit = 0;
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player.HasBuff(ModContent.BuffType<Protection>()))
            {
                Player.Heal(Player.statLifeMax2);
                Player.ClearBuff(ModContent.BuffType<Protection>());
                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Player.HasBuff(ModContent.BuffType<GodVision>()))
            {
                Player.ClearBuff(ModContent.BuffType<GodVision>());
            }
            if (BossRushUtils.IsAnyVanillaBossAlive())
            {
                if (gitGud > 0)
                {
                    PlayerDeathReason reason = new PlayerDeathReason();
                    reason.SourceCustomReason = $"{Player.name} has fail the challenge";
                    Player.KillMe(reason, 9999999999, info.HitDirection);
                    return;
                }
                else
                {
                    amountoftimegothit++;
                }
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            SpawnItem();
            if (ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                DeleteCardItem();
            }
        }
        private void DeleteCardItem()
        {
            for (int i = 0; i < Main.item.Length; i++)
            {
                Item item = Main.item[i];
                if (item is null || item.ModItem is null)
                {
                    continue;
                }
                if (item.ModItem is CardPacketBase)
                {
                    item.active = false;
                }
            }
            foreach (var item in Player.inventory)
            {
                if (item is null || item.ModItem is null)
                {
                    continue;
                }
                if (item.ModItem is CardPacketBase)
                {
                    item.stack = 0;
                    item.active = false;
                }
            }
        }
        private void SpawnItem()
        {
            if (NPC.AnyNPCs(NPCID.KingSlime))
            {
                Player.QuickSpawnItem(null, ItemID.SlimeCrown);
            }
            if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
            {
                Player.QuickSpawnItem(null, ItemID.SuspiciousLookingEye);
            }
            if (NPC.AnyNPCs(NPCID.BrainofCthulhu))
            {
                Player.QuickSpawnItem(null, ItemID.BloodySpine);
            }
            if (NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                Player.QuickSpawnItem(null, ItemID.WormFood);
            }
            if (NPC.AnyNPCs(NPCID.SkeletronHead))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<CursedDoll>());
            }
            if (NPC.AnyNPCs(NPCID.QueenBee))
            {
                Player.QuickSpawnItem(null, ItemID.Abeemination);
            }
            if (NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                Player.QuickSpawnItem(null, ItemID.GuideVoodooDoll);
            }
            if (NPC.AnyNPCs(NPCID.QueenSlimeBoss))
            {
                Player.QuickSpawnItem(null, ItemID.QueenSlimeCrystal);
            }
            if (NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalEye);
            }
            if (NPC.AnyNPCs(NPCID.TheDestroyer))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalWorm);
            }
            if (NPC.AnyNPCs(NPCID.SkeletronPrime))
            {
                Player.QuickSpawnItem(null, ItemID.MechanicalSkull);
            }
            if (NPC.AnyNPCs(NPCID.Plantera))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<PlanteraSpawn>());
            }
            if (NPC.AnyNPCs(NPCID.Golem))
            {
                Player.QuickSpawnItem(null, ItemID.LihzahrdPowerCell);
            }
            if (NPC.AnyNPCs(NPCID.DukeFishron))
            {
                Player.QuickSpawnItem(null, ItemID.TruffleWorm);
            }
            if (NPC.AnyNPCs(NPCID.HallowBoss))
            {
                Player.QuickSpawnItem(null, ItemID.EmpressButterfly);
            }
            if (NPC.AnyNPCs(NPCID.CultistBoss))
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<LunaticCultistSpawner>());
            }
            if (NPC.AnyNPCs(NPCID.MoonLordCore))
            {
                Player.QuickSpawnItem(null, ItemID.CelestialSigil);
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.GodUltimateChallenge);
            packet.Write((byte)Player.whoAmI);
            packet.Write(gitGud);
            packet.Send(toWho, fromWho);
        }
        public override void Initialize()
        {
            gitGud = 0;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["gitgud"] = gitGud;
        }
        public override void LoadData(TagCompound tag)
        {
            gitGud = (int)tag["gitgud"];
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            gitGud = reader.ReadInt32();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            ModdedPlayer clone = (ModdedPlayer)targetCopy;
            clone.gitGud = gitGud;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ModdedPlayer clone = (ModdedPlayer)clientPlayer;
            if (gitGud != clone.gitGud) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}