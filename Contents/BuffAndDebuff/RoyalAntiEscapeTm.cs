using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class RoyalAntiEscapeTm : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.75f;
            player.maxRunSpeed = 0.75f;
            player.runAcceleration *= 0.75f;
        }
    }
}
