using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.RelicItem;

public class SynergyTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class StrikeFullHPTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class SkillDurationTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class SkillCoolDownTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class CombatV4Template : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class StaticDefeneseTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
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
public class GunFireRateTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.AttackSpeed;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, [
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage((value.Additive + (value.Additive - 1) * ( .22f * (relic.RelicTier - 1))) * value.Multiplicative),
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
