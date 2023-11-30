using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;

namespace BossRush.Contents.Items.Potion {
	internal class MysteriousPotion : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(1, 1);
			Item.maxStack = 99;
			base.SetDefaults();
		}
		public override bool CanUseItem(Player player) {
			return !player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>());
		}
		public override bool? UseItem(Player player) {
			StatsCalculation(player, player.GetModPlayer<MysteriousPotionPlayer>());
			player.AddBuff(ModContent.BuffType<MysteriousPotionBuff>(), 14400);
			return true;
		}
		public PlayerStats SetStatsToAdd(MysteriousPotionPlayer modplayer) {
			List<PlayerStats> stats = new List<PlayerStats>
			{ PlayerStats.MaxSentry,
			PlayerStats.MaxMinion,
			PlayerStats.Thorn,
			PlayerStats.DefenseEffectiveness,
			PlayerStats.CritDamage,
			PlayerStats.CritChance,
			PlayerStats.DamageUniverse,
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
			PlayerStats.MeleeDMG };
			if (modplayer.Stats.Count > 0 && modplayer.Stats.Count != stats.Count) {
				foreach (var item in modplayer.Stats) {
					if (stats.Contains(item)) {
						stats.Remove(item);
					}
				}
			}
			return Main.rand.Next(stats);
		}
		private void StatsCalculation(Player player, MysteriousPotionPlayer modplayer) {
			int potionPoint = modplayer.PotionPoint();
			if (potionPoint <= 0) {
				return;
			}
			for (int i = potionPoint; i != 0; i--) {
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
				Color textcolor = Color.Green;
				if (modplayer.StatsMulti[i] < 0) {
					textcolor = Color.Red;
				}
				BossRushUtils.CombatTextRevamp(player.Hitbox, textcolor, StatNumberAsText(modplayer, i), i * 20, 180);
			}
		}
		public static string StatNumberAsText(MysteriousPotionPlayer modplayer, int index) {
			string value = "";
			if (modplayer.StatsMulti[index] > 0) {
				value = "+";
			}
			if (BossRushUtils.DoesStatsRequiredWholeNumber(modplayer.Stats[index])) {
				return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])} {modplayer.Stats[index]}";
			}
			return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])}% {modplayer.Stats[index]}";
		}
	}
	public class MysteriousPotionBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
			Player player = Main.LocalPlayer;
			MysteriousPotionPlayer modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
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
			MysteriousPotionPlayer modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
			PlayerStatsHandle statsplayer = player.GetModPlayer<PlayerStatsHandle>();
			for (int i = 0; i < modplayer.Stats.Count; i++) {
				switch (modplayer.Stats[i]) {
					case PlayerStats.MeleeDMG:
						statsplayer.UpdateMelee += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.RangeDMG:
						statsplayer.UpdateRange += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MagicDMG:
						statsplayer.UpdateMagic += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.SummonDMG:
						statsplayer.UpdateSummon += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MovementSpeed:
						statsplayer.UpdateMovement += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.JumpBoost:
						statsplayer.UpdateJumpBoost += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MaxHP:
						statsplayer.UpdateHPMax += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.RegenHP:
						statsplayer.UpdateHPRegen += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MaxMana:
						statsplayer.UpdateManaMax += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.RegenMana:
						statsplayer.UpdateManaRegen += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.Defense:
						statsplayer.UpdateDefenseBase += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.DamageUniverse:
						statsplayer.UpdateDamagePure += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.CritChance:
						statsplayer.UpdateCritStrikeChance += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.CritDamage:
						statsplayer.UpdateCritDamage += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.DefenseEffectiveness:
						statsplayer.UpdateDefEff *= modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]) + 1;
						break;
					case PlayerStats.Thorn:
						statsplayer.UpdateThorn += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MaxMinion:
						statsplayer.UpdateMinion += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					case PlayerStats.MaxSentry:
						statsplayer.UpdateSentry += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
						break;
					default:
						break;
				}
			}
		}
	}
	public class MysteriousPotionPlayer : ModPlayer {
		public List<PlayerStats> Stats = new List<PlayerStats>();
		public List<int> StatsMulti = new List<int>();
		public int PotionPointAddition = 0;
		public int PotionPoint() => Math.Clamp(5 + PotionPointAddition, 1, 999);
		public float ToStatsNumFloat(PlayerStats stats, int multi) => lookupDictionary[stats] * multi * .01f;
		public int ToStatsNumInt(PlayerStats stats, int multi) => lookupDictionary[stats] * multi;
		public override void ResetEffects() {
			PotionPointAddition = 0;
			if (!Player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>())) {
				if (Stats.Count > 0)
					Stats.Clear();
				if (StatsMulti.Count > 0)
					StatsMulti.Clear();
			}
		}
		public Dictionary<PlayerStats, int> lookupDictionary = new Dictionary<PlayerStats, int>()
		{
			{ PlayerStats.Thorn, 15 },
			{ PlayerStats.MeleeDMG, 15 },
			{ PlayerStats.RangeDMG, 15 },
			{ PlayerStats.MagicDMG, 15 },
			{ PlayerStats.SummonDMG, 15 },
			{ PlayerStats.MovementSpeed,20 },
			{ PlayerStats.JumpBoost, 20 },
			{ PlayerStats.DefenseEffectiveness, 20 },
			{ PlayerStats.CritDamage, 30 },
			{ PlayerStats.MaxMana, 20 },
			{ PlayerStats.MaxHP, 20 },
			{ PlayerStats.Defense, 8 },
			{ PlayerStats.CritChance, 8 },
			{ PlayerStats.DamageUniverse, 8 },
			{ PlayerStats.RegenHP, 6 },
			{ PlayerStats.RegenMana, 4 },
			{ PlayerStats.MaxMinion, 1 },
			{ PlayerStats.MaxSentry, 1 },
		};
	}
}
