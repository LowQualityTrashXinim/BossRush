using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class MoreItemChestDebug : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 1, 1, 0, 0, 10, 10, ItemUseStyleID.HoldUp, false);
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.GetModPlayer<DebugPlayerChest>().num--;
            }
            else
            {
                player.GetModPlayer<DebugPlayerChest>().num++;
            }
            Main.NewText("amountModifier : " + player.GetModPlayer<ChestLootDropPlayer>().amountModifier);
            return base.UseItem(player);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
    public class DebugPlayerChest : ModPlayer
    {
        public float num = 0;
        public override void PostUpdate()
        {
            Player.GetModPlayer<ChestLootDropPlayer>().amountModifier += num;
        }
    }
}
