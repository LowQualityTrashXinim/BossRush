using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class Parried : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parried!");
            Description.SetDefault("10% increased damage");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            if (player.buffTime[buffIndex] == 0)
            {
                player.AddBuff(ModContent.BuffType<CoolDownParried>(), 360);
            }
        }

    }
    internal class CoolDownParried : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parrying Numbness");
            Description.SetDefault("You might want to rest for a while...");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }
    }
}
