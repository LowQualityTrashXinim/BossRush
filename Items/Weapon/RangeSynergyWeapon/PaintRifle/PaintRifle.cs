using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.PaintRifle
{
    internal class PaintRifle : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Mega mode included\"" +
                "\nIt have literal screw as a way to activate mega mode");
        }
        public override void SetDefaults()
        {
            Item.width = 114;
            Item.height = 40;
            Item.rare = 3;

            Item.damage = 25;
            Item.crit = 7;
            Item.knockBack = 2f;

            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.reuseDelay = 9;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item5;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(silver: 1000);

            Item.shoot = ProjectileID.PainterPaintball;
            Item.shootSpeed = 23;
            Item.scale -= 0.35f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-33, 2);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = PositionOFFSET(position, velocity, 30);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vec2ToRotate = velocity;
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    velocity = RandomSpread(RotateRandom(24),3,1.2f);
                    Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity, type, (int)(damage*.7f), knockback, player.whoAmI);
                }
                return false;
            }
            return true;
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
