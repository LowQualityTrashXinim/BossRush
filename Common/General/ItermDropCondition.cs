using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Terraria.GameContent.ItemDropRules;

namespace BossRush.Common.General {
	public class ChallengeModeException : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) && UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX) || ModContent.GetInstance<RogueLikeConfig>().ForceBossDropRegadless;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Exclusive to challenge mode";
	}
	public class CheckLegacyLootboxBoss : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX) && info.npc.boss;
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "";
	}
	public class EvilBossChallengeModeException : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) || ModContent.GetInstance<RogueLikeConfig>().ForceBossDropRegadless) && NPC.downedBoss2 && info.npc.boss;
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => null;
	}
	public class QueenBeeEnranged : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.ZoneJungle;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drops if player enrage queen bee";
	}
	public class DukeIsEnrage : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.ZoneBeach;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drops if player aren't at beach";
	}
	public class DeerclopHateYou : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return Main.raining && !Main.dayTime && info.player.ZoneSnow;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player is fighting in snow biome, in night and is snowing";
	}
	public class IsNotABossAndBossIsAlive : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return BossRushUtils.IsAnyVanillaBossAlive() && !info.npc.boss;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop only when npc is not a boss and boss is alive";
	}
	public class GitGudMode : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return info.player.GetModPlayer<ModdedPlayer>().amountOfTimeGotHit == 0
					&& (
					info.player.difficulty == PlayerDifficultyID.Hardcore
					|| ModContent.GetInstance<RogueLikeConfig>().HardEnableFeature
					|| info.player.IsDebugPlayer());
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player beat boss without getting hit";
	}
	public class DontHitBoss : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.GetModPlayer<ModdedPlayer>().ItemIsUsedDuringBossFight
					&& (
					info.player.difficulty == PlayerDifficultyID.Hardcore
					|| ModContent.GetInstance<RogueLikeConfig>().HardEnableFeature
					|| info.player.IsDebugPlayer());
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player beat boss in no hit aka git gud mode";
	}
	public class NightmareMode : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModContent.GetInstance<RogueLikeConfig>().Nightmare
					|| ModContent.GetInstance<RogueLikeConfig>().HardEnableFeature;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Nightmare mode exclusive";
	}
	public class PerkDrop  : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count < 1;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "";
	}
}
