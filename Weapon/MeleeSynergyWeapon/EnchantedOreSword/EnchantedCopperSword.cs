using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedCopperSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.height = 70;
            Item.width = 24;

            Item.useTime = 40;
            Item.useAnimation = 20;

            Item.damage = 30;
            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<EnchantedCopperSwordP>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.CopperShortsword)
                .AddIngredient(ItemID.CopperBroadsword)
                .Register();
        }
    }
}
