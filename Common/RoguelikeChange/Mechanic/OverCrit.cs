using BossRush.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.Mechanic;
internal class OverCrit_Player : ModPlayer {
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		int crit = Player.GetWeaponCrit(item);
		while (crit > 100) {
			crit -= 100;
			if (Main.rand.Next(1, 101) <= crit) {
				modifiers.CritDamage += 1;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		int crit = proj.CritChance;
		while (crit > 100) {
			crit -= 100;
			if (Main.rand.Next(1, 101) <= crit) {
				modifiers.CritDamage += 1;
			}
		}
	}
}
