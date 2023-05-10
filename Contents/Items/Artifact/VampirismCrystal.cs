using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Artifact
{
    internal class VampirismCrystal : ModItem, IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 58);
            Item.UseSound = SoundID.Roar;
            Item.rare = ItemRarityID.Cyan;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = 3;
            return true;
        }
    }
}