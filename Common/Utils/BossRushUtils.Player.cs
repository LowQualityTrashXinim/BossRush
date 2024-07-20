using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush {
	public static partial class BossRushUtils {
		public static bool ComparePlayerHealthInPercentage(this Player player, float percent) => player.statLife >= percent * player.statLifeMax2;
		public static bool IsDebugPlayer(this Player player) =>
			player.name.Contains("Test") ||
			player.name.Contains("Debug") ||
			player.name == "LowQualityTrashXinim" ||
			player.name.Contains("#Beta");

		public static int ActiveArtifact(this Player player) => player.GetModPlayer<ArtifactPlayer>().ActiveArtifact;
		public static bool HasArtifact<T>(this Player player)
			where T : Artifact => Artifact.GetArtifact(player.GetModPlayer<ArtifactPlayer>().ActiveArtifact) is T;
		public static int DirectionFromPlayerToNPC(float playerX, float npcX) => playerX > npcX ? 1 : -1;
		public static bool DoesStatsRequiredWholeNumber(PlayerStats stats) =>
					stats is PlayerStats.Defense
					|| stats is PlayerStats.MaxMinion
					|| stats is PlayerStats.MaxSentry
					|| stats is PlayerStats.MaxHP
					|| stats is PlayerStats.MaxMana
					|| stats is PlayerStats.CritChance
					|| stats is PlayerStats.RegenHP
					|| stats is PlayerStats.RegenMana
					|| stats is PlayerStats.LootDropIncrease;
	}
	public enum PlayerStats : byte {
		None,
		MeleeDMG,
		RangeDMG,
		MagicDMG,
		SummonDMG,
		PureDamage,
		MovementSpeed,
		JumpBoost,
		MaxHP,
		RegenHP,
		MaxMana,
		RegenMana,
		Defense,
		CritChance,
		CritDamage,
		DefenseEffectiveness,
		LootDropIncrease,
		MaxMinion,
		MaxSentry,
		Thorn,
		ShieldHealth,
		ShieldEffectiveness,
		AttackSpeed,
		AuraRadius,
		LifeStealEffectiveness,
		MysteriousPotionEffectiveness,
		EnergyCap,
		EnergyRechargeCap,
		FullHPDamage,
		StaticDefense,
		DebuffDamage
		//Luck
	}

	/// <summary>
	/// This player class will hold additional infomation that the base Player or ModPlayer class don't provide<br/>
	/// The logic to get to those infomation is automatic <br/>
	/// It make more sense to have a modplayer file do all the logic so we don't have to worry about it when implement
	/// </summary>
	public class BossRushUtilsPlayer : ModPlayer {
		public const float PLAYERARMLENGTH = 12f;
		public Vector2 MouseLastPositionBeforeAnimation = Vector2.Zero;
		public static bool HasPlayerKillThisNPC(int NPCtype) => Main.BestiaryDB.FindEntryByNPCID(NPCtype).Info.Count > 0;
		public override void PostUpdate() {
			if (!Player.ItemAnimationActive) {
				MouseLastPositionBeforeAnimation = Main.MouseWorld;
			}
		}
	}
}
