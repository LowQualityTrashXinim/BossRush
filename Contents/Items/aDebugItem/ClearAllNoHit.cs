using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.NohitReward;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class ClearAllNoHit : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(1, 1);
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Clear();
            return true;
        }
    }
}
