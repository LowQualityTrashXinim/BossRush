using Terraria;
using Terraria.ModLoader;

namespace BossRush.Achievement;

class TheBeginningOfEndless : ModAchivement {
	protected override void SetDefault() {
		Type = RoguelikeAchievementID.TheBeginningOfEndless;
	}
	public override bool ConditionCheck() {
		if(ModContent.GetInstance<BossRushModSystem>().AmountOfLootboxOpenInCurrentSection() > 0) {
			return true;
		}
		return false;
	}
}

class TheFirstOfMany : ModAchivement {
	protected override void SetDefault() {
		Type = RoguelikeAchievementID.TheFirstOfMany;
	}
	public override bool ConditionCheck() {
		return NPC.downedSlimeKing;
	}
}

class AddictiveBehavior : ModAchivement {
	protected override void SetDefault() {
		Type = RoguelikeAchievementID.AddictiveBehavior;
	}
	public override bool ConditionCheck() {
		return NPC.downedSlimeKing;
	}
}
//achievementData.Add(new BossRushAchivement() {
//	Type = 1,
//	Name = "The start of addiction",
//	Description = "",
//	ConditionText = "Open 100 lootbox",
//	textureString = BossRushTexture.MISSINGTEXTURE
//});
//achievementData.Add(new BossRushAchivement() {
//	Type = 1,
//	Name = "There are many more",
//	Description = "",
//	ConditionText = "Open 1000 lootbox",
//	textureString = BossRushTexture.MISSINGTEXTURE
//});
//achievementData.Add(new BossRushAchivement() {
//	Type = 1,
//	Name = "Skill check",
//	Description = "",
//	ConditionText = "Beat a boss without getting hit",
//	textureString = BossRushTexture.MISSINGTEXTURE
//});
//achievementData.Add(new BossRushAchivement() {
//	Type = 1,
//	Name = "First success",
//	Description = "",
//	ConditionText = "Beat the mod from start to finish ( Pre boss to post moonlord )",
//	textureString = BossRushTexture.MISSINGTEXTURE
//});
