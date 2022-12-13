using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class EmeraldGemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
        }
        int count = 0;
        public override void AI()
        {
            int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
            Main.dust[dustnumber].noGravity = true;
            float RotateAccordinglyToVel = Projectile.velocity.SafeNormalize(Vector2.UnitX).ToRotation();
            Projectile.rotation += MathHelper.ToRadians(10 + RotateAccordinglyToVel);
            if (Projectile.velocity.X < 1 && Projectile.velocity.X > -1 && Projectile.velocity.Y < 1 && Projectile.velocity.Y > -1)
            {
                count++;
            }
            else
            {
                if (count == 0)
                {
                    Projectile.velocity -= Projectile.velocity * 0.1f;
                }
            }
            if (count != 0)
            {
                CheckProjectilecollide(Projectile.Center);
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= 20)
                {
                    if (Projectile.ai[1] == 20) { Projectile.velocity = -Projectile.velocity; }
                    if (Projectile.ai[1] <= 50)
                    {
                        if (Projectile.ai[1] == 50)
                        {
                            Projectile.velocity = new Vector2((float)Math.Round(Projectile.velocity.X, 2), (float)Math.Round(Projectile.velocity.Y, 2));
                        }
                        Projectile.damage++;
                        Projectile.velocity += Projectile.velocity * 0.1f;
                    }
                }
            }
        }
        public void CheckProjectilecollide(Vector2 CurrentPos)
        {
            int count = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.ModProjectile is EmeraldGemP && proj.active)
                {
                    float ProjectileDis = Vector2.Distance(CurrentPos, proj.Center);
                    if (ProjectileDis <= 35)
                    {
                        count++;
                        if (count >= 2)
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int l = 0; l < 4; l++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)), ModContent.ProjectileType<SmallEmerald>(), (int)(Projectile.damage * 0.5f), 1f, Projectile.owner);
            }
            for (int i = 0; i < 100; i++)
            {
                Vector2 Ran = Main.rand.NextVector2CircularEdge(15f, 15f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Ran.X, Ran.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
    public class SmallEmerald : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/RangeSynergyWeapon/MagicBow/EmeraldGemP";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.scale = 0.5f;
            Projectile.timeLeft = 60;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 20 && Projectile.velocity.Y <= 20) Projectile.velocity.Y += 0.5f;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 Ran = Main.rand.NextVector2CircularEdge(5f, 5f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Ran.X, Ran.Y, 0, default, Main.rand.NextFloat(.6f, 1f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
