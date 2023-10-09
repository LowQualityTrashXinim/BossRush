using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        /// <summary>
        /// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Position"></param>
        /// <param name="speed"></param>
        public static bool NPCMoveToPosition(this NPC npc, Vector2 Position, float speed, float roomforError = 20f)
        {
            Vector2 distance = Position - npc.Center;
            if (distance.Length() <= roomforError)
            {
                npc.velocity = Vector2.Zero;
                return true;
            }
            npc.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
            return false;
        }
        public static int NewHostileProjectile(IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int HostileProjectile = Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            Main.projectile[HostileProjectile].hostile = true;
            Main.projectile[HostileProjectile].friendly = false;
            return HostileProjectile;
        }
    }
}
