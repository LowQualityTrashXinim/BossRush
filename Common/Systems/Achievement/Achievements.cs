using BossRush.Common.General;
using BossRush.Common.Mode.Nightmare;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : ModAchievement {
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 0;
	}
}

public class TheFirstOfMany : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod();
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
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TokenOfGreedArtifact>();
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
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<BootsOfSpeedManipulationArtifact>();
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
public class LordOfLootBox : ModAchievement {
	public override bool Condition() {
		return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(ModContent.NPCType<LootBoxLord>());
	}
}
public class WeaponChallenge1 : ModAchievement {
	public override bool Condition() {
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out ModdedPlayer modplayer)) {
			return UniversalSystem.DidPlayerBeatTheMod() && modplayer.UseOnly1ItemSinceTheStartOfTheGame(ItemID.CopperShortsword);
		}
		else {
			return false;
		}
	}
}
public class NightmareOvercome : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld();
	}
}
public class TrueNightmare : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld() && Main.masterMode && Main.getGoodWorld;
	}
}
public class GodOfChallenge : ModAchievement {
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() 
			&& Main.LocalPlayer.GetModPlayer<ModdedPlayer>().gitGud > 0
			&& (Main.expertMode || Main.masterMode);
	}
}
