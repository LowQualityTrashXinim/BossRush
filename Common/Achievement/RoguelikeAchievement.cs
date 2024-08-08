using Terraria;

namespace BossRush.Common.Achievement;

class TheBeginningOfEndless : ModAchivement {
	public override bool ConditionCheck() {
		return BossRushModSystem.roguelikedata.AmountOfLootBoxOpen > 0;
	}
}
class TheFirstOfMany : ModAchivement {
	public override bool ConditionCheck() {
		return NPC.downedMoonlord;
	}
}
class BountifulHarvest : ModAchivement {
	public override bool ConditionCheck() {
		return BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 100;
	}
}
class OceanOfFortune : ModAchivement {
	public override bool ConditionCheck() {
		return BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 1000;
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
