using System;
using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BossRush
{
    public static partial class BossRushUtils
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

        public static void getAbsoluteVectorNormalized(this Vector2 velocity, float limit, out float X, out float Y)
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

        public static bool LookForHostileNPC(this Vector2 position, out NPC npc, float distance)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && CompareSquareFloatValue(position, Main.npc[i].Center, distance))
                {
                    npc = Main.npc[i];
                    return true;
                }
            }
            npc = null;
            return false;
        }
        public static void LookForHostileNPC(this Vector2 position, out List<NPC> npc, float distance)
        {
            List<NPC> localNPC = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npcLocal = Main.npc[i];
                if (npcLocal.active && CompareSquareFloatValue(position, npcLocal.Center, distance))
                {
                    localNPC.Add(npcLocal);
                }
            }
            npc = localNPC;
        }
        public static Vector2 NextVector2RotatedByRandom(this Vector2 Vec2ToRotate, float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ToRadians);
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        public static Vector2 NextVector2Square(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier);
            return ToRotateAgain;
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
        public static float InBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }
        public static float OutBack(float t) => 1 - InBack(1 - t);
        public static float InOutBack(float t)
        {
            if (t < 0.5) return InBack(t * 2) *.5f;
            return 1 - InBack((1 - t) * 2) *.5f;
        }
        /// <summary>
        /// Calculate square length of Vector2 and check if it is smaller than square max distance
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="maxDistance"></param>
        /// <returns>
        /// Return true if length of Vector2 smaller than max distance<br/>
        /// Return false if length of Vector2 greater than max distance
        /// </returns>
        public static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance)
        {
            double value1X = pos1.X, 
                value1Y = pos1.Y ,
                value2X = pos2.X,
                value2Y = pos2.Y,
                DistanceX = value1X - value2X,
                DistanceY = value1Y - value2Y,
                maxDistanceDouble = maxDistance * maxDistance;
            return (DistanceX * DistanceX + DistanceY * DistanceY) < maxDistanceDouble;
        }
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
    }
}
