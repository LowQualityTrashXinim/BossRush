using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Deagle
{
    internal class Deagle : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(56, 30, 70, 5f, 21, 21, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
            Item.rare = 3;
            Item.value = Item.sellPrice(silver: 1000);
            Item.scale -= 0.25f;
            Item.UseSound = SoundID.Item38;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.velocity != Vector2.Zero)
            {
                velocity = velocity.RotateRandom(120);
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
                .Register();
        }
    }
}