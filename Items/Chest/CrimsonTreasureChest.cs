using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Items.ExtraItem;

namespace BossRush.Items.Chest
{
    class CrimsonTreasureChest : ChestLootDrop
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
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2 };
        public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2, 3, 4, 5 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
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
            GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player, out int ReturnWeapon, out int SpecialAmount);
                AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
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
