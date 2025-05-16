using BossRush.Common.Mode.Nightmare;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.Achievement.Hard;
public class OceanOfFortune : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 1000;
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
public class NightmareOvercome : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld();
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
