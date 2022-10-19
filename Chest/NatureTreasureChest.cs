using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    class NatureTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 9;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.BoneWings,ItemID.BatWings,ItemID.MothronWings,ItemID.ButterflyWings,ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings,ItemID.FestiveWings,ItemID.SpookyWings,ItemID.TatteredFairyWings });
            player.QuickSpawnItem(entitySource, wing);
            int RandomNumber = Main.rand.Next(5); int Random2 = Main.rand.Next(3);
            switch (RandomNumber)
            {
                case 0:
                    player.QuickSpawnItem(entitySource,ItemID.TurtleHelmet);
                    player.QuickSpawnItem(entitySource,ItemID.TurtleScaleMail);
                    player.QuickSpawnItem(entitySource,ItemID.TurtleLeggings);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource,ItemID.SpookyHelmet);
                    player.QuickSpawnItem(entitySource,ItemID.SpookyBreastplate);
                    player.QuickSpawnItem(entitySource,ItemID.SpookyLeggings);
                    break;
                case 2:
                    switch (Random2)
                    {
                        case 0:
                            player.QuickSpawnItem(entitySource,ItemID.ShroomiteHeadgear);
                            break;
                        case 1:
                            player.QuickSpawnItem(entitySource,ItemID.ShroomiteHelmet);
                            break;
                        case 2:
                            player.QuickSpawnItem(entitySource,ItemID.ShroomiteMask);
                            break;
                    }
                    player.QuickSpawnItem(entitySource,ItemID.ShroomiteBreastplate);
                    player.QuickSpawnItem(entitySource,ItemID.ShroomiteLeggings);
                    break;
                case 3:
                    player.QuickSpawnItem(entitySource,ItemID.SpectreHood);
                    player.QuickSpawnItem(entitySource,ItemID.SpectreRobe);
                    player.QuickSpawnItem(entitySource,ItemID.SpectrePants);
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource,ItemID.SpectreMask);
                    player.QuickSpawnItem(entitySource,ItemID.SpectreRobe);
                    player.QuickSpawnItem(entitySource,ItemID.SpectrePants);
                    break;
            }
            ChestLootDrop NatureChest = new ChestLootDrop(player);
            NatureChest.GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                NatureChest.GetWeapon(out int weapon, out int specialAmount, true, NatureChest.RNGManage(25, 25, 25, 25, 0));
                NatureChest.AmmoForWeapon(out int ammo, out int num, weapon, 2.5f);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 3; i++)
            {
                NatureChest.GetAccessory(out int Accessory, true, true, true, false);
                player.QuickSpawnItem(entitySource, Accessory);
            }
            for (int i = 0; i < amount2; i++)
            {
                NatureChest.GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, amount3);
            }
            player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
            player.QuickSpawnItem(entitySource, ItemID.NaughtyPresent);
            player.QuickSpawnItem(entitySource, ItemID.PumpkinMoonMedallion);
        }
    }
}
