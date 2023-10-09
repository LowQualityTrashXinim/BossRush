using System;
using Terraria;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static Vector2 LimitedVelocity(this Vector2 velocity, float limited)
        {
            GetAbsoluteVectorNormalized(velocity, limited, out float X, out float Y);
            velocity.X = Math.Clamp(velocity.X, -X, X);
            velocity.Y = Math.Clamp(velocity.Y, -Y, Y);
            return velocity;
        }
        public static Vector2 LimitedPosition(this Vector2 position, Vector2 position2, float limited)
        {
            position.X = Math.Clamp(position.X, -limited + position2.X, limited + position2.X);
            position.Y = Math.Clamp(position.Y, -limited + position2.Y, limited + position2.Y);
            return position;
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
        public static Vector2 NextPointOn2Vector2(Vector2 point1, Vector2 point2)
        {
            float length = Vector2.Distance(point1, point2);
            return point1.PositionOFFSET(point2 - point1, Main.rand.NextFloat(length));
        }
        public static bool Vector2WithinRectangle(this Vector2 position, float X, float Y, Vector2 Center)
        {
            Vector2 positionNeedCheck1 = new Vector2(Center.X + X, Center.Y + Y);
            Vector2 positionNeedCheck2 = new Vector2(Center.X - X, Center.Y - Y);
            if (position.X < positionNeedCheck1.X && position.X > positionNeedCheck2.X && position.Y < positionNeedCheck1.Y && position.Y > positionNeedCheck2.Y)
            { return true; }//higher = -Y, lower = Y
            return false;
        }
        public static bool Vector2TouchLine(float pos1, float pos2, float CenterY) => pos1 < (CenterY + pos2) && pos1 > (CenterY - pos2);
        public static bool IsLimitReached(this Vector2 velocity, float limited) => !(velocity.X < limited && velocity.X > -limited && velocity.Y < limited && velocity.Y > -limited);
        public static void GetAbsoluteVectorNormalized(Vector2 velocity, float limit, out float X, out float Y)
        {
            Vector2 newVelocity = velocity.SafeNormalize(Vector2.Zero) * limit;
            X = Math.Abs(newVelocity.X);
            Y = Math.Abs(newVelocity.Y);
        }
        public static Vector2 Vector2DistributeEvenly(this Vector2 vec, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return vec.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / ProjectileAmount));
            }
            return vec;
        }
        public static Vector2 Vector2DistributeEvenlyPlus(this Vector2 vec, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return vec.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / (ProjectileAmount - 1f)));
            }
            return vec;
        }
        public static Vector2 Vector2RotateByRandom(this Vector2 Vec2ToRotate, float ToRadians) => Vec2ToRotate.RotatedByRandom(MathHelper.ToRadians(ToRadians));
        public static Vector2 NextVector2Spread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            return ToRotateAgain;
        }
        /// <summary>
        /// Only use this if you know the projectile can get spawn into a tile<br/>
        /// </summary>
        /// <param name="positionCurrent"></param>
        /// <param name="positionTo"></param>
        /// <returns></returns>
        public static Vector2 SpawnRanPositionThatIsNotIntoTile(Vector2 positionCurrent, float halfwidth, float halfheight, float rotation = 0)
        {
            int counter = 0;
            Vector2 pos;
            do
            {
                counter++;
                pos = positionCurrent + Main.rand.NextVector2Circular(halfwidth, halfheight).RotatedBy(rotation);
            } while (!Collision.CanHitLine(positionCurrent, 0, 0, pos, 0, 0) || counter < 50);
            return pos;
        }
        public static bool IsCloseToPosition(this Vector2 CurrentPosition, Vector2 Position, float distance) => (Position - CurrentPosition).LengthSquared() <= distance * distance;
        /// <summary>
        /// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Position"></param>
        /// <param name="speed"></param>
        public static bool ProjectileMoveToPosition(this Projectile projectile, Vector2 Position, float speed)
        {
            Vector2 distance = Position - projectile.Center;
            if (distance.Length() <= 20f)
            {
                projectile.Center = Position;
                projectile.velocity = Vector2.Zero;
                return true;
            }
            projectile.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
            return false;
        }
        /// <summary>
        /// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Position"></param>
        /// <param name="speed"></param>
        public static bool ProjectileMoveToPosition(this Projectile projectile, Vector2 Position, float speed, float roomforError = 10)
        {
            Vector2 distance = Position - projectile.Center;
            if (distance.Length() <= roomforError)
            {
                projectile.Center = Position;
                projectile.velocity = Vector2.Zero;
                return true;
            }
            projectile.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
            return false;
        }
        public static Vector2 Vector2SmallestInList(List<Vector2> flag)
        {
            if (flag.Count == 0)
                return Vector2.Zero;

            Vector2 smallest = flag[0];
            for (int i = 1; i < flag.Count; i++)
            {
                if (flag[i].LengthSquared() < smallest.LengthSquared())
                    smallest = flag[i];
            }
            return smallest;
        }
        /// <summary>
        /// The higher the number, the heavier this method become, NOT RECOMMEND USING IT AT ALL COST
        /// </summary>
        /// <param name="position"></param>
        /// <param name="ProjectileVelocity"></param>
        /// <param name="offSetBy"></param>
        /// <returns></returns>
        public static Vector2 PositionOffsetDynamic(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero);
            for (float i = offSetBy; i > 0; i--)
            {
                if (Collision.CanHitLine(position, 0, 0, position + OFFSET * i, 0, 0))
                {
                    return position += OFFSET * i;
                }
            }
            return position;
        }
        public static Vector2 PositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
            if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
            {
                return position += OFFSET;
            }
            return position;
        }
        public static Vector2 IgnoreTilePositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
            return position += OFFSET;
        }
        public static Vector2 Vector2RandomSpread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            return ToRotateAgain;
        }
    }
}