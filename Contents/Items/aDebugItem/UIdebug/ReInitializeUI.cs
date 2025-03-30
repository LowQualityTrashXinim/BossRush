using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.aDebugItem.UIdebug;
class ReInitializeUI : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(1, 1);
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().achievementUI.RemoveAllChildren();
			ModContent.GetInstance<UniversalSystem>().achievementUI.OnInitialize();
			ModContent.GetInstance<UniversalSystem>().achievementUI.Activate();
		}
		return false;
	}
}

