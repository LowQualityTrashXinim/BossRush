using BossRush.Common.Global;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent
{
    public class LowHealthTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.Stat;
		}
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
}
