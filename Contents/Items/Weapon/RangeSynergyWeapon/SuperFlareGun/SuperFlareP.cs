using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SuperFlareGun
{
    internal class SuperFlareP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 100;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = -20f;
            return false;
        }
        public override void AI()
        {
            int RandomDust = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemTopaz });
            for (int i = 0; i < 7; i++)
            {
                Vector2 Rotate = Main.rand.NextVector2Circular(5f, 5f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 3)
            {
                Vector2 RandomPos = Projectile.Center + Main.rand.NextVector2Circular(50f, 50f);
                //Projectile.NewProjectile(Projectile.GetSource_FromThis(), RandomPos, Vector2.Zero, ModContent.ProjectileType<ExplodeProjectile>(), 0, 0, Projectile.owner);
                Projectile.ai[0] = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            float randomRotation = Main.rand.NextFloat(90);
            int RandomDust = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemAmber, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz });
            Vector2 Rotate;
            for (int i = 0; i < 150; i++)
            {
                Rotate = Main.rand.NextVector2Circular(25f, 25f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 2.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            for (int i = 0; i < 300; i++)
            {
                if (i % 2 == 0)
                {
                    Rotate = Main.rand.NextVector2CircularEdge(10f, 30f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 1.5f;
                }
                else
                {
                    Rotate = Main.rand.NextVector2CircularEdge(30f, 10f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 1.5f;
                }
                int dustnumber1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.75f, 2f));
                Main.dust[dustnumber1].noGravity = true;
            }
        }
    }
    internal class ExplodeProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 55;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 50)
            {
                Projectile.damage = 50;
            }
        }
        public override void Kill(int timeLeft)
        {
            int RandomDust = Main.rand.Next();
            for (int i = 0; i < 55; i++)
            {
                Vector2 Rotate = Main.rand.NextVector2CircularEdge(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.Center, 10, 10, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }
    }
}