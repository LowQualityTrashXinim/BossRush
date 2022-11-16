using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.Deagle
{
    internal class Deagle : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Quite a killer if you can aim");
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 30;

            Item.damage = 70;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.knockBack = 2;

            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.noMelee = true;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10;

            Item.rare = 3;
            Item.value = Item.sellPrice(silver: 1000);
            Item.scale -= 0.25f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vec2ToRotate = velocity;
            if (player.velocity != Vector2.Zero)
            {
                velocity = RotateRandom(120);
            }
            else
            {
                velocity *= 1.5f;
                damage = (int)(damage * 1.5f);
                knockback *= 2f;
                if (type == ProjectileID.Bullet)
                {
                    type = ProjectileID.BulletHighVelocity;
                }
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 2);
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (player.velocity == Vector2.Zero)
            {
                crit += 55f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.Musket)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
