using BossRush.Common.Global;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;

public class BlizzardTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_Blizzard", new(
			650,
			Vector2.Zero,
			false,
			Color.LightBlue
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
		return new(1, 1, 0, 20 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		int cooldown = Math.Clamp(120 - 20 * (relic.RelicTier - 1), 10, 999);
		return string.Format(Description, [
				Color.LightSkyBlue.Hex3(),
				(Math.Round(cooldown / 60f, 2)).ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1)) * value.Multiplicative),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_Blizzard");
		int Tier = relic.RelicTier;
		int cooldown = Math.Clamp(120 - 20 * (Tier - 1), 10, 999);
		if (!player.Center.LookForAnyHostileNPC(650f) || (modplayer.synchronize_Counter - 10) % cooldown != 0) {
			return;
		}
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);

		Projectile proj = Projectile.NewProjectileDirect(
			player.GetSource_ItemUse(relic.Item, Type.ToString()),
			player.Center,
			Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(10, 15), Main.rand.NextFloat(10, 15)),
			ProjectileID.Blizzard,
			(int)(value.Base * (1 + .1f * Tier - 1) * value.Multiplicative),
			4 + .5f * Tier,
			player.whoAmI);
		proj.DamageType = dmgclass;
		proj.tileCollide = false;
		proj.Set_ProjectileTravelDistance(650);
	}
}
