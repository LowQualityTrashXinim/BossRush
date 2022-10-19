using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.MeleeSynergyWeapon.Katana
{
    internal class CopperKatana : ModItem
    {
        int counter = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("so katana, you can feel the weebo power inside of you, slowly building up, screaming to be release\ncscreaming to be unleash on a foe");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;

            Item.damage = 20;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<CopperSlash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 2;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(gold: 50);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.direction == 1)
            {
                switch (counter)
                {
                    case 0:
                        Vector2 getPos1 = new Vector2(player.Center.X - 70, player.Center.Y);
                        Vector2 aimto1 = Main.MouseWorld - getPos1;
                        Vector2 safeAim1 = aimto1.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos1, safeAim1, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                    case 1:
                        Vector2 getPos2 = new Vector2(player.Center.X - 70, player.Center.Y + 20);
                        Vector2 aimto2 = Main.MouseWorld - getPos2;
                        Vector2 safeAim2 = aimto2.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos2, safeAim2, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                    case 2:
                        Vector2 getPos3 = new Vector2(player.Center.X - 70, player.Center.Y - 20);
                        Vector2 aimto3 = Main.MouseWorld - getPos3;
                        Vector2 safeAim3 = aimto3.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos3, safeAim3, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                }
            }
            else
            {
                switch (counter)
                {
                    case 0:
                        Vector2 getPos1 = new Vector2(player.Center.X + 70, player.Center.Y);
                        Vector2 aimto1 = Main.MouseWorld - getPos1;
                        Vector2 safeAim1 = aimto1.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos1, safeAim1, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                    case 1:
                        Vector2 getPos2 = new Vector2(player.Center.X + 70, player.Center.Y + 20);
                        Vector2 aimto2 = Main.MouseWorld - getPos2;
                        Vector2 safeAim2 = aimto2.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos2, safeAim2, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                    case 2:
                        Vector2 getPos3 = new Vector2(player.Center.X + 70, player.Center.Y - 20);
                        Vector2 aimto3 = Main.MouseWorld - getPos3;
                        Vector2 safeAim3 = aimto3.SafeNormalize(Vector2.UnitX) * 5f;
                        Projectile.NewProjectile(source, getPos3, safeAim3, ModContent.ProjectileType<CopperSlash>(), damage, knockback, player.whoAmI);
                        break;
                }
            }
            if (counter > 1)
            {
                counter = -1;
            }
            counter++;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Katana)
                .AddIngredient(ItemID.CopperBroadsword)
                .Register();
        }
    }
}
