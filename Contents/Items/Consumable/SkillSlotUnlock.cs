using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;

namespace BossRush.Contents.Items.Consumable;
internal class SkillSlotUnlock : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		return modplayer.IncreasesSkillSlot();
	}
}
