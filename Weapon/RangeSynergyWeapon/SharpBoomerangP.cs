using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using Terraria.ID;

namespace BossRush.Weapon.RangeSynergyWeapon
{
    internal class SharpBoomerangP : ModProjectile
    {
        public override string Texture => "BossRush/Weapon/RangeSynergyWeapon/SharpBoomerang";

        public override void SetDefaults()
        {
            Projectile.scale = 0.75f;
            Projectile.width = 38;
            Projectile.height = 72;
            Projectile.timeLeft = 23;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        int count = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float distance = 60;
            Vector2 newMove = player.Center - Projectile.Center;
            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
            Projectile.rotation += MathHelper.ToRadians(60);
            if (Projectile.timeLeft < 3 || distanceTo > 300)
            {
                Vector2 GoBack = player.Center - Projectile.Center;
                Vector2 SafeGoBack = GoBack.SafeNormalize(Vector2.UnitY);
                if (count >= 15)
                {
                    Projectile.knockBack = 0;
                    Projectile.velocity = SafeGoBack * 40f;
                }
                Projectile.timeLeft = 2;
                Projectile.velocity += SafeGoBack * 5f;

                if (distanceTo < distance)
                {
                    Projectile.Kill();
                }
                count++;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 RandomPos = Projectile.Center + Main.rand.NextVector2CircularEdge(50,50);
            Vector2 DistanceToAim = (target.Center - RandomPos).SafeNormalize(Vector2.UnitX)*4f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), RandomPos, DistanceToAim, ProjectileID.SuperStarSlash, Projectile.damage, 0, Projectile.owner);
            target.immune[Projectile.owner] = 1;
        }
    }
}
