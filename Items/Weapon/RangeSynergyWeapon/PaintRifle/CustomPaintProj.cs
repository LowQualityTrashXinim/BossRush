using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.PaintRifle
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
        int r = Main.rand.Next(256); int b = Main.rand.Next(256); int g = Main.rand.Next(256);
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Paint, 0, 0, 0, new Color(r, g, b),1f);
            Main.dust[dust].noGravity = true;
            Projectile.ai[0]++;
            if(Projectile.ai[0] >= 30)
            {
                if (Projectile.velocity.Y <= 6)
                {
                    Projectile.velocity.Y += 0.05f;
                }
            }
        }
    }
}
