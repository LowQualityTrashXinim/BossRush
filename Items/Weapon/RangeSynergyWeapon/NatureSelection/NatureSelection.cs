using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.NatureSelection
{
    internal class NatureSelection : ModItem
    {
        static int counter = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mother of all bows");
        }
        public override void SetDefaults()
        {
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;

            Item.width = 32;
            Item.height = 66;

            Item.rare = 2;
            Item.value = Item.buyPrice(platinum: 5);

            Item.noMelee = true;
            Item.shootSpeed = 20f;
            Item.damage = 40;
            Item.knockBack = 3f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.UseSound = SoundID.Item5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 RotatePos = Main.rand.NextVector2Circular(75f, 75f) * 2 + position;
            Vector2 AimPos = Main.MouseWorld - RotatePos;
            Vector2 safeAim = AimPos.SafeNormalize(Vector2.UnitX) * Main.rand.Next(14, 21);

            switch (counter)
            {
                case 0:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<WoodBowP>(), damage, knockback, player.whoAmI);
                    break;
                case 1:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<BorealWoodBowP>(), damage, knockback, player.whoAmI);
                    break;
                case 2:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<RichMahoganyBowP>(), damage, knockback, player.whoAmI);
                    break;
                case 3:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<PalmWoodBowP>(), damage, knockback, player.whoAmI);
                    break;
                case 4:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<EbonwoodBowP>(), damage, knockback, player.whoAmI);
                    break;
                case 5:
                    Projectile.NewProjectile(source, RotatePos, safeAim, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, RotatePos, new Vector2(0, 0), ModContent.ProjectileType<ShadewoodBowP>(), damage, knockback, player.whoAmI);
                    break;
            }
            counter++;
            if (counter > 5)
            {
                counter = 0;
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
            .AddIngredient(ItemID.WoodenBow, 1)
            .AddIngredient(ItemID.BorealWoodBow, 1)
            .AddIngredient(ItemID.RichMahoganyBow, 1)
            .AddIngredient(ItemID.PalmWoodBow, 1)
            .AddIngredient(ItemID.EbonwoodBow, 1)
            .AddIngredient(ItemID.ShadewoodBow, 1)
            .AddIngredient(ItemID.PearlwoodBow, 1)
            .Register();
        }
    }
}
