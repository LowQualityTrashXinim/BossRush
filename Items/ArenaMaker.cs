using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items
{
    internal class ArenaMaker : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Create a basic arena that 300 block wide" +
                "\nWill spawn at the middle of the mouse");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            
            Item.useTime = 99;
            Item.useAnimation = 99;

            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<ArenaMakerProj>();
            Item.shootSpeed = 0;

            Item.consumable = true;
            Item.maxStack = 99;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodPlatform, 300)
                .Register();
        }
    }
}
