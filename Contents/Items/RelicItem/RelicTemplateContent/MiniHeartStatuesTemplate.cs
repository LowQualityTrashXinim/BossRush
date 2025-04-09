using BossRush.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;

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
