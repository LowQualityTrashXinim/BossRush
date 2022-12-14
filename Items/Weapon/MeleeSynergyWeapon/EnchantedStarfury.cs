using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class EnchantedStarfury : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A truely unthinkable");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;

            Item.damage = 32;
            Item.knockBack = 4f;
            Item.useTime = 60;
            Item.useAnimation = 20;
            Item.rare = 4;
            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 20f;

            Item.width = 66;
            Item.height = 66;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        int switchProj = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 5; i++)
            {
                if (switchProj % 2 == 0)
                {
                    Vector2 rotate = velocity.Vector2DistributeEvenly(5,player.direction == 1 ? -40 : 40, i);
                    Projectile.NewProjectile(source, position, rotate * .5f, ModContent.ProjectileType<EnchantedSwordProjectile>(), damage, knockback, player.whoAmI, i);
                }
                else
                {
                    Vector2 customPos = new Vector2(position.X + Main.rand.Next(-100,100), position.Y - 900 + Main.rand.Next(-200,200));
                    Vector2 aimSpread = Main.MouseWorld + Main.rand.NextVector2Circular(200f,200f);
                    Vector2 velocityTo = (aimSpread - customPos).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Projectile.NewProjectile(source, customPos, velocityTo, ProjectileID.StarCannonStar, damage * 3, knockback, player.whoAmI, i);
                }
            }
            switchProj++;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Starfury)
                .Register();
        }
    }
    class EnchantedSwordProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.penetrate = 1;
            Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        int timer = 0;
        Vector2 localOriginalvelocity;
        public override void AI()
        {
            int dustPar = Main.rand.Next(new int[] {DustID.TintableDustLighted, DustID.YellowStarDust, 57, 58});
            int dust = Dust.NewDust(Projectile.Center,0,0, dustPar, 0,0,0,default,Main.rand.NextFloat(.9f, 1.1f));
            Main.dust[dust].noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (timer == 0)
            {
                localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            }
            if (timer <= 20 + Projectile.ai[0] * 2)
            {
                Projectile.timeLeft = 200;
                Projectile.velocity -= Projectile.velocity * .1f;
                timer++;
            }
            else
            {
                if (!Projectile.velocity.reachedLimited(20)) Projectile.velocity += localOriginalvelocity;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                int dustPar = Main.rand.Next(new int[] { DustID.TintableDustLighted, DustID.YellowStarDust, 57, 58 });
                int dust = Dust.NewDust(Projectile.Center, 0, 0, dustPar, 0, 0, 0, default, Main.rand.NextFloat(1, 1.5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(10f, 10f);
            }
        }
    }
}
