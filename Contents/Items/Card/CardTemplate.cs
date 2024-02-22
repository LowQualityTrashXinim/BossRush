using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Card;

public class CombatTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.DamageUniverse
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString(), });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.DamageUniverse) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.2f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class CombatV2Template : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.DamageUniverse,
			PlayerStats.CritChance
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		}
		else {
			valuestring = Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%";
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		if (stat == PlayerStats.DamageUniverse) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.15f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.35f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.ComparePlayerHealthInPercentage(.9f)) {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
public class CombatV3Template : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.DamageUniverse,
			PlayerStats.CritChance,
			PlayerStats.AttackSpeed
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		}
		else {
			valuestring = Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%";
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		if (stat == PlayerStats.AttackSpeed) {
			return new StatModifier(Main.rand.NextFloat(1.05f, 1.2f), 1);
		}
		if (stat == PlayerStats.DamageUniverse) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.35f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.35f, 1.5f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.ComparePlayerHealthInPercentage(.45f)) {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
public class HealthTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.RegenHP,
			PlayerStats.RegenMana,
			PlayerStats.MaxHP,
			PlayerStats.MaxMana,
			PlayerStats.Defense,
			PlayerStats.DefenseEffectiveness,
			PlayerStats.ShieldHealth,
			PlayerStats.ShieldEffectiveness
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.DefenseEffectiveness || stat == PlayerStats.ShieldEffectiveness) {
			valuestring = Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%";
		}
		else {
			valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP || stat == PlayerStats.RegenMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 3);
		}
		if (stat == PlayerStats.MaxHP || stat == PlayerStats.MaxMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 10);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 2);
		}
		if (stat == PlayerStats.ShieldHealth) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 10 + 100);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class HealthV2Template : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.RegenHP,
			PlayerStats.Defense,
			PlayerStats.DefenseEffectiveness,
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.DefenseEffectiveness) {
			valuestring = Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%";
		}
		else {
			valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP) {
			return new StatModifier(1, 1, 0, Main.rand.Next(3, 7) * 5);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 10);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.4f, 1.85f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.ComparePlayerHealthInPercentage(.35f)) {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
public class SummonerTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MaxMinion,
			PlayerStats.MaxSentry
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.MaxMinion || stat == PlayerStats.MaxSentry) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 3));
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
