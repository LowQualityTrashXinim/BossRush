using Terraria;
using Terraria.ModLoader;
using BossRush.Items.Accessories;

namespace BossRush.BuffAndDebuff
{
    public class FuriousCoolDown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CoolDown");
            Description.SetDefault("You are burn out");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if(player.buffTime[buffIndex] == 0)
            {
                player.GetModPlayer<FuryPlayer>().CooldownFurious = false;
            }
        }
    }
}
