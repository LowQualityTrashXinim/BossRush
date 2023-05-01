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
                for (int i = 0; i < shop.Entries.Count; i++)
                {
                    shop.GetEntry(i).Disable();
                }
            }
        }
    }
}
