using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class LeaderShip : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Commander's Patience");
            Description.SetDefault("Fighting alongside a horde has never been easier!");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxMinions += 5;
            player.whipRangeMultiplier *= 1.25f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.5f;

            player.GetDamage(DamageClass.Ranged) *= 0.1f;
            player.GetDamage(DamageClass.Melee) *= 0.1f;
            player.GetDamage(DamageClass.Magic) *= 0.1f;
        }
    }
}
