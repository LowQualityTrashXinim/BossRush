using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.ChallengeMode
{
    internal class ChallengeModeGlobalNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if(npc.type == NPCID.Nurse)
            {
                npc.StrikeInstantKill();
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                //Re add removing shop soon
            }
        }
    }
}
