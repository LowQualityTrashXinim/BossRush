using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.WeaponModification {
	public class Arrow : ModWeaponParticle {
		public override void SetDefault() {
			ProjectileType = ProjectileID.WoodenArrowFriendly;
			ProjectileDamage = 5;
			KnockBack = 1f;
			ShootSpeed = 10;
			ShootAmount = 1;
		}
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			delay += 10;
		}
	}
	//Modifier
	public class IncreaseDamage : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount++;
			delay += 30;
		}
		public override void ModifyAttack(Player player, ref StatModifier damage, ref float knockback, ref float shootspeed) {
			damage.Base += 10f;
		}
	}
	//Multi cast
	public class DoubleOutput : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount += 2;
		}
	}
}
