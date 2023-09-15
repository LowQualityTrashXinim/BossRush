using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items;

namespace BossRush.Contents.Artifact
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