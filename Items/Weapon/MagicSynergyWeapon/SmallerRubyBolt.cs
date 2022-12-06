using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class SmallerRubyBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        int count = 0;
        public override void AI()
        {
            Projectile.alpha += 5;
            Projectile.scale -= 0.015f;
            if (count >= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, -Projectile.velocity.X + Main.rand.Next(-5, 5), -Projectile.velocity.Y + Main.rand.Next(-5, 5), 100, default, Projectile.scale);
                    Main.dust[dustnumber].noGravity = true;
                }
                count = 0;
            }
            count++;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
}
