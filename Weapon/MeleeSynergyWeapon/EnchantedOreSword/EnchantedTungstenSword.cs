using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedTungstenSword : ModItem
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
            Item.knockBack = 6.1f;

            Item.shoot = ModContent.ProjectileType<EnchantedTungstenSwordP>();
            Item.shootSpeed = 15f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 Below = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y + 700);
            Vector2 AimTo = (Main.MouseWorld - Below).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
            Projectile.NewProjectile(source, Below, AimTo, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.TungstenShortsword)
                .AddIngredient(ItemID.TungstenBroadsword)
                .Register();
        }
    }
}
