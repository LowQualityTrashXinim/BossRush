using Microsoft.Xna.Framework;
using System;

namespace BossRush
{
    public static class BossRushUtils
    {
        public static Vector2 limitedVelocity(this Vector2 velocity, float limited)
        {
            Vector2 limit = new Vector2(limited,limited);
            velocity = Vector2.Clamp(velocity,-limit,limit);
            return velocity;
        }
    }
}
