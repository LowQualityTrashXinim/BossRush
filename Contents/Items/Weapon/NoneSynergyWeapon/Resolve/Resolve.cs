using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.NoneSynergyWeapon.Resolve
{
    internal class Resolve : SynergyModItem, ISynergyItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(1, 1, 20, 7f, 20, 20, ItemUseStyleID.Shoot, true);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("OreShortSword")
                .AddRecipeGroup("OreBroadSword")
                .AddRecipeGroup("OreBow")
                .Register();
            base.AddRecipes();
        }
    }
    class ResolveProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
    }
}
