using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using Terraria.ID;
using BossRush.Common.RoguelikeChange;
using System;
using System.Linq;

namespace BossRush {
	public static partial class BossRushUtils {
		/// <summary>
		/// This check if player health/life is above x%
		/// </summary>
		/// <param name="player"></param>
		/// <param name="percent"></param>
		/// <returns>
		/// True if health is above or equal said percentage
		/// </returns>
		public static bool ComparePlayerHealthInPercentage(this Player player, float percent) => player.statLife >= percent * player.statLifeMax2;
		public static bool IsDebugPlayer(this Player player) =>
			player.name.Contains("Test") ||
			player.name.Contains("Debug") ||
			player.name == "LowQualityTrashXinim" ||
			player.name.Contains("#Beta");
		public static bool HasPlayerKillThisNPC(int NPCtype) => Main.BestiaryDB.FindEntryByNPCID(NPCtype).Info.Count > 0;
		public static int ActiveArtifact(this Player player) => player.GetModPlayer<ArtifactPlayer>().ActiveArtifact;
		public static bool HasArtifact<T>(this Player player)
			where T : Artifact => Artifact.GetArtifact(player.GetModPlayer<ArtifactPlayer>().ActiveArtifact) is T;
		public static int DirectionFromPlayerToNPC(float playerX, float npcX) => playerX > npcX ? -1 : 1;
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
		public static bool Player_MeteoriteArmorSet(Player player) =>
			player.head == ArmorIDs.Head.MeteorHelmet
			&& player.body == ArmorIDs.Body.MeteorSuit
			&& player.legs == ArmorIDs.Legs.MeteorLeggings;
		/// <summary>
		/// Check whenever or not is this item a weapon or not
		/// </summary>
		/// <param name="item"></param>
		/// <param name="ConsumableWeapon">Set to true if you want to allow consumable weapon</param>
		/// <returns>Return true if the item is a weapon</returns>
		public static bool IsAWeapon(this Item item, bool ConsumableWeapon = false) =>
			item.type != ItemID.None
			&& item.damage > 0
			&& item.useTime > 0
			&& item.useAnimation > 0
			&& !item.accessory
			&& item.pick == 0
			&& item.axe == 0
			&& item.hammer == 0
			&& item.ammo == AmmoID.None
			&& (item.maxStack == 1 && !ConsumableWeapon);
		public static bool IsEquipAcc(this Player player, int itemType) {
			Item[] item = new Item[9];
			Array.Copy(player.armor, 3, item, 0, 9);
			return item.Select(i => i.type).Contains(itemType);
		}
		public static bool IsAVanillaSword(int type) {
			switch (type) {
				//Sword that have even end
				case ItemID.TerraBlade:
				case ItemID.Meowmere:
				case ItemID.StarWrath:
				case ItemID.NightsEdge:
				case ItemID.TrueNightsEdge:
				case ItemID.Excalibur:
				case ItemID.TrueExcalibur:
				//WoodSword
				case ItemID.PearlwoodSword:
				case ItemID.BorealWoodSword:
				case ItemID.PalmWoodSword:
				case ItemID.ShadewoodSword:
				case ItemID.EbonwoodSword:
				case ItemID.RichMahoganySword:
				case ItemID.WoodenSword:
				case ItemID.CactusSword:
				//OrebroadSword
				case ItemID.BeeKeeper:
				case ItemID.CopperBroadsword:
				case ItemID.TinBroadsword:
				case ItemID.IronBroadsword:
				case ItemID.LeadBroadsword:
				case ItemID.SilverBroadsword:
				case ItemID.TungstenBroadsword:
				case ItemID.GoldBroadsword:
				case ItemID.PlatinumBroadsword:
				case ItemID.Flymeal:
				//LightSaber
				case ItemID.PurplePhaseblade:
				case ItemID.BluePhaseblade:
				case ItemID.GreenPhaseblade:
				case ItemID.YellowPhaseblade:
				case ItemID.OrangePhaseblade:
				case ItemID.RedPhaseblade:
				case ItemID.WhitePhaseblade:
				//Saber
				case ItemID.PurplePhasesaber:
				case ItemID.BluePhasesaber:
				case ItemID.GreenPhasesaber:
				case ItemID.YellowPhasesaber:
				case ItemID.OrangePhasesaber:
				case ItemID.RedPhasesaber:
				case ItemID.WhitePhasesaber:
				//Misc PreHM sword
				case ItemID.PurpleClubberfish:
				case ItemID.StylistKilLaKillScissorsIWish:
				case ItemID.BladeofGrass:
				case ItemID.FieryGreatsword:
				case ItemID.LightsBane:
				//HardmodeSword
				case ItemID.MythrilSword:
				case ItemID.AdamantiteSword:
				case ItemID.OrichalcumSword:
				case ItemID.TitaniumSword:
				case ItemID.TheHorsemansBlade:
				case ItemID.Bladetongue:
				case ItemID.DD2SquireDemonSword:
				//Sword That shoot projectile
				case ItemID.BeamSword:
				case ItemID.EnchantedSword:
				case ItemID.Starfury:
				case ItemID.InfluxWaver:
				case ItemID.ChlorophyteClaymore:
				case ItemID.ChlorophyteSaber:
				case ItemID.ChristmasTreeSword:
				//Poke Sword
				//Pre HM Sword
				case ItemID.DyeTradersScimitar:
				case ItemID.CandyCaneSword:
				case ItemID.Muramasa:
				case ItemID.BloodButcherer:
				case ItemID.Katana:
				case ItemID.FalconBlade:
				case ItemID.BoneSword:
				//HM sword
				case ItemID.CobaltSword:
				case ItemID.PalladiumSword:
				case ItemID.IceBlade:
				case ItemID.BreakerBlade:
				case ItemID.Frostbrand:
				case ItemID.Cutlass:
				case ItemID.Seedler:
				case ItemID.DD2SquireBetsySword:
				case ItemID.ZombieArm:
				case ItemID.BatBat:
				case ItemID.TentacleSpike:
				case ItemID.SlapHand:
				case ItemID.Keybrand:
				case ItemID.AntlionClaw:
				case ItemID.HamBat:
				case ItemID.PsychoKnife:
					return true;
				default:
					return false;
			}
		}
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
		DebuffDamage,
		SynergyDamage,
		Iframe,
		EnergyRecharge
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
		public override void PostUpdate() {
			if (!Player.ItemAnimationActive) {
				MouseLastPositionBeforeAnimation = Main.MouseWorld;
			}
		}
	}
}
