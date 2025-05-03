using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BundleOfGrenade;
internal class BundleOfGrenade : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(30, 30, 40, 10f, 40, 40, ItemUseStyleID.Swing, ModContent.ProjectileType<FragmentGrenadeProjectile>(), 15, false);
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = Main.rand.Next(new int[] { ProjectileID.Grenade, ProjectileID.BouncyGrenade, ProjectileID.StickyGrenade, ProjectileID.ClusterGrenadeI, ProjectileID.GrenadeI, ModContent.ProjectileType<FragmentGrenadeProjectile>() });
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Grenade)
			.AddIngredient(ItemID.Boomstick)
			.Register();
	}
}
public class FragmentGrenadeProjectile : SynergyModProjectile {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = BossRushUtils.ToSecond(10);
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.netUpdate = true;
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.25f);
		if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.65f);
		Projectile.timeLeft -= 60;
		return false;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 20 * -Projectile.direction);
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity.X *= .999f;
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		for (int l = 0; l < 103; l++) {
			if (l % 4 == 0) {
				int smoke = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
				Main.dust[smoke].noGravity = true;
				Main.dust[smoke].velocity = Main.rand.NextVector2Circular(10, 10);
				Main.dust[smoke].fadeIn = 10;
			}
			if (l % 2 == 0) {
				int smoke = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
				Main.dust[smoke].noGravity = true;
				Main.dust[smoke].velocity = Main.rand.NextVector2Circular(3, 3);
				Main.dust[smoke].fadeIn = 10;
				int fire = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, Scale: Main.rand.NextFloat(2, 4));
				Main.dust[fire].noGravity = true;
				Main.dust[fire].velocity = Main.rand.NextVector2Circular(5, 5);
			}
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(6, 6);
			int dus1t = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, Scale: Main.rand.NextFloat(2, 4));
			Main.dust[dus1t].noGravity = true;
			Main.dust[dus1t].velocity = Main.rand.NextVector2CircularEdge(6, 6) * Main.rand.NextFloat(.8f, 1.2f);
		}
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		if (npclist.Count > 0) {
			foreach (NPC npc in npclist) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, BossRushUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X), Main.rand.Next(1, 101) <= Projectile.CritChance, Projectile.knockBack));
			}
		}
		int amount = 10;
		int type = ProjectileID.Bullet;
		int damage = (int)(Projectile.damage * .65f);
		float knockback = Projectile.knockBack * .65f;
		for (int i = 0; i < amount; i++) {
			Vector2 vel = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(3, 4);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, type, damage, knockback, Projectile.owner);
		}
	}
}
