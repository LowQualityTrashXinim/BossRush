using BossRush.Contents.Items.NohitReward;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem {
	internal class ClearAllNoHit : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(1, 1);
		}
		public override bool? UseItem(Player player) {
			player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Clear();
			return true;
		}
	}
}
