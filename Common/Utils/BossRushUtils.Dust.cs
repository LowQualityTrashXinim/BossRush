using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Utils
{
    partial class BossRushUtils
    {
        public static void SpawnCircularDust(Vector2 position, int type, float speed)
        {
            int dust = Dust.NewDust(position, 0, 0, type);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Main.rand.NextVector2Circular(speed, speed);
        }
    }
}
