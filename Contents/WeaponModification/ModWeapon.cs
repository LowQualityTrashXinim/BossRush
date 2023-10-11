using BossRush.Contents.Projectiles;
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
	public class MagicBolt : ModWeaponParticle {
		public override void SetDefault() {
			ProjectileType = ModContent.ProjectileType<MagicboltProjectile>();
			ProjectileDamage = 12;
			KnockBack = 2f;
			ShootSpeed = 2f;
			ShootAmount = 1;
		}
		public override void ModifyCritAttack(Player player, ref int critChance, ref float critDamage) {
			critChance += 5;
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
	public class FasterCycle : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount++;
			delay -= 60;
			recharge -= 22;
		}
	}
	public class ReduceDamage : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount++;
			delay -= 22;
			recharge -= 17;
		}
		public override void ModifyAttack(Player player, ref StatModifier damage, ref float knockback, ref float shootspeed) {
			damage.Base -= 15f;
		}
	}
	//Multi cast
	public class DoubleOutput : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount += 2;
		}
	}
	public class TripleOutput : ModWeaponParticle {
		public override void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {
			castAmount += 3;
		}
	}
}
