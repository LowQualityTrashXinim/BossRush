using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.Katana
{
    public abstract class BaseSlash : ModProjectile
    {
        protected virtual float HoldoutRangeMax => 30f;
        protected virtual float HoldoutRangeMin => 10f;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 80;
            Projectile.height = 108;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            float halfDuration = duration / 2;
            float progress = ( duration - Projectile.timeLeft )/ halfDuration;
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            if (Projectile.ai[0] >= 5)
            {
                Projectile.scale -= 0.03f;
                Projectile.alpha += 20;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 8;
            Vector2 PosRand = Projectile.position + Main.rand.NextVector2CircularEdge(150, 150);
            Vector2 toTarget = (target.Center - PosRand).SafeNormalize(Vector2.UnitX) * 5;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), PosRand, toTarget, ModContent.ProjectileType<PlatinumSlash>(), damage, knockback, Projectile.owner);
        }
    }
    public class KatanaSlash : BaseSlash
    {

    }

    public class KatanaSlashUpsideDown : BaseSlash
    {

    }
}
