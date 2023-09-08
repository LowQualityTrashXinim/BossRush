using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark
{
    internal class ChaosMiniShark : SynergyModItem, IRogueLikeRangeGun
    {
        int counter = 0;

        public float OffSetPosition => 70;

        public float Spread { get; set; }
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(72, 32, 24, 2f, 6, 6, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10f, true, AmmoID.Bullet);

            Item.rare = 4;
            Item.value = Item.buyPrice(platinum: 5);
            Item.UseSound = SoundID.Item11;

            Spread = 10;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.43f;
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            counter++;
            if (counter == 2)
            {
                for (int i = 0; i < 2 + Main.rand.Next(9); i++)
                {
                    int type2 = Main.rand.Next(new int[] { ProjectileID.StarCannonStar, ProjectileID.BookOfSkullsSkull, ProjectileID.ClothiersCurse, ProjectileID.GiantBee, ProjectileID.Bee, ProjectileID.Grenade, ProjectileID.BallofFire, ProjectileID.WaterBolt, ProjectileID.DemonScythe, ProjectileID.IceBolt, ProjectileID.EnchantedBeam, ProjectileID.BoneGloveProj });
                    Vector2 velocity2 = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                    Projectile.NewProjectile(source, position, velocity2, type2, damage, knockback, player.whoAmI);
                    counter = 0;
                }
            }
            CanShootItem = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
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
