using BossRush.Contents.Items.SpecialReward;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem {
	internal class ClearAllNoHit : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(1, 1);
			Item.GetGlobalItem<GlobalItemHandle>().DebugItem = true;
		}
		public override bool? UseItem(Player player) {
			player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Clear();
			player.GetModPlayer<NoHitPlayerHandle>().DontHitBossNumber.Clear();
			return true;
		}
	}
}
