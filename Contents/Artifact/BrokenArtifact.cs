using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Artifact
{
    internal class BrokenArtifact : ArtifactModItem
    {
        protected override bool CanBeCraft => true;
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(32, 32, 0, 0, 10, 10, ItemUseStyleID.HoldUp, false);
            Item.material = true;
            Item.rare = 9;
        }
    }
}