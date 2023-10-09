using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest
{
    internal class MoonTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("You certainly prove yourself to be something, here a gift");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.rare = ItemRarityID.Red;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            player.QuickSpawnItem(entitySource, ItemID.Zenith, 1);
        }
    }
}
