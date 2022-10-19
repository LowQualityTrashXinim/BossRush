using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.RangeSynergyWeapon
{
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
                if (Projectile.ai[0] == 51)
                {
                    int RandomDust = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemAmber, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz });
                    for (int i = 0; i < 55; i++)
                    {
                        Vector2 Rotate = Main.rand.NextVector2CircularEdge(10f, 10f);
                        int dustnumber = Dust.NewDust(Projectile.Center, 10, 10, RandomDust, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                }
                Projectile.netUpdate = true;
                Projectile.damage = 50;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
        }
    }
}
