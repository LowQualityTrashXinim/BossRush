using BossRush.Contents.Artifact;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Perks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items
{
    internal class BundleHelper : ModItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.MoonLordBossBag);
        public int TimeOpen = 0;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
        }
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            switch (TimeOpen)
            {
                case 0:
                    player.QuickSpawnItemDirect(player.GetSource_ItemUse(Item), ModContent.ItemType<BrokenArtifact>());
                    Item itemHelper = player.QuickSpawnItemDirect(player.GetSource_ItemUse(Item), ModContent.ItemType<BundleHelper>());
                    if (itemHelper.ModItem is BundleHelper bundle)
                    {
                        bundle.TimeOpen = 1;
                    }
                    break;
                case 1:
                    player.QuickSpawnItemDirect(player.GetSource_ItemUse(Item), ModContent.ItemType<StarterPerkChooser>());
                    Item itemHelper1 = player.QuickSpawnItemDirect(player.GetSource_ItemUse(Item), ModContent.ItemType<BundleHelper>());
                    if (itemHelper1.ModItem is BundleHelper bundle1)
                    {
                        bundle1.TimeOpen = 2;
                    }
                    break;
                case 2:
                    player.QuickSpawnItemDirect(player.GetSource_ItemUse(Item), ModContent.ItemType<WoodenLootBox>());
                    break;
                default:
                    break;
            }
        }
    }
}
