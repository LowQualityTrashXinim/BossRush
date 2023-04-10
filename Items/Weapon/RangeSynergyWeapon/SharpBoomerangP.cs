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
        int firstframe = 0;
        float MaxLengthX = 0;
        float MaxLengthY = 0;
        float MouseXPosDirection;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (firstframe == 0)
            {
                MouseXPosDirection = (Main.MouseWorld.X - player.Center.X) > 0 ? 1 : -1;
                MaxLengthX = (Main.MouseWorld - player.MountedCenter).Length();
                MaxLengthY = MaxLengthX * .3333333f * -MouseXPosDirection;
                firstframe++;
            }

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

            float FirstProgress = duration / 3f;
            float SecondProgress = FirstProgress * 2f;
            float timeleftcountbackward = (duration - Projectile.timeLeft);
            float progressY = timeleftcountbackward / FirstProgress;
            if (timeleftcountbackward > FirstProgress)
            {
                progressY = 1 - (progressY - 1) * 2;
            }
            if (timeleftcountbackward > SecondProgress)
            {
                progressY = Math.Abs(progressY) - 4;
            }
            progressY = Math.Clamp(progressY, -1, 1);
            float X = MathHelper.Lerp(0, MaxLengthX, BossRushUtils.OutExpo(progressX));
            float Y = MathHelper.Lerp(0, MaxLengthY, BossRushUtils.InOutSine(progressY));
            Vector2 VelocityPosition = new Vector2(X,Y).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.Center = player.MountedCenter + VelocityPosition;
            Projectile.rotation += MathHelper.ToRadians(15);
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
