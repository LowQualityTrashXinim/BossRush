using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang
{
    internal class Bowmarang : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(32, 64, 15, 3f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<BowmarangP>(), 20f, false);
            Item.crit = 10;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BowmarangP>()] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenBoomerang)
                .AddRecipeGroup("Wood Bow")
                .Register();
        }
    }
}
