using System;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria;

namespace BossRush.Contents.Items.Artifact
{
    internal class MagicalCardDeck : ModItem, IArtifactItem
    {
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
        public int ArtifactID => 7;
    }
}
