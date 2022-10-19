using Terraria;
using Terraria.ModLoader;

namespace BossRush.Artifact
{
    internal class TokenofGreed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Greed");
            Tooltip.SetDefault("Greed lower your weapon damage inexchange for more item\n\"Greed is satified by just having, care not for weapon quality\"");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrokenToken>())
                .Register();
        }
    }
}
