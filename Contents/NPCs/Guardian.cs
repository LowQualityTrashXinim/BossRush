using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.NPCs {
	internal class Guardian : ModNPC {
		public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
		public override void SetStaticDefaults() {
			NPCID.Sets.ImmuneToAllBuffs[Type] = true;
			NPCID.Sets.TrailCacheLength[NPC.type] = 50;
		}
		public override void SetDefaults() {
			NPC.lifeMax = 90000;
			NPC.damage = 400;
			NPC.defense = 50;
			NPC.friendly = false;
			NPC.width = NPC.height = 30;
			NPC.lavaImmune = true;
			NPC.trapImmune = true;
			NPC.knockBackResist = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.scale = 2f;
			NPC.color = Color.Black;
		}
		public override void AI() {
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(NPC.Center + Main.rand.NextVector2Circular(50, 50), 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.5f, 3f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Color.Black;
			}
			Player player = Main.player[NPC.target];
			NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
			if (!player.active || player.dead) {
				NPC.FindClosestPlayer();
				NPC.TargetClosest();
				return;
			}
			if (NPC.ai[0] >= 150) {
				for (int i = 0; i < 8; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenly(8, 360, i);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<GuardianSmallerProjectile>(), (int)(NPC.damage * .25f), 40, -1, NPC.target);
				}
				NPC.ai[0] = 0;
			}
			if (NPC.ai[1] >= 100) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * .1f, ModContent.ProjectileType<GuardianProjectile>(), NPC.damage, 40, -1, NPC.target);
				NPC.ai[1] = 0;
			}
			NPC.ai[0]++;
			NPC.ai[1]++;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			return true;
		}
	}
	public class GuardianProjectile : ModProjectile {
		public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 10;
		}
		public override void AI() {
			Lighting.AddLight(Projectile.Center, new Vector3(1, 1, 1));
			if (Projectile.ai[1] < 10) {
				Projectile.velocity += Projectile.velocity * .001f;
			}
			else {
				Projectile.ai[1]++;
			}
			if (Projectile.ai[0] >= 0) {
				Projectile.velocity += (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .005f;
			}
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
			if (Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.Granite);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Color.Black;
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 150; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.5f, 2f));
				Vector2 vel = Main.rand.NextVector2Circular(5, 5);
				Main.dust[dust].velocity = vel;
				Main.dust[dust].color = Color.Black;
			}
			base.OnKill(timeLeft);
		}
		public override Color? GetAlpha(Color lightColor) {
			return Color.Black;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutColorAdjustment(Color.Black, .02f);
			return true;
		}
	}
	public class GuardianSmallerProjectile : ModProjectile {
		public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		public override void SetDefaults() {
			Projectile.scale = .5f;
			Projectile.width = Projectile.height = 30;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 10;
		}
		public override void AI() {
			Lighting.AddLight(Projectile.Center, new Vector3(1, 1, 1));
			if (Projectile.ai[0] >= 0) {
				Projectile.velocity += (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .0025f;
			}
			Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
			if (Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, DustID.Granite);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Color.Black;
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 50; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.5f, 2f));
				Vector2 vel = Main.rand.NextVector2Circular(3, 3);
				Main.dust[dust].velocity = vel;
				Main.dust[dust].color = Color.Black;
			}
			base.OnKill(timeLeft);
		}
		public override Color? GetAlpha(Color lightColor) {
			return Color.Black;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutColorAdjustment(Color.Black, .01f);
			return true;
		}
	}
}
