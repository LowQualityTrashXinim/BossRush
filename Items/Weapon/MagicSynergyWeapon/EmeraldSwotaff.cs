using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class EmeraldSwotaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Genius idea\\\\Todo : make alt attack");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 45;
            Item.crit = 10;
            Item.knockBack = 3f;
            Item.useTime = 2;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;

            Item.shoot = ProjectileID.EmeraldBolt;
            Item.shootSpeed = 7;
            Item.mana = 10;

            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item8;
        }
        int i = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float rotation = MathHelper.ToRadians(20);
            Vector2 Rotate = velocity.RotatedBy(MathHelper.Lerp(0f, rotation, i / 9f));
            Vector2 Rotate2 = velocity.RotatedBy(MathHelper.Lerp(0f, -rotation, i / 9f));

            Projectile.NewProjectile(source, position, Rotate, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, Rotate2, type, damage, knockback, player.whoAmI);
            i++;
            if (i > 9)
            {
                i = 0;
            }
            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.TungstenBroadsword)
                .AddIngredient(ItemID.EmeraldStaff)
                .Register();
        }
    }
}
