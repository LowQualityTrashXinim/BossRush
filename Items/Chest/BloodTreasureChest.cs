using Terraria;
using System.Collections.Generic;

namespace BossRush.Items.Chest
{
    internal class BloodTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.rare = 6;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            GetAmount(out int Amount1, out int Amount2, out int Amount3, player);
            for (int i = 0; i < Amount1; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < Amount2; i++)
            {
                GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, Amount3);
            }
            GetAccessory(out int Accessories);
            player.QuickSpawnItem(entitySource, Accessories);
        }
    }
}
