using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon
{
    internal class SharpBoomerangP : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/RangeSynergyWeapon/SharpBoomerang";

        public override void SetDefaults()
        {
            Projectile.scale = 0.75f;
            Projectile.width = 38;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        int count = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float MaxLengthX = (Main.MouseWorld - player.Center).Length();
            float MaxLengthY = MaxLengthX / 3f;
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            float HalfDuration = duration * 0.5f;

            float progressX;
            if (Projectile.timeLeft < HalfDuration)
            {
                progressX = Projectile.timeLeft / HalfDuration;
            }
            else
            {
                progressX = (duration - Projectile.timeLeft) / HalfDuration;
            }
            float FirstProgress = player.itemAnimationMax / 3f;
            float SecondProgress = FirstProgress * 2f;
            float progressY = player.itemAnimation / FirstProgress;
            if (player.itemAnimation >= FirstProgress)
            {
                progressY -= (progressY - 1) * 3f;
            }
            if (player.itemAnimation >= SecondProgress)
            {
                progressY -= (Math.Abs(progressY) - 2) * 2;
            }
            progressY = Math.Clamp(progressY, -1, 1);
            Vector2 VelocityPosition = new Vector2(MathHelper.SmoothStep(Projectile.velocity.X * 0, Projectile.velocity.X * MaxLengthX, progressX),
                                                   MathHelper.SmoothStep(Projectile.velocity.Y * 0, Projectile.velocity.Y * MaxLengthY, progressY));
            Projectile.Center = player.MountedCenter + VelocityPosition;
            Projectile.rotation += MathHelper.ToRadians(60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 RandomPos = Projectile.Center + Main.rand.NextVector2CircularEdge(50, 50);
            Vector2 DistanceToAim = (target.Center - RandomPos).SafeNormalize(Vector2.UnitX) * 4f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), RandomPos, DistanceToAim, ProjectileID.SuperStarSlash, Projectile.damage, 0, Projectile.owner);
            target.immune[Projectile.owner] = 7;
        }
    }
}
