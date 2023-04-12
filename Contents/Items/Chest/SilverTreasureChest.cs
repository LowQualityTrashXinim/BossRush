using Terraria;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Chest
{
    class SilverTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 2;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber() => new List<int> { 0, 1, 2, 3 };
        public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int RandomNumber = Main.rand.Next(6);
            switch (RandomNumber)
            {
                case 0:
                    int RandomAssArmor = Main.rand.Next(new int[] { ItemID.MagicHat, ItemID.WizardHat });
                    if (RandomAssArmor == ItemID.WizardHat || RandomAssArmor == ItemID.MagicHat)
                    {
                        player.QuickSpawnItem(entitySource, RandomAssArmor);
                        int RobeWiz = Main.rand.Next(new int[] { ItemID.AmethystRobe, ItemID.DiamondRobe, ItemID.RubyRobe, ItemID.SapphireRobe, ItemID.EmeraldRobe, ItemID.TopazRobe, ItemID.GypsyRobe });
                        player.QuickSpawnItem(entitySource, RobeWiz);
                    }
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.SilverHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.SilverChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.SilverGreaves);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.TungstenHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.TungstenChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.TungstenGreaves);
                    break;
                case 3:
                    player.QuickSpawnItem(entitySource, ItemID.GoldHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.GoldChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.GoldGreaves);
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource, ItemID.PlatinumHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.PlatinumChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.PlatinumGreaves);
                    break;
                case 5:
                    player.QuickSpawnItem(entitySource, ItemID.PumpkinHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.PumpkinBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.PumpkinLeggings);
                    break;
            }
            GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player, out int weapon, out int specialamount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialamount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
            }
            player.QuickSpawnItem(entitySource, ItemID.WoodPlatform, 999);
            player.QuickSpawnItem(entitySource, ItemID.StickyDynamite, 50);
            player.QuickSpawnItem(entitySource, ItemID.Campfire, 10);
            player.QuickSpawnItem(entitySource, ItemID.MagicConch);
            player.QuickSpawnItem(entitySource, ItemID.MagicMirror);
            player.QuickSpawnItem(entitySource, ItemID.MoneyTrough);
        }
    }
}

