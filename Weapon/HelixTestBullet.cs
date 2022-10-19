using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon
{
    class HelixTestBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Very weird worm bullet");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 150000;
            Item.knockBack = 1.5f;
            Item.shootSpeed = 1f;
            Item.height = 30;
            Item.width = 100;

            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = 5;
            Item.value = 1000000;
            Item.rare = 4;

            Item.useTime = 10;
            Item.shoot = ModContent.ProjectileType<HelixBullet>();
            Item.useAnimation = 10;
            Item.reuseDelay = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile( source, position, velocity, ModContent.ProjectileType<HelixBullet>(), damage, knockback,player.whoAmI); 
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shotgun)
                .AddIngredient(ItemID.ClockworkAssaultRifle)
                .Register();
        }
    }
}
