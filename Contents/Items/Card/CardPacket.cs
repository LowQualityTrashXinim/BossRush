using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Card
{
    internal class CardPacket : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 40;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if(Main.rand.NextBool(20))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<PlatinumCard>());
                return;
            }
            if(Main.rand.NextBool(10))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldCard>());
                return;
            }
            if (Main.rand.NextBool(4))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverCard>());
                return;
            }
            player.QuickSpawnItem(entitySource, ModContent.ItemType<CopperCard>());
        }
    }
    internal class BigCardPacket : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 40;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            for (int i = 0; i < 5; i++)
            {
                if (Main.rand.NextBool(20))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<PlatinumCard>());
                    continue;
                }
                if (Main.rand.NextBool(10))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldCard>());
                    continue;
                }
                if (Main.rand.NextBool(4))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverCard>());
                    continue;
                }
                player.QuickSpawnItem(entitySource, ModContent.ItemType<CopperCard>());
            }
        }
    }
}
