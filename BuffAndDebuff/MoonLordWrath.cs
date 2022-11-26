using Terraria;
using Terraria.ModLoader;


namespace BossRush.BuffAndDebuff
{
    internal class MoonLordWrath : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Lord's Wrath");
            Description.SetDefault("May God forgive you, because they won't...");
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
