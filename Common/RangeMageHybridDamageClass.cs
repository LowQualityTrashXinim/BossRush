using Terraria.ModLoader;

namespace BossRush.Common;
internal class RangeMageHybridDamageClass : DamageClass {
	public override bool GetEffectInheritance(DamageClass damageClass) {
		if(damageClass == Magic || damageClass == Ranged || damageClass == Generic) {
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
