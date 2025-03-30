using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Projectiles;
internal class GlassProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Glass);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.Glass);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
		if (Projectile.velocity.Y < 20 && ++Projectile.ai[0] >= 30) {
			Projectile.velocity.Y += .25f;
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override void OnKill(int timeLeft) {
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 75f);
		Player player = Main.player[Projectile.owner];
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(Projectile.damage * .34f), 0));
		}
	}
}
internal class DirtProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.DirtBlock);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.Dirt);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
		if (Projectile.velocity.Y < 20 && ++Projectile.ai[0] >= 30) {
			Projectile.velocity.Y += .25f;
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
}
internal class SnowBlockProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SnowBlock);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.SnowBlock);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
		if (Projectile.velocity.Y < 20 && ++Projectile.ai[0] >= 30) {
			Projectile.velocity.Y += .25f;
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override void OnKill(int timeLeft) {
		int amount = 3 + Main.rand.NextBool().ToInt();
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.SnowBallFriendly, Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
		}
	}
}
