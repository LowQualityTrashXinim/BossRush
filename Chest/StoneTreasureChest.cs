using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace BossRush.Chest
{
    class StoneTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 1;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int RandomNumber = Main.rand.Next(6);
            switch (RandomNumber)
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.CopperHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.CopperChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.CopperGreaves);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.TinHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.TinChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.TinGreaves);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.IronHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.IronChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.IronGreaves);
                    break;
                case 3:
                    player.QuickSpawnItem(entitySource, ItemID.LeadHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.LeadChainmail);
                    player.QuickSpawnItem(entitySource, ItemID.LeadGreaves);
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource, ItemID.NinjaHood);
                    player.QuickSpawnItem(entitySource, ItemID.NinjaPants);
                    player.QuickSpawnItem(entitySource, ItemID.NinjaShirt);
                    break;
                case 5:
                    player.QuickSpawnItem(entitySource, ItemID.JungleHat);
                    player.QuickSpawnItem(entitySource, ItemID.JungleShirt);
                    player.QuickSpawnItem(entitySource, ItemID.JunglePants);
                    break;
            }
            ModContent.GetInstance<ChestLootDrop>().GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                ModContent.GetInstance<ChestLootDrop>().GetWeapon(out int ReturnWeapon, out int SpecialAmount);
                ModContent.GetInstance<ChestLootDrop>().AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            ModContent.GetInstance<ChestLootDrop>().GetAccessory(out int Accessory);
            player.QuickSpawnItem(entitySource, Accessory);
            for (int i = 0; i < amount2; i++)
            {
                ModContent.GetInstance<ChestLootDrop>().GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, amount3);
            }
            if(Main.rand.NextBool(5))
            {
                player.QuickSpawnItem(entitySource, ItemID.KingSlimeBossBag);
            }
            player.QuickSpawnItem(entitySource, ItemID.WoodPlatform, 999);
            player.QuickSpawnItem(entitySource, ItemID.Campfire, 10);
            player.QuickSpawnItem(entitySource, ItemID.PlatinumPickaxe);
            player.QuickSpawnItem(entitySource, ItemID.PlatinumAxe);
        }
    }
}

