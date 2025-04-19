using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.TheOrbit;
internal class TheOrbit : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 21, 4f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<TheOrbitProjectile>(), 16f, true);
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
	}
	int counter = 0;
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		counter = BossRushUtils.Safe_SwitchValue(counter, 3);
		int valid = 1;
		if (counter == 3) {
			valid = 3;
		}
		Projectile.NewProjectile(source, position, velocity, type, damage + (int)(counter % 2 == 1 ? damage * .5f : 0), knockback, player.whoAmI, valid);
	}
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<TheOrbitProjectile>()] < 1;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.EnchantedBoomerang)
			.AddIngredient(ItemID.FlamingMace)
			.Register();
	}
}
public class TheOrbitProjectile : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TheOrbit>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 999;
		Projectile.penetrate = -1;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.timeLeft == 999) {
			for (int i = 0; i < Projectile.ai[0]; i++) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<OrbitProjectile>(), (int)(Projectile.damage * .55f), 0, player.whoAmI, Projectile.whoAmI, Projectile.ai[0], i);
			}
		}
		float distance = 450;
		Projectile.Center += player.velocity;
		Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10);
		if (Projectile.timeLeft <= 100) {
			Projectile.timeLeft += 360;
		}
		if (!Projectile.Center.IsCloseToPosition(player.Center, distance) || Projectile.ai[2] == 1) {
			Projectile.ai[2] = 1;
			Vector2 velto = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.Center.IsCloseToPosition(player.Center, distance)) {
				Projectile.velocity = velto * 15;
			}
			else {
				Projectile.velocity += velto;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(15f);
			}
			if (Projectile.Center.IsCloseToPosition(player.Center, 30)) {
				Projectile.Kill();
			}
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		Projectile.ai[2] = 1;
	}
}
public class OrbitProjectile : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.FlamingMace);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 999;
		Projectile.penetrate = -1;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
		dust.noGravity = true;
		dust.position += Main.rand.NextVector2Circular(32, 32);
		int Projectile_WhoAmI = (int)Projectile.ai[0];
		Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10);
		if (Projectile.timeLeft <= 100) {
			Projectile.timeLeft += 360;
		}
		Projectile proj = Main.projectile[Projectile_WhoAmI];
		Projectile.Center = proj.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(360f / Projectile.ai[1] * Projectile.ai[2] + Projectile.timeLeft * 5)) * 50;
		if (!proj.active) {
			Projectile.Kill();
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(1, 9)));
	}
}
