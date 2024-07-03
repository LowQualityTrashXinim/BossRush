using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Projectiles;
internal class LeafProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BladeOfGrass);
	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = 8;
		ProjectileID.Sets.TrailCacheLength[Type] = 5;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = 30; Projectile.height = 30;
		Projectile.timeLeft = 9999;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.friendly = true;
	}
	//Projectile.ai[0] will act as index
	Player player;
	Vector2 PositionItNeedToBeAt = Vector2.Zero;
	bool ItIsReady = false;
	Vector2 NPCpos = Vector2.Zero;
	public override void AI() {
		DrawOriginOffsetY = 5;
		if (Main.rand.NextBool(5)) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GrassBlades);
			Main.dust[dust].noGravity = true;
		}
		if (Projectile.timeLeft == 9999) {
			player = Main.player[Projectile.owner];
		}
		if (!player.active || player.dead) {
			Projectile.Kill();
			return;
		}
		if (player.ownedProjectileCounts[Type] >= 10) {
			ItIsReady = true;
			NPCpos = Projectile.Center.LookForHostileNPCPositionClosest(1500);
		}
		if (!ItIsReady || NPCpos == Vector2.Zero) {
			PositionItNeedToBeAt = player.Center + Vector2.UnitY.Vector2DistributeEvenly(10, 360, Projectile.ai[0]) * 45;
			Projectile.velocity += (PositionItNeedToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat();
			Projectile.velocity = Projectile.velocity.LimitedVelocity((PositionItNeedToBeAt - Projectile.Center).Length() * 0.0625f);
		}
		else {
			if (Projectile.timeLeft > 300) {
				Projectile.timeLeft = 300;
			}
			if (NPCpos != Vector2.Zero) {
				Vector2 dis = NPCpos - Projectile.Center;
				Projectile.velocity += dis.SafeNormalize(Vector2.Zero) * .5f;
				float disLen = dis.Length() * 0.0625f;
				if (disLen > 15) {
					Projectile.velocity = Projectile.velocity.LimitedVelocity(dis.Length() * 0.0625f);
				}
			}
			NPCpos = Projectile.Center.LookForHostileNPCPositionClosest(1000);
			Projectile.ProjectileAlphaDecay(300);
		}
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.direction = 1;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Poisoned, 180);
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 10; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GrassBlades);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(5f, 5f);
		}
		player.GetModPlayer<GlobalItemPlayer>().Projindex[(int)Projectile.ai[0]] = -1;
	}
}
