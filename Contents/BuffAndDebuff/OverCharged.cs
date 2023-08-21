using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class OverCharged : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;

        public override void Update(Player player, ref int buffIndex)
        {
            int dust = Dust.NewDust(player.Center + new Vector2(Main.rand.Next(-player.width, player.width)), 0, 0, DustID.Electric);
            Main.dust[dust].velocity = -player.velocity * .1f + Main.rand.NextVector2Circular(3, 3);
            player.moveSpeed += .1f;
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetAttackSpeed(DamageClass.Generic) += .1f;
        }
    }
}