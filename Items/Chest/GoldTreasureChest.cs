using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Chest
{
    class GoldTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 5;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3 };
        public override List<int> FlagNumAcc()
        {
            List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 7};
            if(NPC.downedQueenBee)
            {
                list.Add(6);
            }
            return list;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if (NPC.downedQueenBee)
            {
                int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace, ItemID.WaspGun });
                player.QuickSpawnItem(entitySource, OneRareBeeItem);
            }
            int RandomNumber = Main.rand.Next(3);
            switch (RandomNumber)
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.NecroHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.NecroBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.NecroGreaves);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
                    player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
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
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
            }
            switch (Main.rand.Next(2))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.ObsidianPlatform, 3996);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.WaterWalkingPotion, 10);
                    player.QuickSpawnItem(entitySource, ItemID.FeatherfallPotion, 10);
                    player.QuickSpawnItem(entitySource, ItemID.GravitationPotion, 10);
                    break;
            }
            player.QuickSpawnItem(entitySource, ItemID.StickyDynamite, 200);
            player.QuickSpawnItem(entitySource, ItemID.CalmingPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.DemonConch);
        }
    }
}
