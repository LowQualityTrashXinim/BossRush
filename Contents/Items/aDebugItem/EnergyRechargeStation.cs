using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;

namespace BossRush.Contents.Items.aDebugItem;
internal class EnergyRechargeStation : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
	}
	public override void HoldItem(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.Modify_EnergyAmount(1);

	}
}
