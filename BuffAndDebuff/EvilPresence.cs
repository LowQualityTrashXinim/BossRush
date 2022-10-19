using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class EvilPresence : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Presence");
            Description.SetDefault("They are watching you");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
}
