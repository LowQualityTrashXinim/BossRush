using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.PaintRifle
{
    internal class CustomPaintProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = true;
            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Paint, 0, 0, 0, new Color(PaintRifle.r, PaintRifle.g, PaintRifle.b), 1f);
            Main.dust[dust].noGravity = true;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 90)
            {
                if (Projectile.velocity.Y <= 6)
                {
                    Projectile.velocity.Y += 0.05f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 randCircle = Main.rand.NextVector2Circular(6, 6);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Paint, randCircle.X, randCircle.Y, 0, new Color(PaintRifle.r, PaintRifle.g, PaintRifle.b), Main.rand.NextFloat(1, 1.2f));
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
