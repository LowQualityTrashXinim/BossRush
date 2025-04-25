using BossRush.Common.Global;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent {
	public class CombatLowHPTemplate : RelicTemplate {
		public override PlayerStats StatCondition(Relic relic, Player player) {
			if (Main.rand.NextFloat() <= .25f) {
				return Main.rand.Next([PlayerStats.CritChance, PlayerStats.AttackSpeed]);
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
			if (!player.IsHealthAbovePercentage(.45f)) {
				if (stat == PlayerStats.CritChance) {
					modplayer.AddStatsToPlayer(stat, value, singularBaseMultiplier: .3f * (relic.RelicTier - 1));
				}
				else {
					modplayer.AddStatsToPlayer(stat, value, .3f * (relic.RelicTier - 1));
				}
			}
		}
	}
}
