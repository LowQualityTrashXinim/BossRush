using BossRush.Contents.Items.Consumable.SpecialReward;
using Terraria;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : ModAchievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen > 0;
	}
}

public class TheFirstOfMany : ModAchievement {
	public override bool Condition() {
		return NPC.downedMoonlord;
	}
}

public class BountifulHarvest : ModAchievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 100;
	}
}

public class OceanOfFortune : ModAchievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 1000;
	}
}

public class SkillCheck : ModAchievement {
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}
