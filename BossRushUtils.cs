using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

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

        private static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance)
        {
            maxDistance *= maxDistance;
            if (Vector2.DistanceSquared(pos1, pos2) <= maxDistance)
            {
                return true;
            }
            return false;
        }
    }
}
