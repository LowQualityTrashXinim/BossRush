using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    public class Regen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 5;
        }
    }
}
