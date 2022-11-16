using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Accessories.EnragedBossAccessories.EvilEye
{
    class PhantasmalEye : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 3;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 30)
            {
                Projectile.netUpdate = true;
                float distance = 1500;

                NPC closestNPC = FindClosestNPC(distance);
                if (closestNPC == null)
                {
                    Projectile.velocity.Y += 0.3f;
                }
                else
                {
                    Projectile.velocity += (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 7;
                    if (Projectile.timeLeft % 50 == 0)
                    {
                        Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 20;
                    }
                    if (Projectile.velocity.X > 40)
                    {
                        Projectile.velocity.X = 40;
                    }
                    else if (Projectile.velocity.X < -40)
                    {
                        Projectile.velocity.X = -40;
                    }
                    if (Projectile.velocity.Y > 40)
                    {
                        Projectile.velocity.Y = 40;
                    }
                    else if (Projectile.velocity.Y < -40)
                    {
                        Projectile.velocity.Y = -40;
                    }
                }
            }
        }

        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }

                }
            }
            return closestNPC;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<PhantasmalEyeAfterImage>()].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - k * 0.1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}