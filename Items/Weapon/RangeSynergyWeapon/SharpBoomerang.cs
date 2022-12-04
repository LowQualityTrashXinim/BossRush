using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon
{
    internal class SharpBoomerang : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("remarkable");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 72;

            Item.damage = 30;
            Item.knockBack = 5f;
            Item.crit = 6;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.buyPrice(platinum: 5);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.rare = 3;
            Item.scale = 0.75f;

            Item.shoot = ModContent.ProjectileType<SharpBoomerangP>();
            Item.shootSpeed = 60;
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
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
