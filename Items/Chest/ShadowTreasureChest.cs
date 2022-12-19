using Terraria;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Items.Chest
{
    class ShadowTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest to get the following items\n1 Set of random pre-mech armor\n2 Random accessory and a random wing\n5 random pre-mech weapons\n10 of 5 Random Buff Potions\n1 Mech bosses and QueenSlime\nHardmode anvil and forge\nGood Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 6;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
        public override List<int> FlagNumAcc() => new List<int>() {8,9,10};
        public override void RightClick(Player player)
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
            GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
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

            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
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
        }
    }
}

