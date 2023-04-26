using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class ArtifactRemover : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(0, 0, 0, 0, 10, 10, ItemUseStyleID.HoldUp, false);
        }
        public override bool? UseItem(Player player)
        {
            //for (int i = 0; i < Main.item.Length; i++)
            //{
            //    Item item = Main.item[i];
            //    if (item.ModItem is IArtifactItem && item.consumable)
            //    {

            //    }
            //}
            ArtifactPlayerHandleLogic modplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
            QualityPlayer qualityPlayer = player.GetModPlayer<QualityPlayer>();
            GreedyPlayer greedyPlayer = player.GetModPlayer<GreedyPlayer>();
            modplayer.ArtifactCount = 0;
            qualityPlayer.TokenOfPride = false;
            greedyPlayer.TokenOfGreed = false;
            return base.UseItem(player);
        }
    }
}
