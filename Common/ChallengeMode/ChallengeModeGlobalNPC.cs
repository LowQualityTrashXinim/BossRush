using BossRush.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.ChallengeMode
{
    internal class ChallengeModeGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                for (int i = 0; i < shop.item.Length; i++)
                {
                    shop.item[i].TurnToAir();
                }
                nextSlot = 0;
            }
        }
    }
}
