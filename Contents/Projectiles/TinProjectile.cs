using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BossRush.Contents.Projectiles
{
    internal class TinOreProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinOre);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.ai[1] <= 0;
        public override void AI()
        {
            Projectile.ai[1] = BossRushUtils.CoolDown(Projectile.ai[1]);
            Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 20)
            {
                if (Projectile.velocity.Y < 30)
                    Projectile.velocity.Y++;
            }
        }
    }
    class TinBarProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinBar);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
            if (Projectile.ai[0] > 20)
            {
                if (Projectile.velocity.Y < 30)
                    Projectile.velocity.Y += 1.5f;
            }
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = -Vector2.UnitY.Vector2DistributeEvenly(4, 60, i) * 5f;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<TinOreProjectile>(), (int)(Projectile.damage * .5f), Projectile.knockBack, Projectile.owner, 0, 20);
            }
        }
    }
    class TinBroadSwordProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinBroadsword);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 999;
            Projectile.usesLocalNPCImmunity = true;
        }
        int MouseXPosDirection;
        float MaxLengthX = 0;
        float MaxLengthY = 0;
        int maxProgress = 35;
        int progression = 0;
        Vector2 offset = Vector2.Zero;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft == 999)
            {
                MaxLengthX = 90;
                progression = maxProgress;
                MouseXPosDirection = Main.rand.NextBool().BoolOne() * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
                MaxLengthY = -MaxLengthX * .5f * MouseXPosDirection;
            }
            if (progression <= 0)
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (30 - Projectile.timeLeft) / 30f);
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = player.Center + offset;
                return;
            }
            int halfmaxProgress = (int)(maxProgress * .5f);
            int quadmaxProgress = (int)(maxProgress * .25f);
            float progress;
            if (progression > halfmaxProgress)
            {
                progress = (maxProgress - progression) / (float)halfmaxProgress;
            }
            else
            {
                progress = progression / (float)halfmaxProgress;
            }
            float X = MathHelper.SmoothStep(-40, MaxLengthX, progress);
            ProgressYHandle(progression, halfmaxProgress, quadmaxProgress, out float Y);
            Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
            offset = VelocityPosition;
            Projectile.Center = player.Center + VelocityPosition;
            float rotation = MathHelper.SmoothStep(0, 360, 1 - progression / (float)maxProgress) * MouseXPosDirection;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi + MathHelper.ToRadians(rotation);
            progression--;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
        }
        private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y)
        {
            if (timeleft > progressMaxHalf + progressMaxQuad)
            {
                float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
                Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
                return;
            }
            if (timeleft > progressMaxQuad)
            {
                float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
                Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
                return;
            }
            else
            {
                float progressY = 1 - timeleft / progressMaxQuad;
                Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
                return;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos2 = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos2, null, color, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
    class TinShortSwordProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinShortsword);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        int MaxProgression = 50;
        int progression = 0;
        Vector2 spawnPosition = Vector2.Zero;
        float length = 0;
        public override void AI()
        {
            if (Projectile.timeLeft == 300)
            {
                progression = MaxProgression;
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
                spawnPosition = Projectile.Center;
                length = Math.Clamp((Main.MouseWorld - Projectile.Center).Length(), 0, 110);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (progression <= 0)
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (30 - Projectile.timeLeft) / 30f);

            }
            float halfMaxprogress = MaxProgression * .5f;
            float progress;
            if(progression > halfMaxprogress)
            {
                progress = (MaxProgression - progression) / halfMaxprogress;
            }
            else
            {
                progress = progression / halfMaxprogress;
            }
            Projectile.Center = spawnPosition + Vector2.SmoothStep(Projectile.velocity, Projectile.velocity * length, progress);
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.PiOver4;
            }
            else
            {
                Projectile.rotation += MathHelper.PiOver4;
            }
            progression--;
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}