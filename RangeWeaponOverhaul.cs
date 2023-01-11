using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush
{
    /// <summary>
    /// This is for specific range only
    /// </summary>
    public static class RangeWeaponOverhaul
    {
        /// <summary>
        /// Use this to change how much weapon spread should be modify
        /// For global modify use multiplication
        /// For general modify use addictive
        /// Do not use SpreadModify = 0 as it will fuck the other stuff
        /// Best practice for this is to use + operator as it is what i use
        /// Using * is for globally and very hard to balance
        /// </summary>
        public static float SpreadModify;
        /// <summary>
        /// This is use to set how many projectile you can shoot in actual weapon class
        /// Do not use NumOfProjectile = 0 as it will make gun unable to shoot
        /// </summary>
        public static int NumOfProjectile = 1;
        /// <summary>
        /// This is to modify the amount of projectiles you gonna shoot from weapon that got assigned with NumOfProjectile<br/>
        /// This is the safe way to modify the amount of projectile you can modify, do keep in mind to not go below -1 as it is very dangerous
        /// </summary>
        public static float NumOfProModify = 0;
        /// <summary>
        /// Modify the ammount of projectile to be shoot
        /// </summary>
        /// <param name="TakeNumAmount">the original amount</param>
        /// <returns></returns>
        public static float ModifiedProjAmount(float TakeNumAmount)
        {
            return NumOfProModify + TakeNumAmount;
        }
        /// <summary>
        /// Modify the spread of a weapon
        /// </summary>
        /// <param name="TakeFloat">the amount to be change</param>
        /// <returns></returns>
        private static float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat * SpreadModify;

        /// <summary>
        /// Return a Vector that got evenly distribute
        /// </summary>
        /// <param name="ToRadians">The radius that it get distribute</param>
        /// <param name="time">the current progress</param>
        /// <returns></returns>
        public static Vector2 RotateCode(this Vector2 Vec2ToRotate, float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (NumOfProjectile > 1)
            {
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (NumOfProjectile - 1f)));
            }
            return Vec2ToRotate;
        }
        /// <summary>
        /// Return a random vector that got rotate randomly
        /// </summary>
        /// <param name="ToRadians">Rotate radius</param>
        /// <returns></returns>
        public static Vector2 RotateRandom(this Vector2 Vec2ToRotate,float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        /// <summary>
        /// Return a position Vector that got offset
        /// </summary>
        /// <param name="position">Original position</param>
        /// <param name="ProjectileVelocity">Current projectile velocity </param>
        /// <param name="offSetBy">Offset amount</param>
        /// <returns></returns>
        public static Vector2 PositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.UnitX) * offSetBy;
            if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
            {
                return position += OFFSET;
            }
            return position;
        }

        /// <summary>
        /// Return a vector that got its X parameter and Y parameter change randomely
        /// </summary>
        /// <param name="ToRotateAgain">The original Vector</param>
        /// <param name="Spread">Value to change speed</param>
        /// <param name="additionalMultiplier">Multiplier for final speed change</param>
        /// <returns></returns>
        public static Vector2 RandomSpread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }
    }
}
