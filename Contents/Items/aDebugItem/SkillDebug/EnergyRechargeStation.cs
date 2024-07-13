using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class EnergyRechargeStation : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
	}
	public override void HoldItem(Player player) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.Modify_EnergyAmount(10);

	}
}
