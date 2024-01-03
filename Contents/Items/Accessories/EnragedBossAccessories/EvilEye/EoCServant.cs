using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye {
	class EoCServant : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Generic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 500;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (++Projectile.ai[0] > 30) {
				Projectile.netUpdate = true;
				if (Projectile.Center.LookForHostileNPC(out NPC npc, 800)) {
					Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
				}
				else {
					Projectile.velocity.Y += 0.3f;
				}
			}
		}
	}
}
