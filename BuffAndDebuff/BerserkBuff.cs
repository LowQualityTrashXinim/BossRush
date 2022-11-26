using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class BerserkBuff : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berserker's Frenzy");
            Description.SetDefault("You lose ability to think clearly...");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) *= 1.5f;
            player.GetAttackSpeed(DamageClass.Melee) *= 1.5f;

            player.GetDamage(DamageClass.Ranged) *= 0.1f;
            player.GetDamage(DamageClass.Summon) *= 0.1f;
            player.GetDamage(DamageClass.Magic) *= 0.1f;
        }
    }
}
