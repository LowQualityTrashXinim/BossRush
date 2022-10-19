using Terraria;
using Terraria.ModLoader;

namespace BossRush.Weapon.RangeSynergyWeapon.HeartPistol
{
    internal class smallerHeart : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 30;
        }
        public override void AI()
        {
            Projectile.velocity -= Projectile.velocity * 0.05f;
        }
    }
}
