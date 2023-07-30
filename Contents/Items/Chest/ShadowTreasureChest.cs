using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using BossRush.Common;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Artifact;
using System.Security.Cryptography.X509Certificates;

namespace BossRush.Contents.Items.Chest
{
    class ShadowTreasureChest : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 6;
        }
        public override List<int> FlagNumber() => new List<int>() { 7, 8 };
        public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
            player.QuickSpawnItem(entitySource, wing);
            int RandomNumber = Main.rand.Next(7);
            int Random2 = Main.rand.Next(3);
            switch (RandomNumber)
            {
                case 0:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.CobaltHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.CobaltHat);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.CobaltMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.CobaltBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.CobaltLeggings);
                    break;
                case 1:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.MythrilHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.MythrilHat);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.MythrilHood);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.MythrilChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.MythrilGreaves);
                    break;
                case 2:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.AdamantiteHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.AdamantiteHeadgear);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.AdamantiteMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.AdamantiteBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.AdamantiteLeggings);
                    break;
                case 3:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.PalladiumHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.PalladiumHeadgear);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.PalladiumMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.PalladiumBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.PalladiumLeggings);
                    break;
                case 4:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.OrichalcumHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.OrichalcumHeadgear);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.OrichalcumMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.OrichalcumBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.OrichalcumLeggings);
                    break;
                case 5:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource, ItemID.TitaniumHelmet);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource, ItemID.TitaniumHeadgear);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource, ItemID.TitaniumMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource, ItemID.TitaniumBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.TitaniumLeggings);
                    break;
                case 6:
                    player.QuickSpawnItem(entitySource, ItemID.SpiderMask);
                    player.QuickSpawnItem(entitySource, ItemID.SpiderBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.SpiderGreaves);
                    break;
            }
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 2; i++)
            {
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
            for (int i = 0; i < modplayer.potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
            }
            player.QuickSpawnItem(entitySource, ItemID.MythrilAnvil);
            player.QuickSpawnItem(entitySource, ItemID.AdamantiteForge);
            int ranPix = Main.rand.Next(2);
            switch (ranPix)
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.AdamantitePickaxe);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.TitaniumPickaxe);
                    break;
            }
            if (Main.rand.NextBool(20))
            {
                player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
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
                player.QuickSpawnItem(entitySource, ModContent.ItemType<BrokenArtifact>(), 1);
                player.QuickSpawnItem(entitySource, ModContent.ItemType<ArtifactRemover>(), 1);
            }
        }
    }
}