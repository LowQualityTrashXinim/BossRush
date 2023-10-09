using Terraria;
using Terraria.ID;
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
            BossRushUtils.BossRushSetDefault(Item, 20, 20, 0, 0, 1, 1, ItemUseStyleID.HoldUp, false);
        }
        public override bool? UseItem(Player player)
        {
            Main.time = Main.dayTime ? Main.dayLength : Main.nightLength;
            return false;
        }
    }
}