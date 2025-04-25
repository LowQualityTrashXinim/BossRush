using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HandmadeLauncher;
public class HandmadeLauncher : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(72, 32, 40, 6, 60, 60, ItemUseStyleID.Shoot, ProjectileID.Grenade, 20, true, AmmoID.Rocket);
		Item.UseSound = SoundID.Item61;
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 40);
		type = ModContent.ProjectileType<LauncherProjectile>();
	}
	public override Vector2? HoldoutOffset() {
		return new(-11, -3);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		for (int i = 0; i < 20; i++) {
			Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Torch);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(20, 20);
			dust.velocity = velocity.Vector2RotateByRandom(25) * Main.rand.NextFloat(.6f, .9f);
			dust.scale = Main.rand.NextFloat(.85f, 1.3f);
		}
		for (int i = 0; i < 10; i++) {
			Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Smoke);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(20, 20);
			dust.velocity = velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.3f, .8f);
			dust.scale = Main.rand.NextFloat(1.15f, 1.85f);
		}
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FlareGun)
			.AddIngredient(ItemID.Grenade)
			.Register();
	}
}
public class LauncherProjectile : SynergyModProjectile {
	public override void SetDefaults() {
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.scale = .77f;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (++Projectile.ai[0] >= 30) {
			if (Projectile.velocity.Y <= 20) {
				Projectile.velocity.Y += 1;
			}
		}
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(10, 10);
			dust.scale = Main.rand.NextFloat(.85f, 1.3f);
		}
		Projectile.velocity.X *= .99f;
		Projectile.rotation = Projectile.velocity.ToRotation();
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		npc.AddBuff<Aftershock>(BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150);
		for (int i = 0; i < 100; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(15, 15);
			dust.scale = Main.rand.NextFloat(2.15f, 2.85f);
		}
		for (int i = 0; i < 100; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(20, 20);
			dust.velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.6f, .9f);
			dust.scale = Main.rand.NextFloat(.85f, 1.3f);
		}
		for (int i = 0; i < 75; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(20, 20);
			dust.velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.3f, .8f);
			dust.scale = Main.rand.NextFloat(1.15f, 1.85f);
		}
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, Projectile.direction, knockBack: Projectile.knockBack));
		}
	}
}
public class Aftershock : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 20;
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .55f;
	}
}
