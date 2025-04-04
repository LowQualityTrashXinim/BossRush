using BossRush.Common.Global;
using BossRush.Common.Mode.HellishEndeavour;
using BossRush.Common.Mode.Nightmare;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.NPCs;
using BossRush.Contents.Transfixion.Artifacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Tutorial;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 0;
	}
	public override void SpecialEffectOnAchieved() {
		Projectile.NewProjectile(null, Main.LocalPlayer.Center, -Vector2.UnitY, ProjectileID.FireworkFountainRainbow, 0, 0);
	}
}

public class TheFirstOfMany : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Tutorial;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod();
	}
}

public class BountifulHarvest : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 100;
	}
}

public class OceanOfFortune : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 1000;
	}
}

public class SkillCheck : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}

public class TokenOfGreed : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TokenOfGreedArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>() && UniversalSystem.NotNormalMode();
	}
}

public class TokenOfPride : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override void Draw(UIElement element, SpriteBatch spriteBatch) {
		Artifact.GetArtifact(Artifact.ArtifactType<TokenOfPrideArtifact>()).DrawInUI(spriteBatch, element.GetDimensions());
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfWrath : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfSloth : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfSlothArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfGluttony : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class BootOfSpeedManipulation : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<BootsOfSpeedManipulationArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<BootsOfSpeedManipulationArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class VampirismCrystal : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool SpecialDraw => true;
	public override void Draw(UIElement element, SpriteBatch spriteBatch) {
		Artifact.GetArtifact(Artifact.ArtifactType<VampirismCrystalArtifact>()).DrawInUI(spriteBatch, element.GetDimensions());
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<VampirismCrystalArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class HeartOfEarth : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<HeartOfEarthArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class GamblerSoul : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<GamblerSoulArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class ManaOverloader : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<ManaOverloaderArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class EssenceLantern : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<EssenceLanternArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class AlchemistKnowledge : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class LordOfLootBox : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override bool Condition() {
		return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(ModContent.NPCType<LootBoxLord>());
	}
}
public class WeaponChallenge1 : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Challenge;
	}
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
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld();
	}
}
public class TrueNightmare : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Mastery;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld() && Main.masterMode && Main.getGoodWorld;
	}
}
public class GodOfChallenge : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Mastery;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod()
			&& HellishEndeavorSystem.Hellish()
			&& (Main.expertMode || Main.masterMode);
	}
}
public class Elite : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		if (Main.CurrentPlayer.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			return modplayer.EliteKillCount > 0;
		}
		return false;
	}
}
public class SpeedRunner : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 20 && UniversalSystem.DidPlayerBeatTheMod();
		}
		return false;
	}
}
public class BossRushRunnerI : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.BossRush;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 10 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
		}
		return false;
	}
}
public class BossRushRunnerII : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.BossRush;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 5 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
		}
		return false;
	}
}
public class StraightForTheWall : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh) || ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFleshEye) && ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count <= 1;
	}
}
