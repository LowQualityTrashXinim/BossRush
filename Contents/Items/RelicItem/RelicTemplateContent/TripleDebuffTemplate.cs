using BossRush.Common.Global;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent {
	public class TripleDebuffTemplate : RelicTemplate {
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
}
