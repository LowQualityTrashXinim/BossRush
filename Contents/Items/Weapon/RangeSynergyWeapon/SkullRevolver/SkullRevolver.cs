using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver
{
    internal class SkullRevolver : ModItem, ISynergyItem
    {
        int counter = 0;
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(26, 52, 45, 3f, 2, 12, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, false, AmmoID.Bullet);
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;
            Item.crit = 15;
            Item.reuseDelay = 37;
            Item.value = Item.buyPrice(gold: 50);
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
            .AddIngredient(ItemID.Revolver)
            .AddIngredient(ItemID.BookofSkulls)
            .Register();
        }
    }
}
