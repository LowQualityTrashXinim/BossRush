using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    class EnchantedSilverSword : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Ore Sword");
            Tooltip.SetDefault("Amalgamation of a sword");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 26;
            Item.useAnimation = 26;

            Item.damage = 22;
            Item.knockBack = 6f;

            Item.shoot = ModContent.ProjectileType<EnchantedSilverSwordP>();
            Item.shootSpeed = 15f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;
        }
        int count = -1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] RandomShortSword = new int[] {
                ModContent.ProjectileType<EnchantedCopperSwordP>(),
                ModContent.ProjectileType<EnchantedTinSwordP>(),
                ModContent.ProjectileType<EnchantedLeadSwordP>(),
                ModContent.ProjectileType<EnchantedIronSwordP>(),
                ModContent.ProjectileType<EnchantedSilverSwordP>(),
                ModContent.ProjectileType<EnchantedTungstenSwordP>(),
                ModContent.ProjectileType<EnchantedGoldSwordP>(),
                ModContent.ProjectileType<EnchantedPlatinumSwordP>(), };
            if (count < RandomShortSword.Length - 1)
            {
                count++;
            }
            else
            {
                count = 0;
            }
            Vector2 Above = Vector2.Zero;
            Vector2 AimTo = Vector2.Zero;
            float Rotation;
            switch (count)
            {
                case 0:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 1:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 2:
                    Rotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 RotateSurround = velocity.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 8f));
                        Projectile.NewProjectile(source, position, RotateSurround, RandomShortSword[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 3:
                    Above = Main.MouseWorld + velocity.SafeNormalize(Vector2.UnitX) * 250f;
                    AimTo = (player.Center - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Rotation = MathHelper.ToRadians(15);
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 RotateSurround = AimTo.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 5f));
                        Projectile.NewProjectile(source, Above, RotateSurround, RandomShortSword[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 4:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y - 700);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed * 3;
                    Projectile.NewProjectile(source, Above, AimTo, RandomShortSword[count], damage * 3, knockback, player.whoAmI);
                    break;
                case 5:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y + 700);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed * 3;
                    Projectile.NewProjectile(source, Above, AimTo, RandomShortSword[count], damage * 3, knockback, player.whoAmI);
                    break;
                case 6:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 7:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddRecipeGroup("OreShortSword")
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }
}
