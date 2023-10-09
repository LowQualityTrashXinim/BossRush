using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class Rotting : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 10;
            player.lifeRegen -= 12;
        }
    }
}
