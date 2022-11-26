using Terraria;
using Terraria.ModLoader;
using BossRush.Items.Artifact;

namespace BossRush.BuffAndDebuff
{
    internal class SecondChance : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Vampire's Protection");
            Description.SetDefault("A vampire a night keeps the grim reaper out of sight.");
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 0)
            {
                player.GetModPlayer<VampirePlayer>().CoolDown = true;
            }
        }
    }
}
