using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Consumable.SpecialReward;
using Terraria;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : ModAchievement {
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 0;
	}
}

public class TheFirstOfMany : ModAchievement {
	public override bool Condition() {
		return NPC.downedMoonlord;
	}
}

public class BountifulHarvest : ModAchievement {
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 100;
	}
}

public class OceanOfFortune : ModAchievement {
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 1000;
	}
}

public class SkillCheck : ModAchievement {
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}

public class TokenOfGreed : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>();
	}
}

public class TokenOfPride : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>();
	}
}
public class TokenOfWrath : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>();
	}
}
public class TokenOfSloth : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfSlothArtifact>();
	}
}
public class TokenOfGluttony : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>();
	}
}
public class BootOfSpeedManipulation : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<BootsOfSpeedManipulationArtifact>();
	}
}
public class VampirismCrystal : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<VampirismCrystalArtifact>();
	}
}
public class HeartOfEarth : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>();
	}
}
public class GamblerSoul : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<GamblerSoulArtifact>();
	}
}
public class ManaOverloader : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<ManaOverloaderArtifact>();
	}
}
public class EssenceLantern : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<EssenceLanternArtifact>();
	}
}
public class AlchemistKnowledge : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>();
	}
}
