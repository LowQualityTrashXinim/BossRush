using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class ShadowSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 30;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
