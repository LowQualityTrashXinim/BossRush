using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofPride : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.TOKENOFPRIDE;

        public int ArtifactID => 2;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}
