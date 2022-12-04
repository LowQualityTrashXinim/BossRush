using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class SapphireSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("so it is just a better staff but with a sword as tip ?\\\\Todo : make alt attack");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 20;
            Item.crit = 10;
            Item.knockBack = 3f;
            Item.useTime = 2;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.shoot = ProjectileID.SapphireBolt;
            Item.shootSpeed = 7;
            Item.mana = 10;

            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item8;
        }
        int i = 0;
        int counterChooser = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float rotation = MathHelper.ToRadians(10);
            Vector2 Rotate;
            Vector2 Rotate2;
            if (counterChooser == 0)
            {
                Rotate = velocity.RotatedBy(MathHelper.Lerp(0, -rotation, i / 9f));
                Rotate2 = velocity.RotatedBy(MathHelper.Lerp(0, rotation, i / 9f));
                Projectile.NewProjectile(source, position, Rotate, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, Rotate2, type, damage, knockback, player.whoAmI);
            }
            else
            {
                Rotate = velocity.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / 9f));
                Rotate2 = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 9f));
                Projectile.NewProjectile(source, position, Rotate * (i * 0.1f + 1), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, Rotate2 * (i * 0.1f + 1), type, damage, knockback, player.whoAmI);
            }
            i++;
            if (i > 4)
            {
                i = 0;
                counterChooser++;
                if (counterChooser > 1)
                {
                    counterChooser = 0;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.SilverBroadsword)
                .AddIngredient(ItemID.SapphireStaff)
                .Register();
        }
    }
}
