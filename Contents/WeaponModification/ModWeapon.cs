using Terraria.ID;

namespace BossRush.Contents.WeaponModification {
	public class Arrow : ModWeaponParticle {
		public override void SetDefault() {
			ProjectileType = ProjectileID.WoodenArrowFriendly;
			ProjectileDamage = 5;
			KnockBack = 1f;
			ShootSpeed = 10;
			ShootAmount = 1;
		}
	}
}
