using System;
using Terraria;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace BossRush {
	public static partial class BossRushUtils {
		public static Vector2 LimitedVelocity(this Vector2 velocity, float limited) {
			GetAbsoluteVectorNormalized(velocity, limited, out float X, out float Y);
			velocity.X = Math.Clamp(velocity.X, -X, X);
			velocity.Y = Math.Clamp(velocity.Y, -Y, Y);
			return velocity;
		}
		public static Vector2 NextVector2RectangleEdge(this UnifiedRandom r, float RectangleWidthHalf, float RectangleHeightHalf) {
			float X = r.NextFloat(-RectangleWidthHalf, RectangleWidthHalf);
			float Y = r.NextFloat(-RectangleHeightHalf, RectangleHeightHalf);
			bool Randomdecider = r.NextBool();
			Vector2 RandomPointOnEdge = new Vector2(X * Randomdecider.ToInt(), Y * (!Randomdecider).ToInt());
			if (RandomPointOnEdge.X == 0) {
				RandomPointOnEdge.X = RectangleWidthHalf;
			}
			else {
				RandomPointOnEdge.Y = RectangleHeightHalf;
			}
			return RandomPointOnEdge * r.NextBool().ToDirectionInt();
		}
		public static Vector2 NextPointOn2Vector2(Vector2 point1, Vector2 point2) {
			float length = Vector2.Distance(point1, point2);
			return point1.PositionOFFSET(point2 - point1, Main.rand.NextFloat(length));
		}
		public static bool Vector2WithinRectangle(this Vector2 position, float X, float Y, Vector2 Center) {
			Vector2 positionNeedCheck1 = new Vector2(Center.X + X, Center.Y + Y);
			Vector2 positionNeedCheck2 = new Vector2(Center.X - X, Center.Y - Y);
			if (position.X < positionNeedCheck1.X && position.X > positionNeedCheck2.X && position.Y < positionNeedCheck1.Y && position.Y > positionNeedCheck2.Y) { return true; }//higher = -Y, lower = Y
			return false;
		}
		public static bool Vector2TouchLine(float pos1, float pos2, float CenterY) => pos1 < (CenterY + pos2) && pos1 > (CenterY - pos2);
		public static bool IsLimitReached(this Vector2 velocity, float limited) => !(velocity.X < limited && velocity.X > -limited && velocity.Y < limited && velocity.Y > -limited);
		public static void GetAbsoluteVectorNormalized(Vector2 velocity, float limit, out float X, out float Y) {
			Vector2 newVelocity = velocity.SafeNormalize(Vector2.Zero) * limit;
			X = Math.Abs(newVelocity.X);
			Y = Math.Abs(newVelocity.Y);
		}
		public static Vector2 Vector2DistributeEvenly(this Vector2 vec, float ProjectileAmount, float rotation, float i) {
			if (ProjectileAmount > 1) {
				rotation = MathHelper.ToRadians(rotation);
				return vec.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / ProjectileAmount));
			}
			return vec;
		}
		public static Vector2 Vector2DistributeEvenlyPlus(this Vector2 vec, float ProjectileAmount, float rotation, float i) {
			if (ProjectileAmount > 1) {
				rotation = MathHelper.ToRadians(rotation);
				return vec.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / (ProjectileAmount - 1f)));
			}
			return vec;
		}
		public static Vector2 Vector2RotateByRandom(this Vector2 Vec2ToRotate, float ToRadians) => Vec2ToRotate.RotatedByRandom(MathHelper.ToRadians(ToRadians));
		/// <summary>
		/// Only use this if you know the projectile can get spawn into a tile<br/>
		/// </summary>
		/// <param name="positionCurrent"></param>
		/// <param name="positionTo"></param>
		/// <returns></returns>
		public static Vector2 SpawnRanPositionThatIsNotIntoTile(Vector2 positionCurrent, float halfwidth, float halfheight, float rotation = 0) {
			int counter = 0;
			Vector2 pos;
			do {
				counter++;
				pos = positionCurrent + Main.rand.NextVector2Circular(halfwidth, halfheight).RotatedBy(rotation);
			} while (!Collision.CanHitLine(positionCurrent, 0, 0, pos, 0, 0) || counter < 50);
			return pos;
		}
		public static bool IsCloseToPosition(this Vector2 CurrentPosition, Vector2 Position, float distance) => Vector2.DistanceSquared(CurrentPosition, Position) <= distance * distance;
		/// <summary>
		/// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="Position"></param>
		/// <param name="speed"></param>
		public static bool ProjectileMoveToPosition(this Projectile projectile, Vector2 Position, float speed) {
			Vector2 distance = Position - projectile.Center;
			if (distance.Length() <= 20f) {
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
		public static bool ProjectileMoveToPosition(this Projectile projectile, Vector2 Position, float speed, float roomforError = 10) {
			Vector2 distance = Position - projectile.Center;
			if (distance.Length() <= roomforError) {
				projectile.Center = Position;
				projectile.velocity = Vector2.Zero;
				return true;
			}
			projectile.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
			return false;
		}
		/// <summary>
		/// The higher the number, the heavier this method become, NOT RECOMMEND USING IT AT ALL COST
		/// </summary>
		/// <param name="position"></param>
		/// <param name="ProjectileVelocity"></param>
		/// <param name="offSetBy"></param>
		/// <param name="accurancyCheck">off set this by 1, since the starting accurancy check is 1</param>
		/// <returns></returns>
		public static Vector2 PositionOffsetDynamic(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy, int width1 = 0, int height1 = 0, int accurancyCheck = 0) {
			Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero);
			for (float i = offSetBy; i > 0; i--) {
				if (Collision.CanHitLine(position, 16, 16, position + OFFSET * i, width1, height1)) {
					return position += OFFSET * i;
				}
				i -= accurancyCheck;
			}
			return position;
		}
		public static Vector2 PositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy) {
			Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
			if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0)) {
				return position += OFFSET;
			}
			return position;
		}
		public static Vector2 IgnoreTilePositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy) {
			Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
			return position += OFFSET;
		}
		public static Vector2 Vector2RandomSpread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1) {
			ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
			ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
			return ToRotateAgain;
		}
	}
}
