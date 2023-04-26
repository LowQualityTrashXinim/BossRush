using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class DayTimeCycle : ModItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FastClock);
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 1, 1, 0, 0, 1, 1, ItemUseStyleID.HoldUp, false);
        }
        int count = 0;
        public override bool? UseItem(Player player)
        {
            count++;
            Main.dayTime = count % 2 == 0 ? true : false;
            return base.UseItem(player);
        }
    }
}
