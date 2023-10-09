using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static void SpawnBoulderOnTopPlayer(Player player, float range, bool Randomize = true)
        {
            float RandomizeX = Randomize ? Main.rand.NextFloat(-range, range) : range;
            Vector2 spawn = new Vector2(RandomizeX + player.Center.X, player.Center.Y - 1000);
            int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.Boulder, 400, 10f);
            Main.projectile[projectile].hostile = true;
            Main.projectile[projectile].friendly = false;
        }
        public static void SpawnHostileProjectile(Vector2 position, Vector2 velocity, int ProjectileType, int damage, float knockback)
        {
            int projectile = Projectile.NewProjectile(null, position, velocity, ProjectileType, damage, knockback);
            Main.projectile[projectile].hostile = true;
            Main.projectile[projectile].friendly = false;
        }
        public static int SpawnHostileProjectileDirectlyOnPlayer(Player player, float rangeX, float rangeY, bool randomizePosition, Vector2 velocity, int ProjectileType, int damage, float knockback)
        {
            float RandomizeX = randomizePosition ? Main.rand.NextFloat(-rangeX, rangeX) : rangeX;
            float RandomizeY = randomizePosition ? Main.rand.NextFloat(-rangeY, rangeY) : rangeY;
            Vector2 spawn = new Vector2(player.Center.X + RandomizeX, player.Center.Y - 1000 + RandomizeY);
            int projectile = Projectile.NewProjectile(null, spawn, velocity, ProjectileType, damage, knockback);
            Main.projectile[projectile].hostile = true;
            Main.projectile[projectile].friendly = false;
            return projectile;
        }
        public static void ProjectileDefaultDrawInfo(this Projectile projectile, out Texture2D texture, out Vector2 origin)
        {
            Main.instance.LoadProjectile(projectile.type);
            texture = TextureAssets.Projectile[projectile.type].Value;
            origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
        }
        public static void DrawTrail(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
            if (ProjectileID.Sets.TrailingMode[projectile.type] != 2)
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
            else
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.oldRot[k], origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
        }
        public static void DrawTrailWithoutColorAdjustment(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
            if (ProjectileID.Sets.TrailingMode[projectile.type] != 2)
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
            else
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
        }
        public static void DrawTrailWithColorAdjustmentHaveFade(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
            if (ProjectileID.Sets.TrailingMode[projectile.type] != 2)
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.oldRot[k], origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
            else
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                    Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.oldRot[k], origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
                }
            }
        }
    }
}
