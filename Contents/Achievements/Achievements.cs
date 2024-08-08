using BossRush.Common.Systems.Achievement;
using Terraria;

namespace BossRush.Contents.Achievements;

public class TheBeginningOfEndless : Achievement {
    public override bool Condition() {
        return BossRushModSystem.roguelikedata.AmountOfLootBoxOpen > 0;
    }
}

public class TheFirstOfMany : Achievement {
    public override bool Condition() {
        return NPC.downedMoonlord;
    }
}

public class BountifulHarvest : Achievement {
    public override bool Condition() {
        return BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 100;
    }
}

public class OceanOfFortune : Achievement {
    public override bool Condition() {
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
