using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class Protection : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protection");
            Description.SetDefault("Absolute defense, shit damage and movement speed");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.45f;
            player.statDefense += 100;

            player.GetDamage(DamageClass.Generic) *= 0.25f;

            player.moveSpeed *= 0.5f;
            player.maxRunSpeed = 0.5f;
            player.runAcceleration *= 0.5f;
            player.jumpSpeedBoost *= 0.5f;
            player.accRunSpeed *= 0.5f;
        }
    }
}
