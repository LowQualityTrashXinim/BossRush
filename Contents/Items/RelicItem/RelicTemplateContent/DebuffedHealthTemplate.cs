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
	public class DebuffedHealthTemplate : RelicTemplate {
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
}
