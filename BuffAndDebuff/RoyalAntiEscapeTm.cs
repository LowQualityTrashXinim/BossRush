using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class RoyalAntiEscapeTm : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Sticky Formula");
            Description.SetDefault("Turns escaping into a sticky situation...");
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
