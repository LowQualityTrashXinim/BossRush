using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.ModLoader;

namespace BossRush.Common.YouLikeToHurtYourself
{
    internal class MasochistSystem : ModSystem
    {
        public override string WorldCanBePlayedRejectionMessage(PlayerFileData playerData, WorldFileData worldData)
        {
            if (ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself && worldData.GameMode != 3 && !worldData.ForTheWorthy)
            {
                return "A force of pain block you from cheesing" +
                    "\nokay, with the stupid cringe edgy line out of the way, you must play on Master difficulty world";
            }
            return base.WorldCanBePlayedRejectionMessage(playerData, worldData);
        }
        public override bool CanWorldBePlayed(PlayerFileData playerData, WorldFileData worldFileData)
        {
            if (ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself)
            {
                if (worldFileData.GameMode != 3)
                    return false;
                if (!worldFileData.ForTheWorthy)
                    return false;
                return true;
            }
            return true;
        }
    }
}