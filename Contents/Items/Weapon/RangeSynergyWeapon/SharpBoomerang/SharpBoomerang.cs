using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SharpBoomerang
{
    internal class SharpBoomerang : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(38, 72, 14, 5f, 35, 35, ItemUseStyleID.Swing, ModContent.ProjectileType<SharpBoomerangP>(), 40, false);
            Item.crit = 6;
            Item.scale = 0.75f;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(platinum: 5);
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<SharpBoomerangP>()] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenBoomerang)
                .AddRecipeGroup("OreShortSword")
                .Register();
        }
    }
}
