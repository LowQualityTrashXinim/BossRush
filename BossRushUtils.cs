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
            velocity.getAbsoluteVectorNormalized(limited, out float X, out float Y);
            velocity.X = Math.Clamp(velocity.X, -X, X);
            velocity.Y = Math.Clamp(velocity.Y, -Y, Y);
            return velocity;
        }

        public static bool reachedLimited(this Vector2 velocity, float limited)
        {
            velocity.getAbsoluteVectorNormalized(limited, out float X, out float Y);
            if (Math.Abs(velocity.X) >= X) return true;
            if (Math.Abs(velocity.Y) >= Y) return true;
            return false;
        }

        private static void getAbsoluteVectorNormalized(this Vector2 velocity, float limit, out float X, out float Y)
        {
            Vector2 newVelocity = velocity.SafeNormalize(Vector2.One) * limit;
            X = Math.Abs(newVelocity.X);
            Y = Math.Abs(newVelocity.Y);
        }

        public static Vector2 Vector2DistributeEvenly(this Vector2 Vec2ToRotate, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / (ProjectileAmount - 1f)));
            }
            return Vec2ToRotate;
        }

        public static bool LookForHostileNPC(this Vector2 position, float distance)
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

        public static bool LookForClosestHostileNPC(this Vector2 position, out NPC npc, float distance)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && CompareSquareFloatValue(position, Main.npc[i].Center, distance * distance))
                {
                    npc = Main.npc[i];
                    return true;
                }
            }
            npc = null;
            return false;
        }
        public static float InExpo(float t) => (float)Math.Pow(2, 5 * (t - 1));
        public static float OutExpo(float t) => 1 - InExpo(1 - t);
        public static float InOutExpo(float t)
        {
            if (t < 0.5) return InExpo(t * 2) / 2;
            return 1 - InExpo((1 - t) * 2) / 2;
        }
        public static float InSine(float t) => (float)-Math.Cos(t * MathHelper.PiOver2);
        public static float OutSine(float t) => (float)Math.Sin(t * MathHelper.PiOver2);
        public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) * -.5f;
        private static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance) => Vector2.DistanceSquared(pos1, pos2) <= maxDistance;
        public static void DrawTrail(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
            }
        }

        public static void DrawTrail(this Projectile projectile, Color lightColor, int ScaleAccordinglyToLength, int uselessvalue = 0)
        {
            float scaleDown = 1 / ScaleAccordinglyToLength;
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * scaleDown, SpriteEffects.None, 0);
            }
        }
    }
}
