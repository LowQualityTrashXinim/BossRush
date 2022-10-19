using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedGoldSwordP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.75f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 4;
        }

        public override void AI()
        {
            Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX) * 3;
            if (Projectile.velocity.X > 15)
            {
                Projectile.velocity.X = 15;
            }
            else if (Projectile.velocity.X < -15)
            {
                Projectile.velocity.X = -15;
            }
            if (Projectile.velocity.Y > 15)
            {
                Projectile.velocity.Y = 15;
            }
            else if (Projectile.velocity.Y < -15)
            {
                Projectile.velocity.Y = -15;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
    }
}
