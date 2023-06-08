using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class BootOfSpeedManipulation : ModItem,IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public int ArtifactID => 6;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}
