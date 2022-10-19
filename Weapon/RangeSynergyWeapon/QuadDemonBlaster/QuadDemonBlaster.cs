using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.RangeSynergyWeapon.QuadDemonBlaster
{
    class QuadDemonBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quad Demon Blaster");
            Tooltip.SetDefault("Now with extra large spread! Good for 'accidentally' killing Voodoo Demons.");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 39;
            Item.knockBack = 1.5f;
            Item.shootSpeed = 15f;
            Item.height = 30;
            Item.width = 46;

            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = 5;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = 3;

            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.shoot = ProjectileID.Bullet;
            Item.reuseDelay = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = false;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float ProjNum = 10;
            float Rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < ProjNum; i++)
            {
                Vector2 Rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(Rotation, -Rotation, i / (ProjNum - 1)));
                float RandomSpeadx = Main.rand.NextFloat(0.5f, 1f);
                float RandomSpeady = Main.rand.NextFloat(0.5f, 1f);
                Projectile.NewProjectile(source, position.X, position.Y, Rotate.X * RandomSpeadx, Rotate.Y * RandomSpeady, type, damage, knockback, player.whoAmI);
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.QuadBarrelShotgun)
                .AddIngredient(ItemID.PhoenixBlaster)
                .Register();
        }
    }
}
