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

        public static void DrawTrail(Projectile projectile, Color lightColor)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0);
            }
        }
    }
}
