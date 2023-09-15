using System;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria;
using BossRush.Contents.Items;

namespace BossRush.Contents.Artifact
{
    internal class MagicalCardDeck : ModItem, IArtifactItem
    {
        public int ArtifactID => ArtifactItemID.MagicalCardDeck;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 9));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 37);
            Item.rare = ItemRarityID.Cyan;
        }
    }
}
