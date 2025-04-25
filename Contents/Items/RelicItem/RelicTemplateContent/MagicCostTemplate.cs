using BossRush.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent {
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
}
