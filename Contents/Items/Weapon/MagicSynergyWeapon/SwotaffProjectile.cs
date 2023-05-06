using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon
{
    public abstract class SwotaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        Vector2 PosToGo;
        int FirstFrame = 0;
        bool isAlreadyHeldDown = false;
        bool isAlreadyReleased = false;
        int countdownBeforeReturn = 100;
        int AbsoluteCountDown = 420;
        int timeToSpin = 0;
        int projectileBelongToItem;
        Player player;
        bool isAlreadySpinState = false;
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            if (player.altFunctionUse == 2 || isAlreadySpinState)
            {
                SpinAroundPlayer();
                isAlreadySpinState = true;
                return;
            }
            if (Projectile.ai[0] == 1 || Projectile.ai[0] == -1)
            {
                BossRushUtils.ProjectileSwordSwingAI(Projectile, ref PosToGo, ref FirstFrame, (int)Projectile.ai[0]);
                return;
            }
            SpinAtCursorAI();
        }
        protected virtual int IframeIgnore() => 6;
        protected virtual int AltAttackAmountProjectile() => 8;
        protected virtual int? AltAttackProjectileType() => null;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = IframeIgnore();
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Projectile.ai[0] == 0)
            {
                return;
            }
            BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, Projectile);
        }
        private void SpinAtCursorAI()
        {
            Item item = player.HeldItem;
            if (FirstFrame == 0)
            {
                projectileBelongToItem = item.type;
                PosToGo = Main.MouseWorld;
                FirstFrame++;
            }
            Vector2 length = PosToGo - Projectile.Center;
            if (Main.mouseLeft && !isAlreadyHeldDown && !isAlreadyReleased)
            {
                isAlreadyHeldDown = true;
            }
            if (isAlreadyHeldDown)
            {
                countdownBeforeReturn = 10;
            }
            if (!Main.mouseLeft && Main.mouseLeftRelease && isAlreadyHeldDown)
            {
                isAlreadyHeldDown = false;
                isAlreadyReleased = true;
            }
            countdownBeforeReturn -= countdownBeforeReturn > 0 ? 1 : 0;
            AbsoluteCountDown -= AbsoluteCountDown > 0 ? 1 : 0;
            if (countdownBeforeReturn <= 0 || AbsoluteCountDown <= 0 || item.type != projectileBelongToItem)
            {
                length = player.Center - Projectile.Center;
                float distanceTo = length.Length();
                if (distanceTo < 60)
                {
                    Projectile.Kill();
                }
            }
            Projectile.velocity = length.SafeNormalize(Vector2.Zero) * length.Length() + player.velocity;
            Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
            Projectile.rotation += MathHelper.ToRadians(15);
            Vector2 velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(6, 9);
            int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(velocity, 50), 0, 0, DustID.GemAmethyst);
            Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.2f);
            Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
            Main.dust[dust].noGravity = true;
            if (Projectile.ai[1] == 1)
            {
                if (timeToSpin >= 24)
                {
                    if (player.CheckMana(player.GetManaCost(item), true))
                    {
                        player.statMana -= player.GetManaCost(item);
                    }
                    else
                    {
                        Projectile.ai[1] = -1;
                    }
                    timeToSpin = 0;
                }
                timeToSpin++;
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(velocity, 50), velocity, ProjectileID.AmethystBolt, (int)(Projectile.damage * .55f), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].timeLeft = 30;
            }
            if ((Projectile.Center - player.Center).LengthSquared() > 1000 * 1000)
            {
                Projectile.Kill();
            }
        }
        private void SpinAroundPlayer()
        {
            if (FirstFrame == 0)
            {
                PosToGo = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
                FirstFrame++;
            }
            player.direction = PosToGo.X > 0 ? 1 : -1;
            float maxProgress = player.itemAnimationMax * 2f;
            if (Projectile.timeLeft > maxProgress)
            {
                Projectile.timeLeft = (int)maxProgress;
            }
            player.heldProj = Projectile.whoAmI;
            float percentDone = Projectile.timeLeft / maxProgress;
            percentDone = BossRushUtils.InExpo(percentDone);
            Projectile.spriteDirection = player.direction;
            float baseAngle = PosToGo.ToRotation();
            float start = baseAngle + MathHelper.PiOver2 * player.direction;
            float end = baseAngle - (MathHelper.TwoPi + MathHelper.PiOver2) * player.direction;
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            Projectile.rotation = currentAngle;
            Projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            Projectile.Center = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * 42;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            SpawnDustTrailDelay();
        }
        private void SpawnDustTrailDelay()
        {
            int dust = Dust.NewDust(player.Center.PositionOFFSET(Projectile.rotation.ToRotationVector2(), 50), 0, 0, DustID.GemAmethyst);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 0.1f;
            Main.dust[dust].velocity = Projectile.rotation.ToRotationVector2() * 2f;
            Main.dust[dust].fadeIn = 1.5f;
        }
    }
}