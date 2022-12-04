using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.RifleShotgun
{
    class RifleShotgun : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("is it a shotgun or a rifle ?");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.knockBack = 1.5f;
            Item.shootSpeed = 15f;
            Item.height = 30;
            Item.width = 100;

            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = 5;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = 4;

            Item.useTime = 10;
            Item.shoot = 10;
            Item.useAnimation = 30;
            Item.reuseDelay = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item38;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 70f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float ProjNum = 6;
            float Rotation = MathHelper.ToRadians(6);
            for (int i = 0; i < ProjNum; i++)
            {
                Vector2 Rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(Rotation, -Rotation, i / (ProjNum - 1)));
                float RandomSpeadx = Main.rand.NextFloat(0.5f, 1f);
                float RandomSpeady = Main.rand.NextFloat(0.5f, 1f);
                Projectile.NewProjectile(source, position.X, position.Y, Rotate.X * RandomSpeadx, Rotate.Y * RandomSpeady, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Shotgun)
                .AddIngredient(ItemID.ClockworkAssaultRifle)
                .Register();
        }
    }
}
