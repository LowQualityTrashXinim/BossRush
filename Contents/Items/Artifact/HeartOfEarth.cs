using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class HeartOfEarth : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;

        public int ArtifactID => 4;

        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 58);
            Item.rare = ItemRarityID.Cyan;
        }
    }
}