using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Projectiles;
internal class FriendlyGrenadeProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Grenade);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = BossRushUtils.ToSecond(10);
	}
	int bouncecount = 0;
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (bouncecount < 10) {
			Projectile.netUpdate = true;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.9f);
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.75f);
			bouncecount++;
		}
		else {
			if (Projectile.velocity.IsLimitReached(.1f)) {
				Projectile.position += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
		return false;
	}
	public override void AI() {
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation += MathHelper.ToRadians(Projectile.timeLeft * 5 * -Projectile.direction);
			Projectile.velocity.X *= 0.98f;
			Projectile.velocity.Y += 0.5f;
		}
	}
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		for (int l = 0; l < 53; l++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(10, 10);
			int dus1t = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, Scale: Main.rand.NextFloat(2, 4));
			Main.dust[dus1t].noGravity = true;
			Main.dust[dus1t].velocity = Main.rand.NextVector2Circular(15, 15);
		}
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		if (npclist.Count > 0) {
			foreach (NPC npc in npclist) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, BossRushUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X), Main.rand.Next(1, 101) <= Projectile.CritChance, Projectile.knockBack));
			}
		}
	}
}
