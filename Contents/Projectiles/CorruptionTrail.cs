using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	internal class CorruptionTrail : ModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 100;
			Projectile.hide = true;
		}
		public override void AI() {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Corruption);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 300)) {
				Projectile.timeLeft = 100;
				Vector2 distance = npc.Center - Projectile.Center;
				float length = distance.Length();
				if (length > 1) {
					length = 1;
				}
				Projectile.velocity -= Projectile.velocity * .1f;
				Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
				return;
			}
			Projectile.velocity.Y = 1;
			Projectile.velocity -= Projectile.velocity * .01f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.CursedInferno, 120);
		}
	}
}