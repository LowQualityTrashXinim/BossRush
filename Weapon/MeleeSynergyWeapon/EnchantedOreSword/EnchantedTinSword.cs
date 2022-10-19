using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedTinSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = 1;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 19;
            Item.useAnimation = 19;

            Item.damage = 17;
            Item.knockBack = 4.1f;

            Item.shoot = ModContent.ProjectileType<EnchantedTinSwordP>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.TinShortsword)
                .AddIngredient(ItemID.TinBroadsword)
                .Register();
        }
    }
}
