using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class SkillGetter : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
	}
	int counter = 0;
	public override bool AltFunctionUse(Player player) {
		return true;
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		ModSkill skill = SkillLoader.GetSkill(counter);
		if (skill != null) {
			tooltips.Add(new TooltipLine(Mod, "CurrentSkill", $"Current select skill to grant : {skill.DisplayName}"));
		}
		else {
			tooltips.Add(new TooltipLine(Mod, "CurrentSkill", $"Current select skill to grant : null"));
		}
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.altFunctionUse == 2) {
				counter = Math.Clamp(counter, 0, SkillLoader.TotalCount - 1);
				if(++counter == SkillLoader.TotalCount) {
					counter = 0;
				}
			}
			else {
				SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
				modplayer.RequestAddSkill_Inventory(counter);
			}
		}
		return base.UseItem(player);
	}
}
