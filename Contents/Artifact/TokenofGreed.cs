using BossRush.Contents.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact
{
    internal class TokenofGreed : ModItem, IArtifactItem
    {
        public int ArtifactID => ArtifactItemID.TokenOfGreed;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<EternalWealth>();
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.scale = .5f;
            Item.rare = 9;
        }
    }
}