using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
{
    class SquareBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.light = 0.7f;
            Projectile.timeLeft = 10;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 10 == 0)
            {
                Projectile.damage -= 1;
            }
        }
    }
}
