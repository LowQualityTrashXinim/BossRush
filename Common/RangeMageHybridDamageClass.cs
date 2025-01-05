using Terraria.ModLoader;

namespace BossRush.Common;
internal class RangeMageHybridDamageClass : DamageClass {
	public override bool GetEffectInheritance(DamageClass damageClass) {
		if (damageClass == Magic || damageClass == Ranged || damageClass == Generic) {
			return true;
		}

		return false;
	}
	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
		if (damageClass == Ranged || damageClass == Magic || damageClass == Generic)
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
internal class MeleeMageHybridDamageClass : DamageClass {
	public override bool GetEffectInheritance(DamageClass damageClass) {
		if (damageClass == Magic || damageClass == Melee || damageClass == Generic) {
			return true;
		}

		return false;
	}
	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
		if (damageClass == Melee || damageClass == Magic || damageClass == Generic)
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
internal class MeleeRangerHybridDamageClass : DamageClass {
	public override bool GetEffectInheritance(DamageClass damageClass) {
		if (damageClass == Ranged || damageClass == Melee || damageClass == Generic) {
			return true;
		}

		return false;
	}
	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
		if (damageClass == Melee || damageClass == Ranged || damageClass == Generic)
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
