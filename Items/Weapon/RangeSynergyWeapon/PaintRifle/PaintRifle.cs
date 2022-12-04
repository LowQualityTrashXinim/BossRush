using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.PaintRifle
{
    internal class PaintRifle : WeaponTemplate, ISynergyItem
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

            Item.shoot = ModContent.ProjectileType<CustomPaintProj>();
            Item.shootSpeed = 7;
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
            position = PositionOFFSET(position, velocity, 42);
        }

        public static int r = Main.rand.Next(256);
        public static int b = Main.rand.Next(256);
        public static int g = Main.rand.Next(256);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            r = 0; b = 0; g = 0;
            for (int i = 0; i < 2; i++)
            {
                int randChooser = Main.rand.Next(3);
                switch (randChooser)
                {
                    case 0:
                        r = 255;
                        break;
                    case 1:
                        b = 255;
                        break;
                    case 2:
                        g = 255;
                        break;
                }
            }
            type = ModContent.ProjectileType<CustomPaintProj>();
            Vec2ToRotate = velocity;
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    velocity = RandomSpread(RotateRandom(15),2,1.2f);
                    Projectile.NewProjectile(source, position, velocity, type, (int)(damage*.7f), knockback, player.whoAmI);
                    for (int l = 0; l < 15; l++)
                    {
                        Vector2 spread = RandomSpread(RotateRandom(35), 3, .2f) + player.velocity;
                        int dust = Dust.NewDust(position, 0, 0, DustID.Paint, spread.X, spread.Y, 0, new Color(r, g, b), Main.rand.NextFloat(1.2f, 1.45f));
                        Main.dust[dust].noGravity = true;
                    }
                }
                return false;
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 spread = RandomSpread(RotateRandom(35),3,.2f) + player.velocity;
                int dust = Dust.NewDust(position, 0, 0, DustID.Paint, spread.X, spread.Y,0,new Color(r,g,b),Main.rand.NextFloat(1f,1.45f));
                Main.dust[dust].noGravity = true;
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
