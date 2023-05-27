using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Artifact
{
    internal class BrokenArtifact : ModItem, IArtifactItem
    {
        
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.UseSound = SoundID.Zombie105;
            Item.rare = 9;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = -1;
            return true;
        }
    }
}
