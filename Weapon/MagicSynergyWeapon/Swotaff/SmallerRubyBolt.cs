using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class SmallerRubyBolt : ModProjectile
    {
        public override string Texture => "BossRush/Weapon/MagicSynergyWeapon/Swotaff/GiantRubyBolt";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        int count = 0;
        public override void AI()
        {
            SelectFrame();
            Projectile.alpha += 5;
            Projectile.scale-=0.015f;
            if (count >= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, -Projectile.velocity.X+Main.rand.Next(-5,5), -Projectile.velocity.Y + Main.rand.Next(-5, 5), 100, default, Projectile.scale);
                    Main.dust[dustnumber].noGravity = true;
                }
                count = 0;
            }
            count++;
        }
        public void SelectFrame()
        {
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
