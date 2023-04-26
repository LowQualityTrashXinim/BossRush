using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class RubyBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.light = 1f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            float ToRandom = Main.rand.Next(3);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RubyGemP>()] < 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 Rotate = Vector2.One.RotatedBy(MathHelper.ToRadians(45 * i + ToRandom * 30)) * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Rotate, ModContent.ProjectileType<RubyGemP>(), Projectile.damage, 0, Projectile.owner);
                }
            }
        }

        public override void AI()
        {
            int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemRuby, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
            Main.dust[dustnumber].noGravity = true;
            Main.dust[dustnumber].velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero) + Main.rand.NextVector2Circular(1, 1);
            if (CheckNearByProjectile())
            {
                Projectile.damage = (int)(Projectile.damage * 1.5f);
                Projectile.velocity *= 1.25f;
            }
        }

        public bool CheckNearByProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].ModProjectile is RubyGemP && Main.projectile[i].active)
                {
                    float Distance = Vector2.Distance(Projectile.Center, Main.projectile[i].Center);
                    if (Distance <= 20)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 RandomCir = Main.rand.NextVector2Circular(3f, 3f);
                int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(Projectile.GetAlpha(lightColor), .02f);
            return true;
        }
    }
}
