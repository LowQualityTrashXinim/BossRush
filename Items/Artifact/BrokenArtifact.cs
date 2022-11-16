using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Artifact
{
    internal class BrokenArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A far cry from it's former glory, maybe you can make something with this");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 0;
            Item.material = true;
        }
    }
}
