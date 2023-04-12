using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    internal class SuperShortSwordOrbitShortSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.height = 32;
            Projectile.width = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.light = 0.7f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        Player player => Main.player[Projectile.owner];
        Vector2 RotatePosition = Vector2.Zero;
        Vector2 FixedMousePosition = Vector2.Zero;
        Vector2 FixedProjectilePos = Vector2.Zero;
        int Counter = 0;
        int timer = 999;
        public override bool PreAI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            if (player.ItemAnimationJustStarted)
            {
                FixedMousePosition = Main.MouseWorld;
                FixedProjectilePos = Projectile.Center;
            }
            if (player.ItemAnimationActive)
            {
                Vector2 PositionToMouse = FixedMousePosition - FixedProjectilePos;
                Vector2 ToMouse = PositionToMouse.SafeNormalize(Vector2.UnitX);
                float duration = player.itemAnimationMax;
                float halfProgress = duration * .5f;
                float progress;
                if (timer > duration)
                {
                    timer = (int)duration;
                }
                if (timer < halfProgress)
                {
                    progress = timer / halfProgress;
                }
                else
                {
                    progress = (duration - timer) / halfProgress;
                }
                Projectile.rotation = ToMouse.ToRotation() + MathHelper.PiOver4;
                Projectile.Center = RotatePosition + Vector2.SmoothStep(ToMouse, ToMouse * PositionToMouse.Length(), progress);
                timer--;
            }
            if(timer == 0)
            {
                timer = 999;
            }
            RotatePosition = getPosToReturn(player, 45 * Projectile.ai[0], Counter);
            return !player.ItemAnimationActive;
        }
        public override void AI()
        {
            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * 0.25f * player.GetTotalDamage(DamageClass.Melee).Additive);
            Projectile.CritChance = (int)(player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Generic));
            if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<SuperShortSwordPower>()))
            {
                Projectile.Kill();
            }
            if (Counter == MathHelper.TwoPi * 100 || Counter == -MathHelper.TwoPi * 100) { Counter = 0; }
            if (player.direction == 1)
            {
                Counter++;
            }
            else
            {
                Counter--;
            }
            RotatePosition = getPosToReturn(player, 45 * Projectile.ai[0], Counter);
            Projectile.Center = RotatePosition;
        }
        public Vector2 getPosToReturn(Player player, float offSet, int Counter, float Distance = 50)
        {
            Vector2 SafeDegree = (Main.MouseWorld - Projectile.position).SafeNormalize(Vector2.UnitX);
            if (!player.ItemAnimationActive) Projectile.rotation = SafeDegree.ToRotation() + MathHelper.PiOver4;
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
            return player.Center + Rotate.RotatedBy(Counter * 0.05f) * Distance;
        }
    }
}