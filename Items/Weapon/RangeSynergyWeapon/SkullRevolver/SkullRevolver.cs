using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.SkullRevolver
{
    internal class SkullRevolver : ModItem
    {
        int counter = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("gun of 87");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.Bullet;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;

            Item.damage = 45;
            Item.crit = 15;
            Item.knockBack = 3f;
            Item.useTime = 2;
            Item.useAnimation = 12;
            Item.reuseDelay = 37;
            Item.shootSpeed = 20;
            Item.value = Item.buyPrice(gold: 50);

            Item.height = 26;
            Item.width = 52;

            Item.UseSound = SoundID.Item41;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
            counter++;
            if (counter == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
            }
            if (counter == 4)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ClothiersCurse, damage, knockback, player.whoAmI);
                counter = 0;
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
            .AddIngredient(ItemID.Revolver)
            .AddIngredient(ItemID.BookofSkulls)
            .Register();
        }
    }
}
