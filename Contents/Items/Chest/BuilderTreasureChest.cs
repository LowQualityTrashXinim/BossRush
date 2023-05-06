using BossRush.Contents.Items.Accessories;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest
{
    internal class BuilderTreasureChest : ModItem
    {
        public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Gray;
        }
        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            player.QuickSpawnItem(entitySource, ItemID.Rope, 100);
            player.QuickSpawnItem(entitySource, ItemID.Torch, 999);
            player.QuickSpawnItem(entitySource, ItemID.WoodPlatform, 300);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<SuperBuilderTool>());
            player.QuickSpawnItem(entitySource, ItemID.Chest, 15);
        }
    }
}
