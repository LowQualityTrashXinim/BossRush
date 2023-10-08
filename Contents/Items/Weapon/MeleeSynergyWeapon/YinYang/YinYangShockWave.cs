using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang {
	internal class YinYangShockWave : ModProjectile {
		public override void SetDefaults() {
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 128;
			Projectile.height = 128;
		}
		public override void AI() {
			Projectile.scale += 0.2f;
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255) {
				Projectile.Kill();
			}
		}
	}
}
