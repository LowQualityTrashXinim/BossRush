using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

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
            return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != 0;
        }
        public override bool? UseItem(Player player)
        {
            ArtifactPlayerHandleLogic modplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
            modplayer.ArtifactDefinedID = 0;
            return true;
        }
    }
}