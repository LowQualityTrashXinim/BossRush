using System;
using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Utilities;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static Vector2 LimitedVelocity(this Vector2 velocity, float limited)
        {
            velocity.GetAbsoluteVectorNormalized(limited, out float X, out float Y);
            velocity.X = Math.Clamp(velocity.X, -X, X);
            velocity.Y = Math.Clamp(velocity.Y, -Y, Y);
            return velocity;
        }
        /// <summary>
        /// Take a bool and return a int number base on true or false
        /// </summary>
        /// <param name="Num"></param>
        /// <returns>Return 1 if true
        /// <br/>Otherwise return 0</returns>
        public static int BoolZero(this bool Num) => Num ? 1 : 0;
        public static int BoolOne(this bool Num) => Num ? 1 : -1;
        public static Vector2 NextVector2RectangleEdge(this UnifiedRandom r, float RectangleWidthHalf, float RectangleHeightHalf)
        {
            float X = r.NextFloat(-RectangleWidthHalf, RectangleWidthHalf);
            float Y = r.NextFloat(-RectangleHeightHalf, RectangleHeightHalf);
            bool Randomdecider = r.NextBool();
            Vector2 RandomPointOnEdge = new Vector2(X * Randomdecider.BoolZero(), Y * (!Randomdecider).BoolZero());
            if (RandomPointOnEdge.X == 0)
            {
                RandomPointOnEdge.X = RectangleWidthHalf;
            }
            else
            {
                RandomPointOnEdge.Y = RectangleHeightHalf;
            }
            return RandomPointOnEdge * r.NextBool().BoolOne();
        }
        public static bool Vector2WithinRectangle(this Vector2 position, float X, float Y, Vector2 Center)
        {
            Vector2 positionNeedCheck1 = new Vector2(Center.X + X, Center.Y + Y);
            Vector2 positionNeedCheck2 = new Vector2(Center.X - X, Center.Y - Y);
            if (position.X < positionNeedCheck1.X && position.X > positionNeedCheck2.X && position.Y < positionNeedCheck1.Y && position.Y > positionNeedCheck2.Y)
            { return true; }//higher = -Y, lower = Y
            return false;
        }
        public static bool Vector2TouchLine(float pos1, float pos2, float CenterY)
        {
            if (pos1 < (CenterY + pos2) && pos1 > (CenterY - pos2))
            { return true; }//higher = -Y, lower = Y
            return false;
        }

        public static bool ReachedLimited(this Vector2 velocity, float limited)
        {
            velocity.GetAbsoluteVectorNormalized(limited, out float X, out float Y);
            if (Math.Abs(velocity.X) >= X) return true;
            if (Math.Abs(velocity.Y) >= Y) return true;
            return false;
        }

        public static void GetAbsoluteVectorNormalized(this Vector2 velocity, float limit, out float X, out float Y)
        {
            Vector2 newVelocity = velocity.SafeNormalize(Vector2.Zero) * limit;
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
        public static void FallingVector2(ref Vector2 position, ref Vector2 velocity, Player player, float X, float Y, float speed)
        {
            position += new Vector2(X, Y);
            velocity = (position - player.Center).SafeNormalize(Vector2.Zero) * speed;
        }
        public static Vector2 NextVector2Spread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
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
            if (t < 0.5) return InBack(t * 2) * .5f;
            return 1 - InBack((1 - t) * 2) * .5f;
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
                value1Y = pos1.Y,
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
        public static void DrawTrail(this Projectile projectile, Color lightColor, SpriteEffects spriteeffect, float ManualScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, spriteeffect, 0);
            }
        }

        public static List<int> OrderFromSmallest(this List<int> flag)
        {
            List<int> finalflag = flag;
            for (int i = 0; i < flag.Count; ++i)
            {
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (flag[i] > flag[l])
                    {
                        int CurrentIndexNum = finalflag[i];
                        finalflag[i] = flag[l];
                        finalflag[l] = CurrentIndexNum;
                    }
                }
            }
            return finalflag;
        }
        public static List<int> SetUpRNGTier(this List<int> FlagNum)
        {
            if (FlagNum.Count < 2)
            {
                return FlagNum;
            }
            List<int> FlagNumNew = new List<int> { FlagNum[0] };
            float GetOnePercentChance = 100 / (float)FlagNum.Count;
            for (int i = 1; i < FlagNum.Count; ++i)
            {
                if (Main.rand.Next(101) < GetOnePercentChance * i)
                {
                    FlagNumNew.Add(FlagNum[^i]);
                }
            }
            return FlagNumNew;
        }
        public static List<int> RemoveDupeInArray(this List<int> flag)
        {
            List<int> listArray = new List<int>();
            listArray.AddRange(flag);
            List<int> listofIndexWhereDupe = new List<int>();
            for (int i = 0; i < flag.Count; ++i)
            {
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (listArray[i] == flag[l])
                    {
                        listofIndexWhereDupe.Add(i);
                    }
                }
            }
            for (int i = listofIndexWhereDupe.Count - 1; i > -1; --i)
            {
                listArray.RemoveAt(listofIndexWhereDupe[i]);
            }
            return listArray;
        }
    }
}
