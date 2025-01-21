using BossRush.Common.Systems;
using BossRush.Contents.Transfixion.Arguments;
using Humanizer;
using System;
using Terraria;
using Terraria.DataStructures;

namespace BossRush.Contents.Transfixion.SoulBound;
internal class GuardianAngel : ModSoulBound {
	public override string ModifiedToolTip(Item item) {
		int level = GetLevel(item);
		return Description.FormatWith(new string[] {
			Math.Round((.35f + .05f * level) * 100).ToString(),
			Math.Round((.7f + .01f * level) * 100).ToString(),
		});
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, 1 - .7f - .01f * GetLevel(item));
		PlayerStatsHandle.SetSecondLifeCondition(player, "SB_GA", Main.rand.NextFloat() <= .35f + .05f * GetLevel(item));
	}
	public override bool PreKill(Player player, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		if (PlayerStatsHandle.GetSecondLife(player, "SB_GA")) {
			return false;
		}
		return base.PreKill(player, damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
