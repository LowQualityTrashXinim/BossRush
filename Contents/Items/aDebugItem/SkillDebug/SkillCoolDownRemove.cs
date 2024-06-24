using BossRush.Contents.Skill;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class SkillCoolDownRemove : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
	}
	public override void HoldItem(Player player) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.CoolDown = 0;
	}
}
