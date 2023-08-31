using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Contents.Items.Toggle
{
    internal class EternalWealth : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public int ArtifactID => ArtifactItemID.EternalWealth;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}