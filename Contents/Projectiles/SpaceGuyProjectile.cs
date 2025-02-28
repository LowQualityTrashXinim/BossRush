using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	public class SpaceGuyProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1800;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 9; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
				dust.velocity = Main.rand.NextVector2Circular(3, 3);
				dust.scale = Main.rand.NextFloat(.9f, 1.2f);
				dust.noGravity = true;
			}
		}
	}
}
