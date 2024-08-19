using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class EnergyRechargeStation : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.GetGlobalItem<GlobalItemHandle>().DebugItem = true;
	}
	public override void HoldItem(Player player) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.Modify_EnergyAmount(10);

	}
}
