using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Artifact
{
    internal class BrokenArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A far cry from it's former glory, maybe you can make something with this");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
