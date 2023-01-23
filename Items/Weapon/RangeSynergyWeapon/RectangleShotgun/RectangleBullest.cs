using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
{
    class RectangleBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 70;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.light = 0.7f;
            Projectile.timeLeft = 400;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }
    }
}
