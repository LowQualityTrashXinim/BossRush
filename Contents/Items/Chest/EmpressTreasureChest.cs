using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EmpressDelight;

namespace BossRush.Contents.Items.Chest
{
    internal class EmpressTreasureChest : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 37;
            Item.height = 35;
            Item.rare = 10;
        }
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<EmpressDelight>(), 1);
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount, RNGManage(player,25, 25, 25, 25, 0));
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
