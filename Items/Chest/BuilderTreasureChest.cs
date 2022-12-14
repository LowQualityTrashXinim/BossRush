using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Chest
{
    internal class BuilderTreasureChest : ModItem
    {
        public override string Texture => "BossRush/Items/Chest/PlaceHolderTreasureChest";
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
            player.QuickSpawnItem(entitySource, ItemID.WoodPlatform, 300);
            player.QuickSpawnItem(entitySource, ItemID.BuilderPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.ArchitectGizmoPack);
            player.QuickSpawnItem(entitySource, ItemID.Toolbox);
            player.QuickSpawnItem(entitySource, ItemID.Chest, 15);
        }
    }
}
