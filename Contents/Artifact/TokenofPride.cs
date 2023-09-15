using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Contents.Items;

namespace BossRush.Contents.Artifact
{
    internal class TokenofPride : ModItem, IArtifactItem
    {
        public int ArtifactID => ArtifactItemID.TokenOfPride;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 10));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}