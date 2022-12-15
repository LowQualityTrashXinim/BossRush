using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class MindBreak : ModBuff
    {
        public override string Texture => "BossRush/BuffAndDebuff/Regen";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mind Shatter");
            Description.SetDefault("You can no longer tell left from right...");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int randomApplystate = Main.rand.Next(100);
            if (randomApplystate < 2 && !player.HasBuff(BuffID.Confused))
            {            
                int RandomDuration = Main.rand.Next(1, 60);
                player.AddBuff(BuffID.Confused, RandomDuration);
            }
        }
    }
}
