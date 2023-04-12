using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    public class SuperShortSwordPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power of ShortSword");
            Description.SetDefault("Power of shortsword is now in your hand");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 8;
        }
    }
}
