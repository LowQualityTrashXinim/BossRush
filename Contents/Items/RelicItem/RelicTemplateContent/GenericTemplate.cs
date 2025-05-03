using BossRush.Common.Global;
using BossRush.Contents.Perks;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;
public class GenericTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		var perkplayer = player.GetModPlayer<PerkPlayer>();
		if (perkplayer.HasPerk<BlessingOfSolar>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.MeleeDMG;
			}
		}
		else if (perkplayer.HasPerk<BlessingOfVortex>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.RangeDMG;
			}
		}
		else if (perkplayer.HasPerk<BlessingOfNebula>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return Main.rand.Next([
					PlayerStats.MagicDMG,
					PlayerStats.MaxMana,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfStardust>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.SummonDMG;
			}
			else if (Main.rand.NextFloat() <= .25f) {
				return Main.rand.Next([
					PlayerStats.MaxMinion,
					PlayerStats.MaxSentry,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfTitan>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return Main.rand.Next([
					PlayerStats.Thorn,
					PlayerStats.Defense,
					PlayerStats.MaxHP,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfSynergy>()) {
			if (Main.rand.NextFloat() <= .5f) {
				return PlayerStats.SynergyDamage;
			}
		}
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,

			PlayerStats.RegenHP,
			PlayerStats.RegenMana,
			PlayerStats.MaxHP,
			PlayerStats.MaxMana,
			PlayerStats.Defense,

			PlayerStats.MaxMinion,
			PlayerStats.MaxSentry,

			PlayerStats.EnergyCap,

			PlayerStats.MovementSpeed,
			PlayerStats.JumpBoost,

			PlayerStats.Thorn,

			PlayerStats.DebuffDamage,
			PlayerStats.FullHPDamage,
			PlayerStats.SynergyDamage
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		float multiValue = 0;
		if (stat == PlayerStats.DebuffDamage) {
			multiValue = .1f * (relic.RelicTier - 1);
		}
		else if (stat == PlayerStats.SynergyDamage) {
			multiValue = .12f * (relic.RelicTier - 1);
		}
		else if (stat == PlayerStats.FullHPDamage) {
			multiValue = .5f * (relic.RelicTier - 1);
		}
		else {
			multiValue = .25f * (relic.RelicTier - 1);
		}
		if (stat == PlayerStats.Defense
			|| stat == PlayerStats.MaxMana
			|| stat == PlayerStats.MaxHP
			|| stat == PlayerStats.RegenMana
			|| stat == PlayerStats.RegenHP
			|| stat == PlayerStats.MaxMinion
			|| stat == PlayerStats.MaxSentry
			|| stat == PlayerStats.EnergyCap
			|| stat == PlayerStats.Thorn
			) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base + (value.Base - 1) * multiValue);
		}
		else if (stat == PlayerStats.SynergyDamage) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Flat + (value.Flat - 1) * multiValue);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * multiValue);
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring,]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.JumpBoost || stat == PlayerStats.MovementSpeed) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.15f), 2), 1);
		}
		if (stat == PlayerStats.MaxMinion || stat == PlayerStats.MaxSentry) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 3));
		}
		if (stat == PlayerStats.RegenHP || stat == PlayerStats.RegenMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 3);
		}
		if (stat == PlayerStats.MaxHP || stat == PlayerStats.MaxMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 10);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5));
		}
		if (stat == PlayerStats.MeleeDMG
			|| stat == PlayerStats.RangeDMG
			|| stat == PlayerStats.MagicDMG
			|| stat == PlayerStats.SummonDMG) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.1f), 2), 1);
		}
		if (stat == PlayerStats.EnergyCap) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 6) * 20);
		}
		if (stat == PlayerStats.Thorn) {
			return new StatModifier(1, 1, 0, Main.rand.Next(10, 40));
		}
		if (stat == PlayerStats.DebuffDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(.1f, .3f) + 1, 2), 1, 0, 0);
		}
		if (stat == PlayerStats.FullHPDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(.75f, 1f) + 1, 2), 1, 0, 0);
		}
		if (stat == PlayerStats.SynergyDamage) {
			return new StatModifier(1, 1, Main.rand.Next(3, 11), 0);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		float multiValue = 0;
		if (stat == PlayerStats.DebuffDamage) {
			multiValue = .1f * (relic.RelicTier - 1);
		}
		else if (stat == PlayerStats.SynergyDamage) {
			multiValue = .12f * (relic.RelicTier - 1);
		}
		else if (stat == PlayerStats.FullHPDamage) {
			multiValue = .5f * (relic.RelicTier - 1);
		}
		else if (stat == PlayerStats.SynergyDamage) {
			float tierValue = .12f * (relic.RelicTier - 1);
			value.Flat += value.Flat * tierValue;
			modplayer.AddStatsToPlayer(stat, value);
			return;
		}
		else {
			multiValue = .25f * (relic.RelicTier - 1);
		}
		modplayer.AddStatsToPlayer(stat, value, multiValue);
	}
}
