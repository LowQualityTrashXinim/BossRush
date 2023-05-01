using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class EvilPresence : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Evil Presence");
            // Description.SetDefault("They are watching you");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}
