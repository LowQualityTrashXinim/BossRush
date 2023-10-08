using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection {
	abstract class BaseBowTemplate : ModProjectile {
		public override void SetDefaults() {
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.timeLeft = 70;
			Projectile.width = 16;
			Projectile.height = 32;
		}
		public override void AI() {
			Vector2 safeAim = (Main.MouseWorld - Projectile.position).SafeNormalize(Vector2.UnitX);
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] < 19) {
				Projectile.rotation = safeAim.ToRotation();
			}
			if (Projectile.ai[0] == 30) {
				Projectile.velocity = safeAim * 15f;
			}
			if (Projectile.ai[0] > 30) {
				Projectile.rotation += 0.5f;
			}
		}
		public override void OnKill(int timeLeft) {
			float numProj = 5 + Main.rand.Next(3);
			for (int i = 0; i < numProj; i++) {
				Vector2 Star = Projectile.velocity.Vector2DistributeEvenly(numProj, 360, i);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Star, ProjectileID.WoodenArrowFriendly, (int)(Projectile.damage * 0.5f), Projectile.knockBack * .5f, Projectile.owner);
			}
		}
	}
}
