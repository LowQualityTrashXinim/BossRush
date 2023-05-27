using Terraria;

namespace BossRush.Contents.Items.Chest
{
    internal class DukeTreasureChest : ChestLootDrop
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.rare = 10;
        }
        public override void OnRightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            GetAmount(out int amount, out int _, out int _, player);
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount, RNGManage(25, 25, 25, 25, 0));
                AmmoForWeapon(out int ammo, out int num, weapon, 3.5f);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 3; i++)
            {
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
        }
    }
}