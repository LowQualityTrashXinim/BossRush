using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    public class Regen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Romantic Regeneration");
            Description.SetDefault("The Power of Love is by your side!");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 5;
        }
    }
}
