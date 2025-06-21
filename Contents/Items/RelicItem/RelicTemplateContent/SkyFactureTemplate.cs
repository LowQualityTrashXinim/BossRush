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
public class SkyFractureTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Projectile;
		DataStorer.AddContext("Relic_SkyFracture", new(
			450,
			Vector2.Zero,
			false,
			Color.Cyan
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
				Color.Cyan.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1)) * value.Multiplicative),
				Color.Yellow.Hex3(),
				Name
		]);
	}

	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_SkyFracture");
		if ((modplayer.synchronize_Counter - 50) % 150 != 0) {
			return;
		}
		player.Center.LookForHostileNPC(out NPC npc, 450);
		if (npc == null) {
			return;
		}
		int Tier = relic.RelicTier;
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		for (int i = 0; i < Tier; i++) {
			Vector2 position = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(10, 14), Main.rand.NextFloat(10, 14)) * (10 + Main.rand.NextFloat(3));
			Vector2 toTarget = (npc.Center - position).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(9, 13);
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				position,
				toTarget,
				ProjectileID.SkyFracture,
				(int)(value.Base * (1 + .1f * (relic.RelicTier - 1)) * value.Multiplicative),
				4 + .5f * Tier,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(450);
			proj.DamageType = dmgclass;
			proj.tileCollide = false;
		}
	}
}
