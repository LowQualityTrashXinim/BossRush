using Terraria;
using Terraria.ModLoader;
using BossRush.Texture;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class EvilEyeProtection : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Evil Eye Protection");
            // Description.SetDefault("live to see for another day");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<EvilEyePlayer>().EyeProtection = false;
        }
    }
}
