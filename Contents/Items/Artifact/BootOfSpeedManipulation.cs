using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class BootOfSpeedManipulation : ModItem,IArtifactItem
    {
        public int ArtifactID => ArtifactItemID.BootOfSpeedManipulation;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}