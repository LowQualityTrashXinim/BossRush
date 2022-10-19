using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.RangeSynergyWeapon.Merciless
{
    internal class Merciless : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Absolute violence");
        }
        public override void SetDefaults()
        {
            Item.width = 102;
            Item.height = 26;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);

            Item.damage = 20;
            Item.crit = 10;
            Item.knockBack = 5f;

            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.reuseDelay = 30;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 8;
            Item.scale -= 0.15f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, 0);
        }
        int count = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.ExplosiveBullet;
            }
            Vector2 OffSet = Vector2.Normalize(velocity) * 60;
            if (Collision.CanHit(position, 0, 0, position + OffSet, 0, 0))
            {
                position += OffSet;
            }
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocityRotate = RandomSpread(RotateRandom(20), 10, 0.55f);
                Projectile.NewProjectile(source, position, velocityRotate, type, damage, knockback, player.whoAmI);
            }
            if (count == 0)
            {
                Projectile.NewProjectile(source, position, velocity*2.5f, ProjectileID.CannonballFriendly, damage*4, knockback, player.whoAmI);
                count++;
            }
            else 
            {
                count = 0;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Boomstick)
                .AddIngredient(ItemID.QuadBarrelShotgun)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
