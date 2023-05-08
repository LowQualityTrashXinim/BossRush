using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofGreed : ModItem, IArtifactItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = 1;
            return true;
        }
    }
}