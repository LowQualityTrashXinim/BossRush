using Microsoft.Xna.Framework;

namespace BossRush
{
    public static class ProjectileUtils
    {
        public static Vector2 limitedVelocity(this Vector2 velocity, float limited)
        {
            if (velocity.X > limited) velocity.X = limited;
            if (velocity.Y < -limited) velocity.X = -limited;
            if (velocity.Y > limited) velocity.Y = limited;
            if (velocity.Y < -limited) velocity.Y = -limited;
            return velocity;
        }
    }
}
