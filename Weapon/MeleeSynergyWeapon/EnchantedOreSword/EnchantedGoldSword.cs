using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedGoldSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = 1;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 14;
            Item.useAnimation = 14;

            Item.damage = 26;
            Item.knockBack = 7f;

            Item.shoot = ModContent.ProjectileType<EnchantedGoldSwordP>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<EnchantedGoldSwordP>()] < 1)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.GoldShortsword)
                .AddIngredient(ItemID.GoldBroadsword)
                .Register();
        }
    }
}
