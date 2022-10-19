using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedSilverSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = 1;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 16;
            Item.useAnimation = 16;

            Item.damage = 25;
            Item.knockBack = 6f;

            Item.shoot = ModContent.ProjectileType<EnchantedSilverSwordP>();
            Item.shootSpeed = 15f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y - 700);
            Vector2 AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
            Projectile.NewProjectile(source, Above, AimTo, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.SilverShortsword)
                .AddIngredient(ItemID.SilverBroadsword)
                .Register();
        }
    }
}
