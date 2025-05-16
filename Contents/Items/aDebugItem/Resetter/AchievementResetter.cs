using BossRush.Common.Systems.Achievement;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem.Resetter;

class AchievementResetter : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			int achievementCount = AchievementSystem.Achievements.Count;
			for (int i = 0; i < achievementCount; i++) {
				AchievementSystem.SafeGetAchievement(i).Achieved = false;
			}
		}
		return base.UseItem(player);
	}
}
