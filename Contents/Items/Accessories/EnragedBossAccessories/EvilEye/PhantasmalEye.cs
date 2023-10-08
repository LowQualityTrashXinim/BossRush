using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye {
	class PhantasmalEye : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Generic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.timeLeft = 500;
			Projectile.penetrate = -1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.immune[Projectile.owner] = 3;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] < 30) {
				return;
			}
			Projectile.netUpdate = true;
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 1500)) {
				Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 7;
				if (Projectile.timeLeft % 50 == 0) {
					Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 20;
				}
				if (Projectile.velocity.X > 40) {
					Projectile.velocity.X = 40;
				}
				else if (Projectile.velocity.X < -40) {
					Projectile.velocity.X = -40;
				}
				if (Projectile.velocity.Y > 40) {
					Projectile.velocity.Y = 40;
				}
				else if (Projectile.velocity.Y < -40) {
					Projectile.velocity.Y = -40;
				}
				return;
			}
			Projectile.velocity.Y += 0.3f;

		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<PhantasmalEye>("PhantasmalEyeAfterImage")).Value;

			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, Projectile.scale - k * 0.1f, SpriteEffects.None, 0);
			}
			return true;
		}
	}
}