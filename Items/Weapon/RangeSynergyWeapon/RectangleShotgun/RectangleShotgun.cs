using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
{
    class RectangleShotgun : WeaponTemplate, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("is it a shotgun or a rifle ?");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.knockBack = 4f;
            Item.height = 12;
            Item.width = 74;

            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = 4;

            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<SquareBullet>();
            Item.shootSpeed = 30f;
            Item.reuseDelay = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item38;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vec2ToRotate = velocity;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            NumOfProjectile = 5;
            for (int i = 0; i < NumOfProjectile; i++)
            {
                Vector2 Rotate = RotateCode(30, i);
                Projectile.NewProjectile(source, position, Rotate, ModContent.ProjectileType<SquareBullet>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
            .AddIngredient(ItemID.Boomstick, 2)
            .Register();
        }
    }
}
