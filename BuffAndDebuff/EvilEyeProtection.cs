using Terraria;
using Terraria.ModLoader;
using BossRush.Items.Accessories.EnragedBossAccessories.EvilEye;

namespace BossRush.BuffAndDebuff
{
    internal class EvilEyeProtection : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Eye's Gaze");
            Description.SetDefault("The everwatching presence's protection is overflowing within you...");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<EvilEyePlayer>().EyeProtection = false;
        }
    }
}
