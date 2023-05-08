using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofPride : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.TOKENOFPRIDE;
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
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = 2;
            return true;
        }
    }
}
