using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;


namespace BossRush.Contents.BuffAndDebuff
{
    internal class MoonLordWrath : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Lord Wrath");
            Description.SetDefault("Your are at their mercy");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moonLeech = true;
            player.lifeRegen = 0;
            player.lifeRegenCount = 0;
            player.lifeRegenTime = 0;
        }
    }
}
