using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class ArtifactRemover : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(10, 10);
        }
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != ArtifactPlayerHandleLogic.ArtifactDefaultID;
        }
        public override bool? UseItem(Player player)
        {
            ArtifactPlayerHandleLogic modplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
            modplayer.ArtifactDefinedID = ArtifactPlayerHandleLogic.ArtifactDefaultID;
            return true;
        }
    }
}