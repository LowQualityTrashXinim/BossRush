using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class RubyGemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
        }
        int count = 0;
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 20)
            {
                Projectile.velocity -= Projectile.velocity * 0.1f;
                if ((Math.Abs(Projectile.velocity.X) < 1 && Math.Abs(Projectile.velocity.Y) < 1) || count == 1)
                {
                    if (CheckNearByProjectile() && count == 0)
                    {
                        Projectile.penetrate = 1;
                        Projectile.damage *= 5;
                        count++;
                        Projectile.timeLeft = 900;
                    }
                    if (count == 1)
                    {
                        if (CheckNearByProjectile(false))
                        {
                            Projectile.damage += 10;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 RandomCir = Main.rand.NextVector2Circular(5f, 5f);
                            int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                            Main.dust[dustnumber].noGravity = true;
                        }
                        Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 3f;
                        Projectile.rotation = Projectile.velocity.ToRotation();
                    }
                }
            }
        }
        public bool CheckNearByProjectile(bool CheckItSelf = true)
        {
            int count = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.ModProjectile is RubyGemP && CheckItSelf)
                {
                    float Distance = Vector2.Distance(Projectile.Center, proj.Center);
                    if (Distance <= 30)
                    {
                        count++;
                        if (count >= 2)
                        {
                            return true;
                        }
                    }
                }
                if (proj.ModProjectile is RubyBolt)
                {
                    float Distance = Vector2.Distance(Projectile.Center, proj.Center);
                    if (Distance <= 30)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 RandomCir = Main.rand.NextVector2Circular(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
