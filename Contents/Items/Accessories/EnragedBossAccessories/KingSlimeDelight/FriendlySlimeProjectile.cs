using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight {
	class FriendlySlimeProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Generic;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.width = 10;
			Projectile.height = 20;
			Projectile.timeLeft = 200;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 30) {
				Projectile.netUpdate = true;
				if (Projectile.velocity.Y < 20) Projectile.velocity.Y += 0.3f;
			}
		}
	}
}

