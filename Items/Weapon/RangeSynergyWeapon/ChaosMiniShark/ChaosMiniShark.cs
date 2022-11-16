using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark
{
    internal class ChaosMiniShark : ModItem
    {
        int counter = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("not my best work that for sure\nHave 80% of not consuming ammo");
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 32;

            Item.damage = 35;
            Item.knockBack = 2f;
            Item.shootSpeed = 10;
            Item.useTime = 5;
            Item.useAnimation = 5;

            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 4;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(platinum: 5);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.8f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 70f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            counter++;
            if (counter == 2)
            {
                for (int i = 0; i < 2 + Main.rand.Next(9); i++)
                {
                    int type2 = Main.rand.Next(new int[] { ProjectileID.StarCannonStar, ProjectileID.BookOfSkullsSkull, ProjectileID.ClothiersCurse, ProjectileID.GiantBee, ProjectileID.Bee, ProjectileID.Grenade, ProjectileID.BallofFire, ProjectileID.WaterBolt, ProjectileID.DemonScythe, ProjectileID.IceBolt, ProjectileID.EnchantedBeam, ProjectileID.BoneGloveProj });
                    Vector2 velocity2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                    Projectile.NewProjectile(source, position, velocity2, type2, damage, knockback, player.whoAmI);
                    counter = 0;
                }
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
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.FlowerofFire)
                .AddIngredient(ItemID.WaterBolt)
                .AddIngredient(ItemID.BookofSkulls)
                .AddIngredient(ItemID.DemonScythe)
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.Grenade)
                .AddIngredient(ItemID.BeeGun)
                .AddIngredient(ItemID.WaspGun)
                .AddIngredient(ItemID.StarCannon)
                .Register();
        }
    }
}
