using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.ChallengeMode
{
    internal class ChallengeModeGlobalNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                //Re add removing shop soon
            }
        }
    }
}
