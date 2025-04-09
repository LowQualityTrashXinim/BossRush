using BossRush.Common.Global;
using BossRush.Contents.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent {
	public class SkillActivationTemplate : RelicTemplate {
		public override PlayerStats StatCondition(Relic relic, Player player) {
			return Main.rand.Next([
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.CritDamage,
			PlayerStats.AttackSpeed,
			PlayerStats.Defense,
		]);
		}
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			string Number = stat == PlayerStats.CritChance ? RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * ((relic.RelicTier - 1) / 3f)) : RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * ((relic.RelicTier - 1) / 3f));
			return string.Format(Description, [Color.Yellow.Hex3(), Name, Number]);
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
				float additive;
				if (stat == PlayerStats.CritChance) {
					additive = MathF.Round(value.Base * (relic.RelicTier - 1) / 3f);
					modplayer.AddStatsToPlayer(stat, Base: additive);
				}
				else {
					modplayer.AddStatsToPlayer(stat, value, (relic.RelicTier - 1) / 3f);
				}
			}
		}
	}
}
