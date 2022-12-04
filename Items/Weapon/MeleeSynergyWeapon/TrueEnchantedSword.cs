using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class TrueEnchantedSword : ModItem, ISynergyItem
    {
        int count = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The long await magical younger brother of StarWrath and True sword family");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 100;

            Item.damage = 250;
            Item.crit = 30;
            Item.knockBack = 7f;
            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 20;

            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 6;
            Item.value = Item.buyPrice(platinum: 5);

            Item.UseSound = SoundID.Item1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            count++;
            float num = 10f;
            float rotation = MathHelper.ToRadians(10);
            for (int i = 0; i < 6; i++)
            {
                Vector2 SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y - 800 + Main.rand.Next(-300, 100));
                Vector2 Aimto = Main.MouseWorld - SkyPos;
                Vector2 safeAim = Aimto.SafeNormalize(Vector2.UnitX);
                if (i != 5)
                {
                    Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.Starfury, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.SuperStar, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 NewPos = position + Main.rand.NextVector2Circular(100f, 100f);
                Vector2 Aimto2 = Main.MouseWorld - NewPos;
                Vector2 safeAim = Aimto2.SafeNormalize(Vector2.UnitX);
                if (i != 5)
                {
                    Projectile.NewProjectile(source, NewPos, safeAim * 10f, ProjectileID.EnchantedBeam, (int)(damage * 0.75), knockback, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(source, NewPos, safeAim * 14f, ProjectileID.SuperStar, (int)(damage * 0.75), knockback, player.whoAmI);
                }
            }
            if (count == 5)
            {
                Projectile.NewProjectile(source, position, velocity * 0.3f, ModContent.ProjectileType<TrueEnchantedSwordBeam>(), damage * 2, knockback, player.whoAmI);
                count = 0;
            }
            for (int i = 0; i < num; i++)
            {
                Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
                Projectile.NewProjectile(source, position, rotate * 0.6f, ProjectileID.IceBolt, (int)(damage * 0.9), knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ModContent.ItemType<FrozenEnchantedSword>())
                .AddIngredient(ModContent.ItemType<EnchantedStarfury>())
                .AddIngredient(ModContent.ItemType<EnchantedLongSword>())
                .AddIngredient(ItemID.SuperStarCannon)
                .Register();
        }
    }
}
