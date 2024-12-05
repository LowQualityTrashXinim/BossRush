﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using Terraria.DataStructures;

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
				return Main.rand.Next(new PlayerStats[] {
					PlayerStats.MagicDMG,
					PlayerStats.MaxMana,
					PlayerStats.MaxHP,
				});
			}
		}
		else if (perkplayer.HasPerk<BlessingOfStarDust>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.SummonDMG;
			}
			else if (Main.rand.NextFloat() <= .25f) {
				return Main.rand.Next(new PlayerStats[] {
					PlayerStats.MaxMinion,
					PlayerStats.MaxSentry,
				});
			}
		}
		else if (perkplayer.HasPerk<BlessingOfTitan>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return Main.rand.Next(new PlayerStats[] {
					PlayerStats.Thorn,
					PlayerStats.Defense,
					PlayerStats.MaxHP,
				});
			}
		}
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage,

			PlayerStats.RegenHP,
			PlayerStats.RegenMana,
			PlayerStats.MaxHP,
			PlayerStats.MaxMana,
			PlayerStats.Defense,
			PlayerStats.DefenseEffectiveness,
			PlayerStats.ShieldHealth,
			PlayerStats.ShieldEffectiveness,

			PlayerStats.MaxMinion,
			PlayerStats.MaxSentry,

			PlayerStats.EnergyCap,
			PlayerStats.EnergyRechargeCap,

			PlayerStats.MovementSpeed,
			PlayerStats.JumpBoost,

			PlayerStats.Thorn
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.ShieldHealth
			|| stat == PlayerStats.Defense
			|| stat == PlayerStats.MaxMana
			|| stat == PlayerStats.MaxHP
			|| stat == PlayerStats.RegenMana
			|| stat == PlayerStats.RegenHP
			|| stat == PlayerStats.MaxMinion
			|| stat == PlayerStats.MaxSentry
			|| stat == PlayerStats.EnergyCap
			|| stat == PlayerStats.EnergyRechargeCap
			) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value);
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring, });
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
		if (stat == PlayerStats.ShieldHealth) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 10 + 100);
		}
		if (stat == PlayerStats.PureDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.01f, 1.06f), 2), 1);
		}
		if (stat == PlayerStats.MeleeDMG
			|| stat == PlayerStats.RangeDMG
			|| stat == PlayerStats.MagicDMG
			|| stat == PlayerStats.SummonDMG) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.01f, 1.11f), 2), 1);
		}
		if (stat == PlayerStats.EnergyCap) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 6) * 20);
		}
		if (stat == PlayerStats.EnergyRechargeCap) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 10));
		}
		if (stat == PlayerStats.Thorn) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.15f), 2), 1, 0, 0);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class CombatV2Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage,
			PlayerStats.CritChance
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value);
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		if (stat == PlayerStats.PureDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.15f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.2f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.ComparePlayerHealthInPercentage(.9f)) modplayer.AddStatsToPlayer(stat, value);
	}
}
public class CombatV3Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
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
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value);
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 16));
		}
		if (stat == PlayerStats.AttackSpeed) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.2f), 2), 1);
		}
		if (stat == PlayerStats.PureDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.2f), 2), 1);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.ComparePlayerHealthInPercentage(.45f)) modplayer.AddStatsToPlayer(stat, value);
	}
}
public class CombatV4Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		if (Main.rand.NextFloat() <= .25f) return Main.rand.Next(new PlayerStats[] {
			PlayerStats.PureDamage,
		});
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value), });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		var value = new StatModifier();
		if (stat == PlayerStats.PureDamage) {
			value.Base += Main.rand.Next(1, 6);
			return value;
		}
		value.Base += Main.rand.Next(5, 11);
		return value;
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class StrikeFullHPTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.FullHPDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value), });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.75f, 1.01f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class HealthV2Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.RegenHP,
			PlayerStats.Defense,
			PlayerStats.DefenseEffectiveness,
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.DefenseEffectiveness) {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value);
		}
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, valuestring });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP) {
			return new StatModifier(1, 1, 0, Main.rand.Next(3, 7) * 2);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5) * 5);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.4f, 1.85f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (!player.ComparePlayerHealthInPercentage(.35f)) {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
public class HealthV3Template : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.RegenHP,
			PlayerStats.Defense,
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value), });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.RegenHP) {
			return new StatModifier(1, 1, 0, Main.rand.Next(4, 7) * 3);
		}
		return new StatModifier(1, 1, 0, Main.rand.Next(4, 8) * 2);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				modplayer.AddStatsToPlayer(stat, value);
				break;
			}
		}
	}
}
public class DebuffDamageIncreasesTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.DebuffDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value), });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.1f, .3f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class StaticDefeneseTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.StaticDefense;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, new string[] { Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToNumber(value), });
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, Main.rand.Next(1, 13));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class MagicCostTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.MagicDMG;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Multiplicative),
			RelicTemplateLoader.RelicValueToNumber(value.Flat)
		});

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, MathF.Round(Main.rand.NextFloat(1.05f, 1.12f), 2), Main.rand.Next(3, 10), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
		player.manaCost += .15f;
	}
}
public class SynergyTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.SynergyDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToNumber(value.Flat),
			new Color(100, 255, 255).Hex3()
		});

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, Main.rand.Next(3, 11), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class GunFireRateTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.AttackSpeed;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive),
		});

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.1f, .3f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.HeldItem.useAmmo == AmmoID.Bullet)
			modplayer.AddStatsToPlayer(stat, value);
	}
}

public class ArcherMasteryTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.RangeDMG;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive),
			RelicTemplateLoader.RelicValueToNumber(value.Base),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive * 2 - 1),
		});

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .2f), 2), 1, 0, Main.rand.Next(3, 10));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.HeldItem.useAmmo == AmmoID.Arrow) {
			modplayer.AddStatsToPlayer(stat, value);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: value.Base);
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, value.Additive * 2 - 1);
		}
	}
}
public class SkillActivateTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.CritDamage,
			PlayerStats.AttackSpeed,
			PlayerStats.Defense,
		});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string Number = stat == PlayerStats.CritChance ? RelicTemplateLoader.RelicValueToNumber(value.Base) : RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		return string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			Name,
			Number
	});
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
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
public class SkillDurationTemplate : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.SkillDuration;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToNumber(value.Base / 60)
	});
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
public class DebuffTemplateV1 : RelicTemplate {
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new[] {
				PlayerStats.HealEffectiveness,
				PlayerStats.Defense,
				PlayerStats.RegenHP
			});
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, new string[] {
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToPercentage(value.Additive)
	});
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
		if (count > 3) {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
/// <summary>
/// This is my example on how to make a custom template that have your own stats<br/>
/// </summary>
public class SlimeSpikeTemplate : RelicTemplate {
	//I prefer to return PlayerStats.None because our stats doesn't exist in this mod
	//In actuality we can return whatever we want, we could also still use this to indicate what damageclass the projectile should deal
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next(new PlayerStats[] {
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		});
	}
	//We gonna use this method to hold our custom value, think of it as hacking but not the networking kind
	//Miss using this method is completely fine as the real final result is handle in Effect hook
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 10 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		//This is to get the name of the stats which we set
		string Name = Enum.GetName(stat) ?? string.Empty;
		//we use localization and our localization string look like this
		//SlimeSpikeTemplate.Description: Shoot [c/{0}:{1} friendly slime spike] dealing [c/{2}:{3}] [c/{4}:{5}] when enemy are near
		//As such it is important to format these string so that custom value can be shown on relic
		return string.Format(Description, new string[] {
				Color.Blue.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * relic.RelicTier),
				Color.Yellow.Hex3(),
				Name
		});
	}
	//This where our relic effect take place, it is think of this as a UpdateEquip hook in ModPlayer
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		//Look for any NPC near the player
		if (!player.Center.LookForAnyHostileNPC(250f) || modplayer.synchronize_Counter % 30 != 0) {
			return;
		}
		//We gonna make this template strength base from the relic Tier
		int Tier = relic.RelicTier;
		//Set damage type base on PlayerStats
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		//Spawn the projectiles base on Relic Tier
		for (int i = 0; i < Tier; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item),
				player.Center,
				Main.rand.NextVector2CircularEdge(7, 7),
				ModContent.ProjectileType<FriendlySlimeProjectile>(),
				(int)value.Base * Tier,
				2 + .5f * Tier,
				player.whoAmI);
			proj.DamageType = dmgclass;
		}
	}
}
