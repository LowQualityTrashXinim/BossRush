using Microsoft.Xna.Framework;
using System;

namespace BossRush
{
    public static class ProjectileUtils
    {
        public static Vector2 limitedVelocity(this Vector2 velocity, float limited)
        {
            velocity.X = Math.Clamp(velocity.X,-limited,limited);
            velocity.Y = Math.Clamp(velocity.Y, -limited, limited);
            return velocity;
        }
    }
}
