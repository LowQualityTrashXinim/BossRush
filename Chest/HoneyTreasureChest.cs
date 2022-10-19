using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace BossRush.Chest
{
    class HoneyTreasureChest : ModItem
    { 
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good luck !");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 4;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            for (int i = 0; i < 3; i++)
            {
                switch (Main.rand.Next(30))
                {
                    case 0:
                        player.QuickSpawnItem(entitySource,ItemID.BeeCloak);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource,ModContent.ItemType<HoneyTreasureChest>());
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource,ItemID.HoneyedGoggles);
                        break;
                    case 5:
                        player.QuickSpawnItem(entitySource,ItemID.QueenBeeBossBag);
                        break;
                    case 10:
                        player.QuickSpawnItem(entitySource,ItemID.HoneyBalloon);
                        break;
                    case 15:
                        player.QuickSpawnItem(entitySource,ItemID.SweetheartNecklace);
                        break;
                    case 19:
                        player.QuickSpawnItem(entitySource,ItemID.WaspGun);
                        break;
                }
            }
            ChestLootDrop HoneyCHest = new ChestLootDrop(player);
            if (NPC.downedBoss3)
            {
                for (int i = 0; i < 10; i++)
                {
                    HoneyCHest.GetWeapon(out int weapon, out int specialAmount);
                    HoneyCHest.AmmoForWeapon(out int ammo, out int num, weapon);
                    player.QuickSpawnItem(entitySource, weapon, specialAmount);
                    player.QuickSpawnItem(entitySource, ammo, num);
                }
            }
            player.QuickSpawnItem(entitySource, ItemID.Honeyfin, 10);
            HoneyCHest.GetPotion(out int potion);
            player.QuickSpawnItem(entitySource, potion, 3);
            player.QuickSpawnItem(entitySource,ItemID.ManaCrystal);
        }
    }
}
