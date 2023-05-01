using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang
{
    internal class Bowmarang : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Really cursed invention");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 64;

            Item.damage = 15;
            Item.crit = 10;
            Item.knockBack = 3f;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<BowmarangP>();
            Item.shootSpeed = 20f;
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
