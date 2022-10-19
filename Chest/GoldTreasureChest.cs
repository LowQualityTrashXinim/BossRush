using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    class GoldTreasureChest : ModItem
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
                    player.QuickSpawnItem(entitySource,ItemID.NecroHelmet);
                    player.QuickSpawnItem(entitySource,ItemID.NecroBreastplate);
                    player.QuickSpawnItem(entitySource,ItemID.NecroGreaves);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource,ItemID.MeteorHelmet);
                    player.QuickSpawnItem(entitySource,ItemID.MeteorSuit);
                    player.QuickSpawnItem(entitySource,ItemID.MeteorLeggings);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource,ItemID.MoltenHelmet);
                    player.QuickSpawnItem(entitySource,ItemID.MoltenBreastplate);
                    player.QuickSpawnItem(entitySource,ItemID.MoltenGreaves);
                    break;
            }
            ChestLootDrop GoldChest = new ChestLootDrop(player);
            GoldChest.GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                GoldChest.GetWeapon(out int weapon, out int specialAmount);
                GoldChest.AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            GoldChest.GetAccessory(out int Accessory);
            player.QuickSpawnItem(entitySource, Accessory);
            for (int i = 0; i < amount2; i++)
            {
                GoldChest.GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, amount3);
            }
            switch (Main.rand.Next(2))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.ObsidianPlatform, 2997);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.WaterWalkingPotion, 10);
                    player.QuickSpawnItem(entitySource, ItemID.FeatherfallPotion, 10);
                    player.QuickSpawnItem(entitySource, ItemID.GravitationPotion, 10);
                    break;
            }
            player.QuickSpawnItem(entitySource,ItemID.LifeCrystal, 5);
            player.QuickSpawnItem(entitySource,ItemID.ManaCrystal, 5);
            player.QuickSpawnItem(entitySource,ItemID.StickyDynamite, 200);
            player.QuickSpawnItem(entitySource,ItemID.CalmingPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.DemonConch);
        }
    }
}
