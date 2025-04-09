using BossRush.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent {
	public class AcherMasteryTemplate : RelicTemplate {
		public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.RangeDMG;
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			float tierValue = 1 + .07f * (relic.RelicTier - 1);
			return string.Format(Description, [
					Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * tierValue),
			RelicTemplateLoader.RelicValueToNumber(value.Base + value.Base * tierValue),
			RelicTemplateLoader.RelicValueToPercentage((value.Additive * 2 - 1) + (value.Additive * 2 - 1) * tierValue),
		]);
		}

		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .2f), 2), 1, 0, Main.rand.Next(3, 10));
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			if (player.HeldItem.useAmmo == AmmoID.Arrow) {
				float tierValue = .07f * (relic.RelicTier - 1);
				modplayer.AddStatsToPlayer(stat, value, tierValue);
				modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: value.Base + value.Base * tierValue);
				modplayer.AddStatsToPlayer(PlayerStats.CritDamage, value.Additive * 2 - 1, singularAdditiveMultiplier: tierValue);
			}
		}
	}
}
