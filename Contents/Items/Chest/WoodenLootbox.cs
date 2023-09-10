﻿using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Potion;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Chest
{
    class WoodenLootBox : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 0;
        }
        public override List<int> FlagNumber() => new List<int> { 0, 1, 2 };

        public override List<int> FlagNumAcc() => new List<int> {  2 };
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int ReturnWeapon, out int SpecialAmount);
                AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < modplayer.potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(true), modplayer.potionNumAmount);
            }
            if (player.IsDebugPlayer())
            {
                GetArmorForPlayer(entitySource, player);
            }
            else
            {
                int RandomNumber = Main.rand.Next(8);
                switch (RandomNumber)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.WoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.WoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.WoodGreaves);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.BorealWoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.BorealWoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.BorealWoodGreaves);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.RichMahoganyHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.RichMahoganyBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.RichMahoganyGreaves);
                        break;
                    case 3:
                        player.QuickSpawnItem(entitySource, ItemID.EbonwoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.EbonwoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.EbonwoodGreaves);
                        break;
                    case 4:
                        player.QuickSpawnItem(entitySource, ItemID.PalmWoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.PalmWoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.PalmWoodGreaves);
                        break;
                    case 5:
                        player.QuickSpawnItem(entitySource, ItemID.ShadewoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.ShadewoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.ShadewoodGreaves);
                        break;
                    case 6:
                        player.QuickSpawnItem(entitySource, ItemID.CactusHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.CactusBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.CactusLeggings);
                        break;
                    case 7:
                        player.QuickSpawnItem(entitySource, ItemID.AshWoodHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.AshWoodBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.AshWoodGreaves);
                        break;
                }
                int RandomAssArmor = Main.rand.Next(new int[] { ItemID.FlinxFurCoat, ItemID.VikingHelmet, ItemID.EmptyBucket, ItemID.NightVisionHelmet, ItemID.DivingHelmet, ItemID.Goggles, ItemID.Gi });
                player.QuickSpawnItem(entitySource, RandomAssArmor);
            }
            if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                int RandomModdedBuff = Main.rand.Next(new int[] {
                    ModContent.ItemType<BerserkerElixir>(),
                    ModContent.ItemType<GunslingerElixir>(),
                    ModContent.ItemType<SageElixir>(),
                    ModContent.ItemType<CommanderElixir>(),
                    ModContent.ItemType<TitanElixir>() });
                player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
            }
            player.QuickSpawnItem(entitySource, ItemID.SlimeCrown);
            player.QuickSpawnItem(entitySource, ItemID.GrapplingHook);
            player.QuickSpawnItem(entitySource, ItemID.LesserHealingPotion, 5);
            player.QuickSpawnItem(entitySource, ItemID.MagicConch);
            player.QuickSpawnItem(entitySource, ItemID.MagicMirror);
            player.QuickSpawnItem(entitySource, ItemID.DemonConch);
        }
    }
}