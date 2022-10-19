using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedPlatinumSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = 1;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 26;
            Item.useAnimation = 13;

            Item.damage = 27;
            Item.knockBack = 7.1f;

            Item.shoot = ModContent.ProjectileType<EnchantedPlatinumSwordP>();
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.PlatinumShortsword)
                .AddIngredient(ItemID.PlatinumBroadsword)
                .Register();
        }
    }
}
