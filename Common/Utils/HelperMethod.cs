using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Utils
{
    internal class HelperMethod
    {
        public static void SpawnBoulder(Player player, float range, bool Randomize = true)
        {
            float RandomizeX = Randomize ? Main.rand.NextFloat(-range, range) : range;
            Vector2 spawn = new Vector2(RandomizeX + player.Center.X, -1000 + player.Center.Y);
            int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.Boulder, 400, 10f, Main.myPlayer, 0f, 0f);
            Main.projectile[projectile].hostile = true;
            Main.projectile[projectile].friendly = false;
        }
    }
}
