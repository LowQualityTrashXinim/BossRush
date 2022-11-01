using Terraria;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    internal class BloodTreasureChest : ModItem
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
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            ChestLootDrop IceChest = new ChestLootDrop(player);
            ModContent.GetInstance<ChestLootDrop>().GetAmount(out int Amount1, out int Amount2, out int Amount3, player);
            for (int i = 0; i < Amount1; i++)
            {
                ModContent.GetInstance<ChestLootDrop>().GetWeapon(out int weapon, out int specialAmount);
                ModContent.GetInstance<ChestLootDrop>().AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < Amount2; i++)
            {
                ModContent.GetInstance<ChestLootDrop>().GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, Amount3);
            }
            IceChest.GetAccessory(out int Accessories);
            player.QuickSpawnItem(entitySource, Accessories);
        }
    }
}
