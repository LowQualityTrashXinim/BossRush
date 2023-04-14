using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
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
        int count = 0;
        public override void AI()
        {
            if (count == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                count++;
            }
            Projectile.velocity *= .97f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 Direction = Projectile.rotation.ToRotationVector2() * 35;
            Vector2 Head = Projectile.Center + Direction;
            Vector2 End = Projectile.Center - Direction;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Head, End, 22, ref point);
        }
    }
}
