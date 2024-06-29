using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.ID;

namespace BossRush.Contents.Items.Card;
public class CombatTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString(), });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.PureDamage) {
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
			PlayerStats.PureDamage,
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
		if (stat == PlayerStats.PureDamage) {
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
		if (Main.rand.NextFloat() <= .25f) {
			return Main.rand.Next(new PlayerStats[] {
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.AttackSpeed
		});
		}
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
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
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.2f)), 1);
		}
		if (stat == PlayerStats.PureDamage) {
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
public class CombatV4Template : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		if (Main.rand.NextFloat() <= .25f) {
			return Main.rand.Next(new PlayerStats[] {
			PlayerStats.PureDamage,
		});
		}
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		if(stat == PlayerStats.PureDamage) {
			return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString(), });
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%", });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		StatModifier value = new StatModifier();
		if (stat == PlayerStats.PureDamage) {
			value.Base += Main.rand.Next(1, 11);
			return value;
		}
		value.Base += Main.rand.Next(8,21);
		return value;
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
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
public class HealthV3Template : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.RegenHP,
			PlayerStats.Defense,
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
			return new StatModifier(1, 1, 0, Main.rand.Next(8, 11) * 5);
		}
		return new StatModifier(1, 1, 0, Main.rand.Next(4, 8) * 15);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) {
				continue;
			}
			if (Main.debuff[player.buffType[i]]) {
				modplayer.AddStatsToPlayer(stat, value);
				break;
			}
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
public class UtilityTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return PlayerStats.LootDropIncrease;
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, 1);
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring = Math.Round(value.ApplyTo(1) - 1, 2).ToString();
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class SkillTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
				PlayerStats.EnergyCap,
				PlayerStats.EnergyRechargeCap
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round(value.ApplyTo(1) - 1, 2).ToString() });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		if (stat == PlayerStats.EnergyCap) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 6) * 20);
		}
		return new StatModifier(1, 1, 0, Main.rand.Next(1, 10));
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class MovementTemplate : CardTemplate {
	public override PlayerStats StatCondition(Player player) {
		return Main.rand.Next(new PlayerStats[] {
				PlayerStats.MovementSpeed,
				PlayerStats.JumpBoost
		});
	}
	public override string ModifyToolTip(PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() });
	}
	public override StatModifier ValueCondition(Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
