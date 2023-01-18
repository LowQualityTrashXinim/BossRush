using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush
{
    public static class BossRushUtils
    {
        public static Vector2 limitedVelocity(this Vector2 velocity, float limited)
        {
            velocity.X = Math.Clamp(velocity.X, -limited, limited);
            velocity.Y = Math.Clamp(velocity.Y, -limited, limited);
            return velocity;
        }

        public static bool reachedLimited(this Vector2 velocity, float limited)
        {
            if (Math.Abs(Math.Clamp(velocity.X, -limited, limited)) >= limited) return true;
            if (Math.Abs(Math.Clamp(velocity.Y, -limited, limited)) >= limited) return true;
            return false;
        }

        public static Vector2 Vector2DistributeEvenly(this Vector2 Vec2ToRotate, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation *.5f, rotation *-.5f, i / (ProjectileAmount - 1f)));
            }
            return Vec2ToRotate;
        }

        public static bool LookForHostileNPC(this Vector2 position,float distance)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                {
                    if (CompareSquareFloatValue(position, Main.npc[i].Center, distance)) return true;
                }
            }
            return false;
        }
        private static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance) => Vector2.DistanceSquared(pos1, pos2) <= maxDistance;
        public static void DrawTrail(Projectile projectile, Color lightColor, float ScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ScaleAccordinglyToLength, SpriteEffects.None, 0);
            }
        }
    }
}
