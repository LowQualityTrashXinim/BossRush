using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories
{
    internal class SuperBuilderTool : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hope this will make your building much quicker");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.vanity = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.tileSpeed += 10;
            player.pickSpeed *= .1f;
            player.blockRange += 10;
            player.wallSpeed += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BuilderPotion)
                .AddIngredient(ItemID.Toolbox)
                .AddIngredient(ItemID.ArchitectGizmoPack)
                .Register();
        }
    }
}
