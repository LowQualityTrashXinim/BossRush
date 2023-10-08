using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StickySlime {
	internal class GooP : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 200;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.timeLeft <= 160) {
				if (Projectile.velocity.Y > 15f) Projectile.velocity.Y += 0.05f;
			}
		}
	}
}
