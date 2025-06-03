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

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;
public class MagicMissileTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Projectile;
		DataStorer.AddContext("Relic_MagicMissile", new(
			650,
			Vector2.Zero,
			false,
			Color.LightSkyBlue
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
		return new(1, 1, 0, 30 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		int cooldown = 180 - 20 * (relic.RelicTier - 1);
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
		DataStorer.ActivateContext(player, "Relic_MagicMissile");
		int cooldown = 180 - 20 * (relic.RelicTier - 1);
		if (!player.Center.LookForAnyHostileNPC(650f) || (modplayer.synchronize_Counter - 10) % cooldown != 0) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);

		Projectile proj = Projectile.NewProjectileDirect(
			player.GetSource_ItemUse(relic.Item, Type.ToString()),
			player.Center,
			Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) * 3,
			ProjectileID.MagicMissile,
			(int)(value.Base * (1 + .1f * Tier + 1) * value.Multiplicative),
			4 + .5f * Tier,
			player.whoAmI);
		proj.DamageType = dmgclass;
		proj.tileCollide = false;
		proj.Set_ProjectileTravelDistance(650);
	}
}
