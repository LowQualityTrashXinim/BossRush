using BossRush.Contents.Skill;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class SkillCoolDownRemove : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.GetGlobalItem<GlobalItemHandle>().DebugItem = true;
	}
	public override void HoldItem(Player player) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.CoolDown = 0;
	}
}
