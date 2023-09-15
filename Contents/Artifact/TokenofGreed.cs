using BossRush.Contents.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact
{
    internal class TokenofGreed : ArtifactModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<EternalWealth>();
        }
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = 9;
        }
    }
}