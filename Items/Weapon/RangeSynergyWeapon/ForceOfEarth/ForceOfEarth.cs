using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.ForceOfEarth
{
    internal class ForceOfEarth : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("let them witness the force of earth");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 74;
            Item.rare = 3;

            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;

            Item.shootSpeed = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.damage = 32;
            Item.knockBack = 3;
            Item.crit = 12;
            Item.value = Item.buyPrice(platinum: 5);

            Item.UseSound = SoundID.Item5;
        }
        int[] ArrayOfProjectile = new int[] {
            ModContent.ProjectileType<CopperBowP>(),
            ModContent.ProjectileType<TinBowP>(),
            ModContent.ProjectileType<IronBowP>(),
            ModContent.ProjectileType<LeadBowP>(),
            ModContent.ProjectileType<SilverBowP>(),
            ModContent.ProjectileType<TungstenBowP>(),
            ModContent.ProjectileType<GoldBowP>(),
            ModContent.ProjectileType<PlatinumBowP>()
        };
        public override void HoldItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<EarthPower>(), 2);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CopperBowP>()] < 1)
            {
                for (int i = 0; i < ArrayOfProjectile.Length; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(0, 0), ArrayOfProjectile[i], (int)(Item.damage * 0.25f), 0, player.whoAmI);
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(45 * i));
                Vector2 newpostion = position + Rotate * 40;
                Vector2 Aim = Main.MouseWorld - newpostion;
                Vector2 SafeAim = Aim.SafeNormalize(Vector2.UnitX);
                Projectile.NewProjectile(source, newpostion, SafeAim * Item.shootSpeed, type, damage, knockback, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.CopperBow)
                .AddIngredient(ItemID.TinBow)
                .AddIngredient(ItemID.IronBow)
                .AddIngredient(ItemID.LeadBow)
                .AddIngredient(ItemID.SilverBow)
                .AddIngredient(ItemID.TungstenBow)
                .AddIngredient(ItemID.GoldBow)
                .AddIngredient(ItemID.PlatinumBow)
                .Register();
        }
    }
}
