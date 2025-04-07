using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using BossRush.Common.Global;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;

namespace BossRush.Contents.Items.RelicItem;
public class GenericTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
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

			PlayerStats.Thorn
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
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
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base + (value.Base - 1) * (.25f * (relic.RelicTier - 1)));
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.25f * (relic.RelicTier - 1)));
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
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value, .25f * (relic.RelicTier - 1));
	}
}
public class CombatV2Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.CritChance
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * (.3f * (relic.RelicTier - 1)));
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.3f * (relic.RelicTier - 1)));
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.2f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.IsHealthAbovePercentage(.9f)) modplayer.AddStatsToPlayer(stat, value, 1 + .3f * (relic.RelicTier - 1));
	}
}
public class CombatV3Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		if (Main.rand.NextFloat() <= .25f) {
			return Main.rand.Next([
			PlayerStats.CritChance,
			PlayerStats.AttackSpeed
		]);
		}
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * (.3f * (relic.RelicTier - 1)));
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.3f * (relic.RelicTier - 1)));
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		if (stat == PlayerStats.AttackSpeed) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.2f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.2f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.IsHealthAbovePercentage(.45f)) modplayer.AddStatsToPlayer(stat, value, 1 + .3f * (relic.RelicTier - 1));
	}
}
public class CombatV4Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value.Base + relic.RelicTier - 1),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		var value = new StatModifier();
		value.Base += Main.rand.Next(1, 4);
		return value;
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		value.Base += relic.RelicTier - 1;
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class StrikeFullHPTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.FullHPDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.5f * (relic.RelicTier - 1))),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.75f, 1f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value, .5f * (relic.RelicTier - 1));
	}
}
public class HealthV2Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.RegenHP,
			PlayerStats.Defense,
			PlayerStats.DefenseEffectiveness,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		float valueMult = .2f * (relic.RelicTier - 1);
		if (stat == PlayerStats.DefenseEffectiveness) {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * valueMult);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base + (value.Additive - 1) * valueMult);
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP) {
			return new StatModifier(1, 1, 0, Main.rand.Next(4, 6) * 2);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(7, 11));
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.4f, 1.85f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.IsHealthAbovePercentage(.35f)) {
			modplayer.AddStatsToPlayer(stat, value, .2f * (relic.RelicTier - 1));
		}
	}
}
public class HealthV3Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.RegenHP,
			PlayerStats.Defense,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		value.Base += value.Base * (relic.RelicTier - 1) / 3f;
		return string.Format(Description, args: [Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value.Base)]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP) {
			return new StatModifier(1, 1, 0, Main.rand.Next(3, 5) * 3);
		}
		return new StatModifier(1, 1, 0, Main.rand.Next(4, 8) * 2);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				float additive = MathF.Round(value.Base * (1 + (relic.RelicTier - 1) / 3f));
				modplayer.AddStatsToPlayer(stat, Base: additive);
				break;
			}
		}
	}
}
public class DebuffDamageIncreasesTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.DebuffDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.1f * (relic.RelicTier - 1))),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.1f, .3f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value, .1f * (relic.RelicTier - 1));
	}
}
public class StaticDefeneseTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.StaticDefense;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		value.Base += relic.RelicTier - 1;
		return string.Format(Description, [Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToNumber(value),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, Main.rand.Next(1, 13));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		value.Base += relic.RelicTier - 1;
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class MagicCostTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.MagicDMG;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		float tierValue = .05f * (relic.RelicTier - 1);
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Multiplicative * (tierValue + 1)),
			RelicTemplateLoader.RelicValueToNumber(value.Flat + value.Flat * tierValue)
		]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, MathF.Round(Main.rand.NextFloat(1.05f, 1.12f), 2), Main.rand.Next(3, 10), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		float tierValue = .05f * (relic.RelicTier - 1);
		value.Flat += value.Flat * tierValue;
		value *= tierValue + 1;
		modplayer.AddStatsToPlayer(stat, value);
		player.manaCost += .15f;
	}
}
public class SynergyTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.SynergyDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, [
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToNumber(value.Flat + value.Flat * .12f * (relic.RelicTier - 1)),
			new Color(100, 255, 255).Hex3()
		]);

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, Main.rand.Next(3, 11), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		float tierValue = .12f * (relic.RelicTier - 1);
		value.Flat += value.Flat * tierValue;
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class GunFireRateTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.AttackSpeed;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, [
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * ( .22f * (relic.RelicTier - 1))),
		]);

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .1f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.HeldItem.useAmmo == AmmoID.Bullet) {
			float tierValue = .22f * (relic.RelicTier - 1);
			modplayer.AddStatsToPlayer(stat, value, tierValue);
		}
	}
}

public class ArcherMasteryTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.RangeDMG;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		float tierValue = 1 + .07f * (relic.RelicTier - 1);
		return string.Format(Description, [
				Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * tierValue),
			RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * tierValue),
			RelicTemplateLoader.RelicValueToPercentage((value.Additive * 2 - 1) + (value.Additive * 2 - 1) * tierValue),
		]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .2f), 2), 1, 0, Main.rand.Next(3, 10));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.HeldItem.useAmmo == AmmoID.Arrow) {
			float tierValue = .07f * (relic.RelicTier - 1);
			modplayer.AddStatsToPlayer(stat, value, tierValue);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: value.Base + value.Base * tierValue);
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, value.Additive * 2 - 1, singularAdditiveMultiplier: tierValue);
		}
	}
}
public class SkillActivateTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.CritDamage,
			PlayerStats.AttackSpeed,
			PlayerStats.Defense,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string Number = stat == PlayerStats.CritChance ? RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * ((relic.RelicTier - 1) / 3f)) : RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * ((relic.RelicTier - 1) / 3f));
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			Name,
			Number
	]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.PureDamage) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.1f, .3f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 21));
		}
		if (stat == PlayerStats.CritDamage) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.2f, .5f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.AttackSpeed) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .2f), 2), 1, 0, 0);
		}
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.1f, .35f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		SkillHandlePlayer skillPlayer = player.GetModPlayer<SkillHandlePlayer>();
		if (skillPlayer.Activate) {
			float additive;
			if (stat == PlayerStats.CritChance) {
				additive = MathF.Round(value.Base * (relic.RelicTier - 1) / 3f);
				modplayer.AddStatsToPlayer(stat, Base: additive);
			}
			else {
				modplayer.AddStatsToPlayer(stat, value, (relic.RelicTier - 1) / 3f);
			}
		}
	}
}
public class SkillDurationTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.SkillDuration;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToNumber(value.Base / 60 + relic.RelicTier - 1)
	]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, Base: value.Base + BossRushUtils.ToSecond(relic.RelicTier - 1));
	}
}
public class DebuffTemplateV1 : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
				PlayerStats.HealEffectiveness,
				PlayerStats.Defense,
				PlayerStats.RegenHP
			]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * ( (relic.RelicTier - 1) / 3f))
	]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.3f, .5f), 2), 1, 0, 0);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		int count = 0;
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				count++;
			}
		}
		if (count >= 3) {
			modplayer.AddStatsToPlayer(stat, value, (relic.RelicTier - 1) / 3f);
		}
	}
}
/// <summary>
/// This is my example on how to make a custom template that have your own stats<br/>
/// </summary>
public class SlimeSpikeTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_SlimeSpike", new(375, Vector2.Zero, false, Color.Blue));
	}
	//we can return whatever we want since this doesn't matter to what we are making,
	//however we could also still use this to indicate what damageclass the projectile should deal
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		//We are randomizing the base damage that our friendly slime spike gonna deal
		return new(1, 1, 0, 15 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		//This is to get the name of the stats which we set
		string Name = Enum.GetName(stat) ?? string.Empty;
		//we use localization and our localization string look like this
		//SlimeSpikeTemplate.Description: Shoot [c/{0}:{1} friendly slime spike] dealing [c/{2}:{3}] [c/{4}:{5}] when enemy are near
		//As such it is important to format these string so that custom value can be shown on relic
		return string.Format(Description, [
			//Terraria have a feature that allow we to add color to text, they uses hex3 for custom text coloring
				Color.Blue.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				//This is my custom method that convert float number to string
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}
	//This where our relic effect take place, it is think of this as a UpdateEquip hook in ModPlayer
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_SlimeSpike");

		//Look for any NPC near the player
		if (!player.Center.LookForAnyHostileNPC(375f) || modplayer.synchronize_Counter % 30 != 0) {
			return;
		}
		//We gonna make this template strength base from the relic Tier
		int Tier = relic.RelicTier;
		//Set damage type base on PlayerStats
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		//Spawn the projectiles base on Relic Tier
		for (int i = 0; i < Tier; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center,
				Main.rand.NextVector2CircularEdge(7, 7),
				ModContent.ProjectileType<FriendlySlimeProjectile>(),
				(int)(value.Base * (1 + .1f * (Tier - 1))),
				2 + .5f * Tier,
				player.whoAmI);
			//Setting projectile travel distance before killing
			proj.Set_ProjectileTravelDistance(375);
			//Setting projectile damage type
			proj.DamageType = dmgclass;
			//Set the projectile to ignore tile collision
			proj.tileCollide = false;
		}
	}
}
public class SkillCoolDownTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return PlayerStats.SkillCooldown;
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToNumber(value.Base / 60)
	]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, BossRushUtils.ToSecond(Main.rand.Next(1, 10)));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class FireBallTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_FireBall", new(
			400,
			Vector2.Zero,
			false,
			Color.OrangeRed
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 40 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
				Color.Orange.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_FireBall");
		if (!player.Center.LookForAnyHostileNPC(400f) || (modplayer.synchronize_Counter - 10) % 90 != 0) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center,
				Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) * 3,
				ProjectileID.BallofFire,
				(int)(value.Base * (1 + .1f * Tier + 1)),
				4 + .5f * Tier,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(400);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
		}
	}
}
public class SkyFractureTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_SkyFracture", new(
			450,
			Vector2.Zero,
			false,
			Color.Cyan
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 40 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
				Color.Cyan.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_SkyFracture");
		if ((modplayer.synchronize_Counter - 50) % 150 != 0) {
			return;
		}
		player.Center.LookForHostileNPC(out NPC npc, 450);
		if (npc == null) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Vector2 position = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(10, 14), Main.rand.NextFloat(10, 14)) * (10 + Main.rand.NextFloat(3));
			Vector2 toTarget = (npc.Center - position).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(9, 13);
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				position,
				toTarget,
				ProjectileID.SkyFracture,
				(int)(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				4 + .5f * Tier,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(450);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
		}
	}
}
public class MagicMissileTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_MagicMissile", new(
			650,
			Vector2.Zero,
			false,
			Color.LightSkyBlue
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 30 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		int cooldown = 180 - 20 * (relic.RelicTier - 1);
		return string.Format(Description, [
				Color.LightSkyBlue.Hex3(),
				(Math.Round(cooldown / 60f, 2)).ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_MagicMissile");
		int cooldown = 180 - 20 * (relic.RelicTier - 1);
		if (!player.Center.LookForAnyHostileNPC(650f) || (modplayer.synchronize_Counter - 10) % cooldown != 0) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);

		Projectile proj = Projectile.NewProjectileDirect(
			player.GetSource_ItemUse(relic.Item, Type.ToString()),
			player.Center,
			Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) * 3,
			ProjectileID.MagicMissile,
			(int)(value.Base * (1 + .1f * Tier + 1)),
			4 + .5f * Tier,
			player.whoAmI);
		proj.DamageType = dmgclass;
		proj.tileCollide = false;
		proj.Set_ProjectileTravelDistance(650);
	}
}

public class DemonScytheTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_DemonScythe", new(
			600,
			Vector2.Zero,
			false,
			Color.MediumPurple
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 54 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
				Color.MediumPurple.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_DemonScythe");
		if ((modplayer.synchronize_Counter - 50) % 120 != 0) {
			return;
		}
		player.Center.LookForHostileNPC(out NPC npc, 600);
		if (npc == null) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Vector2 pos = player.Center + Main.rand.NextVector2Circular(590, 590);
			Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.Zero);
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center + Main.rand.NextVector2Circular(590, 590),
				vel,
				ProjectileID.DemonScythe,
				(int)(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				4 + .5f * Tier,
				player.whoAmI);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
			proj.Set_ProjectileTravelDistance(600);
		}
	}
}
public class BlizzardTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_Blizzard", new(
			650,
			Vector2.Zero,
			false,
			Color.LightBlue
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 20 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		int cooldown = Math.Clamp(120 - 20 * (relic.RelicTier - 1), 10, 999);
		return string.Format(Description, [
				Color.LightSkyBlue.Hex3(),
				(Math.Round(cooldown / 60f, 2)).ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_Blizzard");
		int Tier = relic.RelicTier;
		int cooldown = Math.Clamp(120 - 20 * (Tier - 1), 10, 999);
		if (!player.Center.LookForAnyHostileNPC(650f) || (modplayer.synchronize_Counter - 10) % cooldown != 0) {
			return;
		}
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);

		Projectile proj = Projectile.NewProjectileDirect(
			player.GetSource_ItemUse(relic.Item, Type.ToString()),
			player.Center,
			Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(10, 15), Main.rand.NextFloat(10, 15)),
			ProjectileID.Blizzard,
			(int)(value.Base * (1 + .1f * Tier - 1)),
			4 + .5f * Tier,
			player.whoAmI);
		proj.DamageType = dmgclass;
		proj.tileCollide = false;
		proj.Set_ProjectileTravelDistance(650);
	}
}

public class MiniHeartStatuesTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return base.StatCondition(relic, player);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, Main.rand.Next(1, 5) * 5);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [
				Color.Green.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base),
				MathF.Round(Math.Max(600 - 60 * (relic.RelicTier - 1), 120) / 60f,2).ToString(),
				Color.Yellow.Hex3(),
		]);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (modplayer.synchronize_Counter % Math.Max(600 - 60 * (relic.RelicTier - 1), 120) == 0) {
			player.Heal((int)value.Base);
		}
	}
}
