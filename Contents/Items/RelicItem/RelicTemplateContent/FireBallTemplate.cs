﻿using BossRush.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;
public class FireBallTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_FireBall", new(
			400,
			Vector2.Zero,
			false,
			Color.OrangeRed
			));
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new(1, 1, 0, 40 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
				Color.Orange.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_FireBall");
		if (!player.Center.LookForAnyHostileNPC(400f) || (modplayer.synchronize_Counter - 10) % 90 != 0) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center,
				Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) * 3,
				ProjectileID.BallofFire,
				(int)(value.Base * (1 + .1f * Tier + 1)),
				4 + .5f * Tier,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(400);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
		}
	}
}
