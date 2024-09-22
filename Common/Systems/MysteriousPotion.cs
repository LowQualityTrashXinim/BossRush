using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common.Systems {
	public class MysteriousPotionBuff : ModBuff {
		public static readonly Dictionary<PlayerStats, int> lookupDictionary = new Dictionary<PlayerStats, int>() {
			{ PlayerStats.Thorn, 15 },
			{ PlayerStats.MeleeDMG, 15 },
			{ PlayerStats.RangeDMG, 15 },
			{ PlayerStats.MagicDMG, 15 },
			{ PlayerStats.SummonDMG, 15 },
			{ PlayerStats.MovementSpeed, 20 },
			{ PlayerStats.JumpBoost, 20 },
			{ PlayerStats.DefenseEffectiveness, 20 },
			{ PlayerStats.CritDamage, 30 },
			{ PlayerStats.MaxMana, 20 },
			{ PlayerStats.MaxHP, 20 },
			{ PlayerStats.Defense, 8 },
			{ PlayerStats.CritChance, 8 },
			{ PlayerStats.PureDamage, 8 },
			{ PlayerStats.RegenHP, 7 },
			{ PlayerStats.RegenMana, 5 },
			{ PlayerStats.MaxMinion, 1 },
			{ PlayerStats.MaxSentry, 1 },
			{ PlayerStats.AttackSpeed, 10 },
		};
		public static PlayerStats SetStatsToAdd(MysteriousPotionPlayer modplayer) {
			var stats = new List<PlayerStats>
			{ PlayerStats.MaxSentry,
			PlayerStats.MaxMinion,
			PlayerStats.Thorn,
			PlayerStats.DefenseEffectiveness,
			PlayerStats.CritDamage,
			PlayerStats.CritChance,
			PlayerStats.PureDamage,
			PlayerStats.Defense,
			PlayerStats.RegenMana,
			PlayerStats.MaxMana,
			PlayerStats.RegenHP,
			PlayerStats.MaxHP,
			PlayerStats.MovementSpeed,
			PlayerStats.JumpBoost,
			PlayerStats.SummonDMG,
			PlayerStats.MagicDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MeleeDMG,
			PlayerStats.AttackSpeed};
			if (modplayer.Stats.Count > 0 && modplayer.Stats.Count != stats.Count) {
				foreach (var item in modplayer.Stats) {
					if (stats.Contains(item)) {
						stats.Remove(item);
					}
				}
			}
			return Main.rand.Next(stats);
		}
		/// <summary>
		/// A safer way to set Mysterious Potion buff
		/// </summary>
		/// <param name="addpoint"></param>
		/// <param name="player"></param>
		public static void SetBuff(int addpoint, int timetoadd, Player player) {
			if (!player.HasBuff<MysteriousPotionBuff>() && addpoint > 0) {
				StatsCalculation(addpoint, player, player.GetModPlayer<MysteriousPotionPlayer>());
				player.AddBuff(ModContent.BuffType<MysteriousPotionBuff>(), timetoadd);
			}
		}
		/// <summary>
		/// Use this before applying Mysterious Potion Buff effect
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public static void StatsCalculation(int addpoint, Player player, MysteriousPotionPlayer modplayer) {
			for (int i = addpoint; i != 0; i--) {
				if (i < 0) {
					modplayer.Stats.Add(SetStatsToAdd(modplayer));
					modplayer.StatsMulti.Add(i);
					break;
				}
				int pointSubstract = i != 1 ? Main.rand.Next(1, i) : 1;
				modplayer.Stats.Add(SetStatsToAdd(modplayer));
				modplayer.StatsMulti.Add(pointSubstract);
				i -= pointSubstract;
			}
			for (int i = 0; i < modplayer.Stats.Count; i++) {
				var textcolor = Color.Green;
				if (modplayer.StatsMulti[i] < 0) textcolor = Color.Red;
				BossRushUtils.CombatTextRevamp(player.Hitbox, textcolor, StatNumberAsText(modplayer, i), i * 20, 180);
			}
		}
		public static string StatNumberAsText(MysteriousPotionPlayer modplayer, int index) {
			string value = "";
			if (modplayer.StatsMulti[index] > 0) value = "+";
			if (BossRushUtils.DoesStatsRequiredWholeNumber(modplayer.Stats[index])) return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])} {modplayer.Stats[index]}";
			return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])}% {modplayer.Stats[index]}";
		}
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
			var player = Main.LocalPlayer;
			var modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
			tip = "";
			for (int i = 0; i < modplayer.Stats.Count; i++) {
				if (BossRushUtils.DoesStatsRequiredWholeNumber(modplayer.Stats[i])) {
					if (modplayer.StatsMulti[i] > 0)
						tip += $"+ {modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i])} {modplayer.Stats[i]}\n";
					else
						tip += $"- {Math.Abs(modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]))} {modplayer.Stats[i]}\n";
					continue;
				}
				if (modplayer.StatsMulti[i] > 0)
					tip += $"+ {modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i])}% {modplayer.Stats[i]}\n";
				else
					tip += $"- {Math.Abs(modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]))}% {modplayer.Stats[i]}\n";
			}
		}
		public override void Update(Player player, ref int buffIndex) {
			var modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
			var statsplayer = player.GetModPlayer<PlayerStatsHandle>();
			for (int i = 0; i < modplayer.Stats.Count; i++) switch (modplayer.Stats[i]) {
					case PlayerStats.MaxHP:
					case PlayerStats.RegenHP:
					case PlayerStats.MaxMana:
					case PlayerStats.RegenMana:
					case PlayerStats.CritChance:
					case PlayerStats.MaxMinion:
					case PlayerStats.Defense:
					case PlayerStats.MaxSentry:
						statsplayer.AddStatsToPlayer(modplayer.Stats[i], Base: modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]));
						break;
					default:
						statsplayer.AddStatsToPlayer(modplayer.Stats[i], Additive: 1 + modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]));
						break;
				}
		}
	}
	public class MysteriousPotionPlayer : ModPlayer {
		public override void Initialize() {
			Stats = new List<PlayerStats>();
			StatsMulti = new List<int>();
		}
		public List<PlayerStats> Stats = new List<PlayerStats>();
		public List<int> StatsMulti = new List<int>();
		public float ToStatsNumFloat(PlayerStats stats, int multi) => (float)Math.Round(MysteriousPotionBuff.lookupDictionary[stats] * multi * .01f, 2);
		public int ToStatsNumInt(PlayerStats stats, int multi) => MysteriousPotionBuff.lookupDictionary[stats] * multi;
		public override void ResetEffects() {
			if (!Player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>())) {
				if (Stats.Count > 0)
					Stats.Clear();
				if (StatsMulti.Count > 0)
					StatsMulti.Clear();
			}
		}
	}
}
