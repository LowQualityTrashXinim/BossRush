using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class SageBuff : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Sage's Overflow");
            Description.SetDefault("You feel as though the mana within you is is about to burst...");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) *= 0.1f;
            player.GetDamage(DamageClass.Summon) *= 0.1f;
            player.GetDamage(DamageClass.Melee) *= 0.1f;

            player.manaCost *= 0.55f;
            player.manaRegen *= 5;
            player.statManaMax2 += 50;
            player.manaRegenBonus += 150;
            player.manaRegenDelay = (int)(player.manaRegenDelay*0.35f);
            player.manaRegenDelayBonus = (int)(player.manaRegenDelayBonus * 0.35f);
        }
    }
}
