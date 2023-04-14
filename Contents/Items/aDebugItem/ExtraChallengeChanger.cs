using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.ExtraChallenge;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class ExtraChallengeChanger : ModItem
    {
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 1, 1, 0, 0, 10, 10, ItemUseStyleID.HoldUp, false);
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ExtraChallengePlayer>().ChallengeChooser++;
            
            return base.UseItem(player);
        }
    }
}
