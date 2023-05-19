using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Texture;

namespace BossRush.Contents.Items.Chest
{
    internal class UnderworldTreasureChest : ChestLootDrop
    {
        public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 0;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber()
        {
            return new List<int> { 0 };
        }
        public override List<int> FlagNumAcc() => new List<int>() { 0, 1, 2, 3 };
        public override void OnRightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            GetAmount(out int weaponAmount, out int potionTypeAmount, out int potionNum, player);
            for (int i = 0; i < weaponAmount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
            }
            for (int i = 0; i < potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), potionNum);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
        }
    }

}
