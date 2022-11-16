using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.DarkCactus
{
    internal class CactusBall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = true;
            Projectile.penetrate = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10)
            {
                Projectile.netUpdate = true;
                Projectile.rotation += 0.5f;

                if (Projectile.velocity.Y <= 20) Projectile.velocity.Y += 0.3f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.85f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.position += new Vector2(12, 12);
            float rotate = MathHelper.ToRadians(180);
            float projectileNum = 6;
            for (int i = 0; i < projectileNum; i++)
            {
                Vector2 rotation = new Vector2(5, 5).RotatedBy(MathHelper.Lerp(rotate, -rotate, i / (projectileNum - 1)));
                Projectile.NewProjectile(null, Projectile.position, rotation, ProjectileID.TentacleSpike, (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, Projectile.owner);
            }
        }
    }
}
