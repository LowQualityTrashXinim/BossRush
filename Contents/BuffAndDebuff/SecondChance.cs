using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class SecondChance : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
    }
}
