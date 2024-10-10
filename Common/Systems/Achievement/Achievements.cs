using BossRush.Contents.Items.Consumable.SpecialReward;
using Terraria;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : Achievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen > 0;
	}
}

public class TheFirstOfMany : Achievement {
	public override bool Condition() {
		return NPC.downedMoonlord;
	}
}

public class BountifulHarvest : Achievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 100;
	}
}

public class OceanOfFortune : Achievement {
	public override bool Condition() {
		return false;//BossRushModSystem.roguelikedata.AmountOfLootBoxOpen >= 1000;
	}
}

public class SkillCheck : Achievement {
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}
