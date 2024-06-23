using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.aDebugItem;
internal class PerkDebugItem : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
		uiSystemInstance.perkUIstate.StateofState = PerkUIState.DebugState;
		uiSystemInstance.SetState(uiSystemInstance.perkUIstate);
		return true;
	}
}
