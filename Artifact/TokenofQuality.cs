using Terraria;
using Terraria.ModLoader;

namespace BossRush.Artifact
{
    internal class TokenofQuality : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Quality");
            Tooltip.SetDefault("Increase weapon damage exchange for less weapon\n\"Quality over quantity\"");
        }
        public override string Texture => "BossRush/MissingTexture";
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
