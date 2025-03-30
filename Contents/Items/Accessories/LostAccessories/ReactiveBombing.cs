using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ReactiveBombing : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ReactiveBombingPlayer>().ReactiveBomb = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.StaticDefense, Base: 10);
	}
}
public class ReactiveBombingPlayer : ModPlayer {
	public bool ReactiveBomb = false;
	public override void ResetEffects() {
		ReactiveBomb = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (ReactiveBomb) {
			SpawnBomb();
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (ReactiveBomb) {
			SpawnBomb();
		}
	}
	public void SpawnBomb() {
		int damage = (int)Player.GetDamage(DamageClass.Generic).ApplyTo(90);
		for (int i = 0; i < 4; i++) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Circular(5f, 5f) * Main.rand.NextFloat(2, 3.5f), ModContent.ProjectileType<ReactiveBombProjectile>(), damage, 3f, Player.whoAmI);
		}
	}
}
public class ReactiveBombProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Bomb);
	public override void SetDefaults() {
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 500;
		Projectile.penetrate = 1;
	}
	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		fallThrough = false;
		return true;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (++Projectile.ai[1] <= 6) {
			Projectile.netUpdate = true;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.6f);
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.6f);
		}
		else {
			if (Projectile.velocity.IsLimitReached(.1f)) {
				Projectile.position += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
		}
		return false;
	}
	public override bool? CanDamage() {
		return Projectile.ai[0] >= 30;
	}
	public override void AI() {
		if (Projectile.velocity.IsLimitReached(.1f)) {
			Projectile.rotation += Projectile.velocity.ToRotation() * .1f;
		}
		Projectile.velocity.X *= 0.98f;
		Projectile.velocity.Y += 0.35f;
		Projectile.ai[0]++;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.Kill();
	}
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		for (int i = 0; i < 34; i++) {
			var randomSpeed = Main.rand.NextVector2Circular(5, 5);
			Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 3.5f));
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(7.5f, 7.5f) * Main.rand.NextFloat(2, 4);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 3f);
		}
		BossRushUtils.LookForHostileNPC(Projectile.Center, out List<NPC> npclist, 150f);
		foreach (var npc in npclist) {
			int direction = Projectile.Center.X < npc.Center.X ? -1 : 1;
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, direction, false, 3.5f));
		}
	}
}
