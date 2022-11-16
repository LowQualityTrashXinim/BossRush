using Terraria;
using Terraria.ModLoader;
using BossRush;

namespace BossRush.BuffAndDebuff
{
    internal class GodVision : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God vision");
            Description.SetDefault("Ultimate accuracy, let just hope you don't get disturb");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 70;
            GlobalWeaponModify.SpreadModify *= 0;

        }
    }
}
