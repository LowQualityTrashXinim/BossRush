using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles {
	class NightmaresProjectile : ModProjectile {
		public override string Texture => BossRushTexture.WHITEBALL;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 100;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.extraUpdates = 3;
			Projectile.penetrate = -1;
		}
		public override Color? GetAlpha(Color lightColor) {
			return new Color(0, 0, 0);
		}
		public override void AI() {
			if (Projectile.timeLeft <= 180) {
				Projectile.ProjectileAlphaDecay(180);
			}
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, DustID.Granite, newColor: Color.Black);
			dust.noGravity = true;
			dust.noLight = true;
			dust.velocity = Vector2.Zero;
			dust.scale = Main.rand.NextFloat(.9f, 1.24f);
			if (Projectile.velocity.IsLimitReached(1f)) {
				Projectile.velocity *= .99f;
			}
			if (++Projectile.ai[0] <= 90) {
				return;
			}
			Player player = Main.player[Projectile.owner];
			Projectile.Center.LookForHostileNPC(out NPC npc, 1200);
			if (npc == null) {
				if (!Projectile.Center.IsCloseToPosition(player.Center, 200f)) {
					Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .01f;
				}
				else {
					Projectile.velocity *= .99f;
				}
			}
			else {
				Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .1f;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
			}
		}
		public void DrawTrail1(Texture2D texture, Vector2 origin) {
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
				Color color = new Color(0, 0, 0, 255) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, Projectile.scale - k / 100f, SpriteEffects.None, 0);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.WHITEBALL).Value;
			Vector2 origin = Projectile.Size * .5f;
			DrawTrail1(texture, origin);
			return false;
		}
	}
}
