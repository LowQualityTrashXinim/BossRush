using BossRush.Contents.Items.Spawner;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest
{
    class MechTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Right click to open chest");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 7;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber()
        {
            List<int> list = new List<int>() { 7, 8, 9 };
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                list.RemoveAt(1);
                list.RemoveAt(0);
                list.Add(10);
                list.Add(11);
            }
            return list;
        }
        public override List<int> FlagNumAcc() => new List<int> { 8, 9, 10 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.ButterflyWings, ItemID.FlameWings, ItemID.FrozenWings, ItemID.SteampunkWings, ItemID.Jetpack });
            player.QuickSpawnItem(entitySource, wing);
            GetAmount(out int amount, out int amount2, out int amount3, player);
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                int randChooser = Main.rand.Next(4);
                switch (randChooser)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteHeadgear);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteMask);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophytePlateMail);
                        player.QuickSpawnItem(entitySource, ItemID.ChlorophyteGreaves);
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount, RNGManage(25, 25, 25, 25, 0));
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 2; i++)
            {
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                player.QuickSpawnItem(entitySource, ItemID.ChlorophytePickaxe);
            }
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
            }
            player.QuickSpawnItem(entitySource, ModContent.ItemType<PlanteraEssence>());
            player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
        }
    }
}
