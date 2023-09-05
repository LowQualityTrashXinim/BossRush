using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.Spawner;

namespace BossRush.Contents.Items.Chest
{
    internal class HardModeBossBundle : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Gray;
        }
        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<BleedingWorm>());
            player.QuickSpawnItem(entitySource, ItemID.QueenSlimeCrystal);
            player.QuickSpawnItem(entitySource, ItemID.MechanicalSkull);
            player.QuickSpawnItem(entitySource, ItemID.MechanicalWorm);
            player.QuickSpawnItem(entitySource, ItemID.MechanicalEye);
        }
    }
    internal class PreHardmodeBossBundle : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Gray;
        }
        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<CursedDoll>());
            player.QuickSpawnItem(entitySource, ItemID.DeerThing);
            player.QuickSpawnItem(entitySource, ItemID.Abeemination);
            player.QuickSpawnItem(entitySource, ItemID.GoblinBattleStandard);
        }
    }
}