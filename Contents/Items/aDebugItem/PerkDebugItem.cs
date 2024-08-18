using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Perks;

namespace BossRush.Contents.Items.aDebugItem;
internal class PerkDebugItem : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.maxStack = 999;
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool? UseItem(Player player) {
		PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
		if (player.altFunctionUse != 2) {
			UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			uiSystemInstance.ActivatePerkUI(PerkUIState.DebugState);
		}
		else if (player.IsDebugPlayer()) {
			modplayer.perks.Clear();
		}
		return true;
	}
}
