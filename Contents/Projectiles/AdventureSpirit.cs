using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Perks;

namespace BossRush.Contents.Projectiles;
internal class AdventureSpirit : ModProjectile {
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 999;
		Projectile.tileCollide = false;
		Projectile.light = 4f;
	}
	public override bool? CanDamage() {
		return false;
	}
	public int radius = 425;
	public override void AI() {
		if (Main.rand.NextBool(4)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, 20, 20, DustID.Torch);
			dust.velocity = Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(-3f, -1f);
		}
		SelectFrame();
		Projectile.ai[0] = BossRushUtils.Safe_SwitchValue((int)Projectile.ai[0], 300, 0);
		Player player = Main.player[Projectile.owner];
		if (player.dead || !player.active) {
			Projectile.Kill();
		}
		if (Vector2.DistanceSquared(player.Center, Projectile.Center) > 3000 * 3000) {
			Projectile.Center = player.Center;
		}
		radius = 425;
		Projectile.timeLeft = 999;
		Vector2 toPlayer = player.Center - Projectile.Center;
		float speed = .1f;
		PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
		if (modplayer.perks.ContainsKey(Perk.GetPerkType<ArenaBlessing>())) {
			int[] buffstack = new int[4];
			int value = modplayer.perks[Perk.GetPerkType<ArenaBlessing>()];
			buffstack[0] = BuffID.Campfire;
			if (value >= 2) {
				speed += .1f;
				radius += 100;
				buffstack[1] = BuffID.HeartLamp;
			}
			if (value >= 3) {
				speed += .1f;
				radius += 100;
				buffstack[2] = BuffID.StarInBottle;
			}
			if (value >= 4) {
				speed += .1f;
				radius += 100;
				buffstack[3] = BuffID.CatBast;
			}
			bool isInRange = Vector2.DistanceSquared(player.Center, Projectile.Center) <= radius * radius;
			if (isInRange) {
				for (int i = 0; i < buffstack.Length; i++) {
					if (buffstack[i] != 0) {
						player.AddBuff(buffstack[i], 300);
					}
				}
			}
		}
		else {
			Projectile.Kill();
		}
		Projectile.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
		Projectile.velocity = BossRushUtils.LimitedVelocity(Projectile.velocity, toPlayer.Length() * speed * 0.05f);
	}
	public void SelectFrame() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		BossRushUtils.BresenhamCircle(Projectile.Center, radius, Color.Lerp(Color.Red, Color.Orange, Projectile.ai[0] / 300f));
		return base.PreDraw(ref lightColor);
	}
}
