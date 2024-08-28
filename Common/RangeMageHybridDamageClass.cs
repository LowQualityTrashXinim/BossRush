using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common;
internal class RangeMageHybridDamageClass : DamageClass {

	public override bool GetEffectInheritance(DamageClass damageClass) {
		return damageClass == Magic;
	}
	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
		if (damageClass == Ranged || damageClass == Magic)
			return StatInheritanceData.Full;
		return new StatInheritanceData(
			damageInheritance: 0f,
			critChanceInheritance: 0f,
			attackSpeedInheritance: 0f,
			armorPenInheritance: 0f,
			knockbackInheritance: 0f
		);
	}
}
