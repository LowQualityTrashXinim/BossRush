using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;

namespace BossRush.Contents.Items.Chest
{
    class BlackTreasureChest : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 11;
        }

        public override void PostModifyTooltips(ref List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "SynergyOnly", "Is limited to synergy mode only"));
        }

        public override bool CanBeRightClick() => ModContent.GetInstance<BossRushModConfig>().SynergyMode;
        

        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if (NPC.downedEmpressOfLight)
            {
                int ran1 = Main.rand.Next(10);
                switch (ran1)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.EmpressBlade);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.FairyQueenBossBag);
                        break;
                }
            }
            if (NPC.downedFishron)
            {
                int ran1 = Main.rand.Next(10);
                switch (ran1)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ModContent.ItemType<DukeTreasureChest>());
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.FishronBossBag);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.FishronWings);
                        break;
                }
            }
            if (NPC.downedMoonlord)
            {
                int ran1 = Main.rand.Next(10);
                switch (ran1)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ModContent.ItemType<MoonTreasureChest>());
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.MoonLordBossBag);
                        break;
                }
            }
            if (Main.rand.NextBool(25) && ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<UltimatePistol>());
            }
            switch (Main.rand.Next(11))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<WoodenLootBox>());
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<IronLootBox>());
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverLootBox>());
                    break;
                case 3:
                    if (WorldGen.crimson)
                    {
                        player.QuickSpawnItem(entitySource, ModContent.ItemType<CrimsonLootBox>());
                    }
                    else
                    {
                        player.QuickSpawnItem(entitySource, ModContent.ItemType<CorruptionLootBox>());
                    }
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldLootBox>());
                    break;
                case 5:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<HoneyTreasureChest>());
                    break;
                case 6:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<ShadowTreasureChest>());
                    break;
                case 7:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<CrystalTreasureChest>());
                    break;
                case 8:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<MechTreasureChest>());
                    break;
                case 9:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<NatureTreasureChest>());
                    break;
                case 10:
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<LihzahrdTreasureChest>());
                    break;
            }

        }
    }
}