using Terraria;
using Terraria.ModLoader;
using BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword;
using BossRush.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark;
using BossRush.Items.Weapon.RangeSynergyWeapon.NatureSelection;
using BossRush.Items.Accessories.EnragedBossAccessories.EmpressDelight;
using BossRush.Items.Weapon.MeleeSynergyWeapon;

namespace BossRush.Items.Chest
{
    internal class EmpressTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You certainly prove yourself to be something, here a gift");
        }
        public override void SetDefaults()
        {
            Item.width = 37;
            Item.height = 35;
            Item.rare = 10;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);

            player.QuickSpawnItem(entitySource, ModContent.ItemType<EmpressDelight>(), 1);
            int SynergyWeapon = Main.rand.Next(new int[] { ModContent.ItemType<SuperShortSword>(), ModContent.ItemType<NatureSelection>(), ModContent.ItemType<TrueEnchantedSword>(), ModContent.ItemType<ChaosMiniShark>() });
            player.QuickSpawnItem(entitySource, SynergyWeapon, 1);
        }
    }
}
