using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.Items.aDebugItem.UIdebug;
class ReInitializeUI : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(1, 1);
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		tooltips.Add(new TooltipLine(Mod, "DebugUIInitializer", "Current UI to reintialize : Transmutation"));
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().transmutationUI.RemoveAllChildren();
			ModContent.GetInstance<UniversalSystem>().transmutationUI.OnInitialize();
			ModContent.GetInstance<UniversalSystem>().transmutationUI.Activate();
		}
		return false;
	}
}

