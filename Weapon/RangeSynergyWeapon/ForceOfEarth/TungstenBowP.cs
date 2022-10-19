using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.RangeSynergyWeapon.ForceOfEarth
{
    internal class TungstenBowP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 16;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<EarthPower>()))
            {
                Projectile.Kill();
            }
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(90));
            Projectile.Center = player.Center + Rotate * 40;

            Vector2 aimto = Main.MouseWorld - Projectile.position;
            Vector2 safeAimto = aimto.SafeNormalize(Vector2.UnitX);
            Projectile.rotation = safeAimto.ToRotation();
        }
    }
}
