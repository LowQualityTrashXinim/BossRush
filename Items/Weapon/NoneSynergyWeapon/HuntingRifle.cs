using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.NoneSynergyWeapon
{
    internal class HuntingRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("meant for killing small bird");
        }

        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 22;

            Item.damage = 25;
            Item.crit = 10;
            Item.knockBack = 4f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shootSpeed = 17;

            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 60f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int i = 0; i < 7; i++)
            {
                Vector2 rotateRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, position, rotateRan, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }
    }
}
