using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (++Projectile.ai[0] > 30) {
				Projectile.netUpdate = true;
				if (Projectile.velocity.Y < 20) {
					Projectile.velocity.Y += 0.3f;
				}
			}
			if (Projectile.ai[0] % 5 == 0) {
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.t_Slime);
				dust.noGravity = true;
				dust.color = new(100, 200, 255);
				dust.fadeIn = 1f;
				dust.velocity = Vector2.Zero;
				dust.scale = Main.rand.NextFloat(.7f, 1.1f);
			}
		}
	}
}

