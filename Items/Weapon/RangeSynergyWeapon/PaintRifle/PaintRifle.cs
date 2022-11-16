using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.PaintRifle
{
    internal class PaintRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Now come with mega mode");
        }
        public override void SetDefaults()
        {
            Item.width = 114;
            Item.height = 40;
            Item.rare = 3;

            Item.damage = 20;
            Item.crit = 7;
            Item.knockBack = 2f;

            Item.useTime = 2;
            Item.useAnimation = 8;
            Item.reuseDelay = 12;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(silver: 1000);

            Item.shoot = ProjectileID.PainterPaintball;
            Item.shootSpeed = 17;
            Item.scale -= 0.25f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-33, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
            Vector2 offset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, offset * offset, 0, 0))
            {
                position += offset;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PainterPaintballGun, 2)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
