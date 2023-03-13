using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Items.Chest
{
    class CrystalTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest to get the following items\n1 Set of better random pre-mech armor\n2 Random accessory and a random wing\n5 random pre-mech weapons\n10 of 5 Random Buff Potions\nrare chance getting slime queen treasure bag\nGood Luck fighting mech!");
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
        public override List<int> FlagNumber() => new List<int>() { 4, 7, 8, 9 };
        public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
            player.QuickSpawnItem(entitySource, wing);
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.FrostHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.FrostBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.FrostLeggings);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorHat);
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorShirt);
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorPants);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaChestplate);
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaLeggings);
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
            for (int i = 0; i < 4; i++)
            {
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), amount3);
            }
            if (Main.rand.NextBool(5))
            {
                player.QuickSpawnItem(entitySource, ItemID.QueenSlimeBossBag);
            }
            if (Main.rand.NextBool(20))
            {
                player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
            }
        }

    }
}
