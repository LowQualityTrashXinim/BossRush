using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using BossRush.Contents.Artifacts;
using Terraria.ID;
using BossRush.Common.Systems;

namespace BossRush.Common {
	public class ChallengeModeException : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) || ModContent.GetInstance<BossRushModConfig>().ForceBossDropRegadless;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Exclusive to challenge mode";
	}
	public class EvilBossChallengeModeException : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) || ModContent.GetInstance<BossRushModConfig>().ForceBossDropRegadless) && NPC.downedBoss2;
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Exclusive to challenge mode";
	}
	public class MagicalCardDeckException : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return info.player.HasArtifact<MagicalCardDeckArtifact>();
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Magical card deck call in more card";
	}
	public class SynergyDrop : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return UniversalSystem.CanAccessContent(info.player, UniversalSystem.SYNERGY_MODE);
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Exclusive to Synergy mode";
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
					|| ModContent.GetInstance<BossRushModConfig>().HardEnableFeature 
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
				return info.player.GetModPlayer<ModdedPlayer>().amountOfTimeGotHit == 0
					&& (
					info.player.difficulty == PlayerDifficultyID.Hardcore
					|| ModContent.GetInstance<BossRushModConfig>().HardEnableFeature
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
				return ModContent.GetInstance<BossRushModConfig>().Nightmare
					|| ModContent.GetInstance<BossRushModConfig>().HardEnableFeature;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Nightmare mode exclusive";
	}
	public class LifeCrystalMax : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return true;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "";
	}
	public class ManaCrystalMax : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return true;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "";
	}
}
