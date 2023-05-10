using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Artifact
{
    internal class HeartOfEarth : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 58);
            Item.UseSound = SoundID.Roar;
            Item.rare = ItemRarityID.Cyan;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = 4;
            return true;
        }
    }
}