using BossRush.Contents.Items;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact
{
    internal class NormalizeArtifact : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public int ArtifactID => 0;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}