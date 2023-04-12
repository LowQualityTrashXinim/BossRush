using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items
{
    internal class PowerEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Beyond what we could imagine");
        }
        public override void SetDefaults()
        {
            Item.rare = 10;
            Item.width = 54;
            Item.height = 20;
            Item.material = true;
        }
    }
}
