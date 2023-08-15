using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class FateDecider : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;

        public int ArtifactID => ArtifactItemID.FateDecider;

        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = ItemRarityID.Cyan;
        }
    }
}