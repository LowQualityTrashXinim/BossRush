using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.ExtraItem;

namespace BossRush.Chest
{
    class CrimsonTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 3;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            ChestLootDrop IceChest = new ChestLootDrop(player);
            switch (Main.rand.Next(5))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.CrimsonHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.CrimsonScalemail);
                    player.QuickSpawnItem(entitySource, ItemID.CrimsonGreaves);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
                    player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.FossilHelm);
                    player.QuickSpawnItem(entitySource, ItemID.FossilShirt);
                    player.QuickSpawnItem(entitySource, ItemID.FossilPants);
                    break;
                case 3:
                    player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource, ItemID.ObsidianHelm);
                    player.QuickSpawnItem(entitySource, ItemID.ObsidianShirt);
                    player.QuickSpawnItem(entitySource, ItemID.ObsidianPants);
                    break;
            }
            ChestLootDrop EvilChest = new ChestLootDrop(player);
            IceChest.GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                IceChest.GetWeapon(out int ReturnWeapon, out int SpecialAmount);
                IceChest.AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            IceChest.GetAccessory(out int Accessory);
            player.QuickSpawnItem(entitySource, Accessory);
            for (int i = 0; i < amount2; i++)
            {
                IceChest.GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, amount3);
            }
            player.QuickSpawnItem(entitySource, ItemID.TinkerersWorkshop);
            player.QuickSpawnItem(entitySource, ItemID.Hellforge);
            player.QuickSpawnItem(entitySource, ItemID.IronAnvil);
            player.QuickSpawnItem(entitySource, ItemID.WoodPlatform, 999);
            player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystalStand);
            player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal, 99);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<PocketPortal>());
        }

    }
}
