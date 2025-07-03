using BossRush.Common.Global;
using BossRush.Common.Utils;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmberBoneSpear;
using BossRush.Contents.Projectiles;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
public class CactusSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CactusSword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (hit.Crit && globalItem.Item_Counter1[index] <= 0) {
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(10) * Main.rand.NextFloat(6, 8), ProjectileID.RollingCactus, player.GetWeaponDamage(item) * 3, item.knockBack, player.whoAmI);
			Main.projectile[projectile].friendly = true;
			Main.projectile[projectile].hostile = false;
			Main.projectile[projectile].penetrate = -1;
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(3));
		}
	}
}
public class EnchantedSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EnchantedSword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		if (item.DamageType == DamageClass.Melee) {
			useSpeed += .1f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), position, velocity.Vector2RotateByRandom(3), ProjectileID.EnchantedBeam, (int)(damage * 1.25f), knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.shoot == ProjectileID.None) {
			Vector2 spawningPos = Main.rand.NextVector2Circular(650f, 650f) + player.Center;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(item), spawningPos, (target.Center - spawningPos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.EnchantedBeam, (int)(player.GetWeaponDamage(item) * 1.25f), item.knockBack, player.whoAmI);
			Main.projectile[projectile].tileCollide = false;
		}
	}
}
public class StarFury : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Starfury;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
				Vector2 positionAbovePlayer = player.Center + new Vector2(Main.rand.Next(-200, 200), -1000);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), positionAbovePlayer, (Main.MouseWorld - positionAbovePlayer).SafeNormalize(Vector2.Zero) * 10, ProjectileID.Starfury, (int)(player.GetWeaponDamage(item) * 1.5f), 3f, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.shoot == ProjectileID.None && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(1.5f));
			Vector2 positionAbovePlayer = target.Center + new Vector2(Main.rand.Next(-200, 200), -1000) + Main.rand.NextVector2Circular(100, 100);
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item), positionAbovePlayer,
					((Main.MouseWorld - positionAbovePlayer).SafeNormalize(Vector2.Zero) * 17).Vector2RandomSpread(2, Main.rand.NextFloat(.95f, 1.12f)).Vector2RotateByRandom(2), ProjectileID.Starfury, (int)(player.GetWeaponDamage(item) * 1.5f), item.knockBack, player.whoAmI);
			}
		}
	}
}
public class IceBlade : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IceBlade;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			if (velocity.Length() < 5) {
				velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 5;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(.5f));
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(3, 7)));
	}
}
public class BatBat : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BatBat;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle playerstat = player.GetModPlayer<PlayerStatsHandle>();
		playerstat.UpdateMovement += .12f;
		playerstat.UpdateJumpBoost += .3f;
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeSteal(index, player, globalItem);
		if (Main.rand.NextBool(10)) {
			Vector2 position = target.Center + Main.rand.NextVector2CircularEdge(target.width + 10, target.height + 10);
			Vector2 vel = (position - target.Center).SafeNormalize(Vector2.Zero) * 5f;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), position, vel, ProjectileID.Bat, (int)(hit.Damage * .65f), 0, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeSteal(index, player, globalItem);
		if (Main.rand.NextBool(10)) {
			Vector2 position = target.Center + Main.rand.NextVector2CircularEdge(target.width + 10, target.height + 10);
			Vector2 vel = (position - target.Center).SafeNormalize(Vector2.Zero) * 5f;
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position, vel, ProjectileID.Bat, (int)(hit.Damage * .65f), 0, player.whoAmI);
		}
	}
	private void LifeSteal(int index, Player player, EnchantmentGlobalItem globalItem) {
		if (globalItem.Item_Counter1[index] <= 0) {
			player.Heal(1);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);//0.2 second
		}
	}
}
public class BoneSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BoneSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		BoneExplosion(index, player, globalItem, target.Center, damageDone, item.knockBack);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.Bone) {
			BoneExplosion(index, player, globalItem, target.Center, damageDone, proj.knockBack);
		}
	}
	private static void BoneExplosion(int index, Player player, EnchantmentGlobalItem globalItem, Vector2 position, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0 && Main.rand.NextBool(4)) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position, Main.rand.NextVector2CircularEdge(10f, 10f), ProjectileID.Bone, damage, knockback, player.whoAmI);
			Main.projectile[proj].penetrate = -1;
			Main.projectile[proj].maxPenetrate = -1;
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
		}
	}
}
public class BladedGlove : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BladedGlove;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed -= .05f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		NPC.HitInfo sample = hit;
		sample.Crit = false;
		player.StrikeNPCDirect(target, sample);
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class ZombieArm : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ZombieArm;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (!Main.dayTime) {
			crit += 10;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitByNPC(int index, EnchantmentGlobalItem globalItem, Player player, NPC npc, Player.HurtInfo hurtInfo) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
		foreach (NPC target in npclist) {
			player.StrikeNPCDirect(target, npc.CalculateHitInfo((int)(player.GetWeaponDamage(player.HeldItem) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class MandibleBlade : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AntlionClaw;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (player.velocity.Length() > 2) {
			damage += .1f;
		}
	}
	public override void OnHitByNPC(int index, EnchantmentGlobalItem globalItem, Player player, NPC npc, Player.HurtInfo hurtInfo) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(
				player.GetSource_FromThis(),
				player.Center,
				Main.rand.NextVector2Circular(4f, 4f) * 5f,
				ModContent.ProjectileType<AntlionMandibleModProjectile>(),
				(int)(player.GetWeaponDamage(player.HeldItem) * .75f),
				(int)(player.HeldItem.knockBack * .5f),
				player.whoAmI);
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class TentacleSpike : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TentacleSpike;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (player.ZoneCorrupt) {
			crit += 10;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		target.AddBuff(BuffID.TentacleSpike, 900);
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		target.AddBuff(BuffID.TentacleSpike, 900);
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class LightsBane : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LightsBane;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter2[index] = BossRushUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 && proj.minion) {
			return;
		}
		Vector2 vel = Main.rand.NextVector2CircularEdge(5, 5);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), target.Center.PositionOFFSET(vel, -60), vel, ProjectileID.LightsBane, player.GetWeaponDamage(player.HeldItem), player.HeldItem.knockBack, player.whoAmI, 1);
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 180);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		Vector2 vel = Main.rand.NextVector2CircularEdge(5, 5);
		Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center.PositionOFFSET(vel, -60), vel, ProjectileID.LightsBane, player.GetWeaponDamage(item), player.HeldItem.knockBack, player.whoAmI, 1);
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class BladeOfGrass : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BladeofGrass;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (player.ZoneJungle) {
			damage *= 1.2f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool()) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BladeOfGrass, (int)(damage * .45f), knockback, player.whoAmI,
				MathHelper.ToRadians(Main.rand.Next(3, 7)), Main.rand.Next(4, 8));
		}
		else {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BladeOfGrass, (int)(damage * .45f), knockback, player.whoAmI, 0, Main.rand.Next(4, 8));
		}
	}
}
public class Volcano : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FieryGreatsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter2[index] = BossRushUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(2));
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		player.Center.LookForHostileNPC(out List<NPC> npclist, 125);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(player.GetWeaponDamage(player.HeldItem) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		for (int i = 0; i < 35; i++) {
			int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(125 / 12f, 125 / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(125 / 12f, 125 / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 240);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(6));
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		player.Center.LookForHostileNPC(out List<NPC> npclist, 125);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(player.GetWeaponDamage(item) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		for (int i = 0; i < 35; i++) {
			int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(125 / 12f, 125 / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(125 / 12f, 125 / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class NightsEdge : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.NightsEdge;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (item.shoot == ProjectileID.None) {
			item.shoot = ProjectileID.NightsEdge;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
		Projectile.NewProjectile(source, player.Center, vel, ProjectileID.NightsEdge, damage, knockback, player.whoAmI, -.1f, 30, 1);
	}
}
public class Gladius : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Gladius;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 5;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3)) {
			Vector2 position = target.Center + Main.rand.NextVector2CircularEdge(50 + target.width, 50 + target.height);
			Vector2 vel = (target.Center - position).SafeNormalize(Vector2.Zero) * 10;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				position,
				vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: -120);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(7) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 position = target.Center + Main.rand.NextVector2CircularEdge(50 + target.width, 50 + target.height);
			Vector2 vel = (target.Center - position).SafeNormalize(Vector2.Zero) * 10;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				position,
				vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: -120);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile2 shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class Katana : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Katana;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 3);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.2f);
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, 1.35f);
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 15);
			Vector2 pos = target.Center;
			int type = ModContent.ProjectileType<SwordProjectile>();
			if (Main.rand.NextBool()) {
				pos += Main.rand.NextVector2CircularEdge(target.width + 50, target.height + 50);
				type = ModContent.ProjectileType<SwordProjectileSpear>();
			}
			else {
				pos += Main.rand.NextVector2CircularEdge(target.width + 20, target.height + 20);
			}
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero), type, item.damage, item.knockBack, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile swordproj) {
				swordproj.ItemIDtextureValue = ItemIDType;
				Main.projectile[proj].scale = 1;
				Main.projectile[proj].Resize(ContentSamples.ItemsByType[ItemIDType].width, ContentSamples.ItemsByType[ItemIDType].height);
			}
			else if (Main.projectile[proj].ModProjectile is SwordProjectileSpear swordspearproj) {
				swordspearproj.ItemIDtextureValue = ItemIDType;
				Main.projectile[proj].Resize(ContentSamples.ItemsByType[ItemIDType].width, ContentSamples.ItemsByType[ItemIDType].height);
				Main.projectile[proj].velocity = (target.Center - pos).SafeNormalize(Vector2.Zero);
			}
		}
	}
}
public class WoodenBoomerang : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenBoomerang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.WoodenBoomerang] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.WoodenBoomerang, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
	}
}

public class EnchantedBoomerang : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.EnchantedBoomerang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.EnchantedBoomerang] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.EnchantedBoomerang, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
	}
}
public class IceBoomerang : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.IceBoomerang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.IceBoomerang] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.IceBoomerang, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.IceBoomerang) {
			target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(5, 8)));
		}
	}
}
public class Shroomerang : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.Shroomerang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.Shroomerang] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.Shroomerang, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
	}
}
public class ThornChakram : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.ThornChakram;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.ThornChakram] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.ThornChakram, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
	}
}
public class BreakerBlade : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BreakerBlade;
	}
	public override void OnAddEnchantment(Item item, EnchantmentGlobalItem globalitem, int EnchantmentItemID, int slot) {
		int meleeItem = TerrariaArrayID.MeleeHM[Main.rand.Next(TerrariaArrayID.MeleeHM.Length)];
		Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), meleeItem);
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += 1f;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		if (item.DamageType == DamageClass.Melee)
			useSpeed *= 0.8f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.DamageType == DamageClass.Melee)
			damage.Base += 25;
	}
}
public abstract class YoyoEnchantment : ModEnchantment {
	public float yoyoAmount = 2f;
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		for (int i = 0; i < 200; i++) {
			if (Main.projectile[i].type == ModContent.ProjectileType<GhostYoyo>() && Main.projectile[i].ai[0] == ItemIDType && Main.projectile[i].active)
				return;
		}
		for (int j = 0; j < yoyoAmount; j++)
			Projectile.NewProjectile(
				player.GetSource_ItemUse(item),
				player.position,
				Vector2.Zero,
				ModContent.ProjectileType<GhostYoyo>(),
				ContentSamples.ItemsByType[ItemIDType].damage / 2,
				1,
				player.whoAmI, ItemIDType, MathHelper.Lerp(MathHelper.TwoPi, 0f, j / yoyoAmount), 0.075f);
	}
}
public class WoodenYoyo : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.WoodYoyo;
	}


}
public class Malaise : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.CorruptYoyo;
		yoyoAmount = 4;
	}

}

public class Amazon : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.JungleYoyo;
		yoyoAmount = 3;
	}

}

public class HiveFive : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.HiveFive;
		yoyoAmount = 5;
	}

}

public class Rally : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.Rally;
	}

}

public class Artery : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.CrimsonYoyo;
		yoyoAmount = 3;
	}
}

public class Code1 : YoyoEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.Code1;
		yoyoAmount = 5;

	}

}

public class Terragrim : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Terragrim;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .45f) {
			player.StrikeNPCDirect(target, hit);
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float attackspeed = player.GetWeaponAttackSpeed(item) - 1;
		if (attackspeed <= 0) {
			return;
		}
		modifiers.SourceDamage += attackspeed * .5f;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		if (item.DamageType == DamageClass.Melee) {
			useSpeed += .15f;
		}
	}
}

public class Trimarang : ModEnchantment {

	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.Trimarang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		int projectileamount = player.ownedProjectileCounts[ProjectileID.Trimarang];
		if (projectileamount < 1) {
			if (player.itemAnimation == player.itemAnimationMax) {
				for (int i = 0; i < 3; i++) {
					Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2DistributeEvenlyPlus(3, 30, i);
					Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.Trimarang, itemstat.damage, itemstat.knockBack, player.whoAmI);
				}
			}
		}
		else {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1 + .2f);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 20);
		}
	}

}

public class BloodButcherer : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BloodButcherer;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextBool(10))
			target.AddBuff(ModContent.BuffType<BloodButchererEnchantmentDebuff>(), BossRushUtils.ToSecond(2));
		modifiers.FinalDamage.Flat += GiveBonusDamage(proj.damage, target);
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextBool(10))
			target.AddBuff(ModContent.BuffType<BloodButchererEnchantmentDebuff>(), BossRushUtils.ToSecond(2));
		modifiers.FinalDamage.Flat += GiveBonusDamage(item.damage, target);
	}
	private float GiveBonusDamage(float baseDamage, NPC target) {
		if (target.lifeRegen < 0)
			return baseDamage * MathHelper.Lerp(0f, 0.25f, MathHelper.Clamp(Math.Abs(target.lifeRegen) / 10f, 0f, 1f));
		return 0;
	}
}
public class BallOHurt : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BallOHurt;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Melee) += 0.2f;
		player.GetCritChance(DamageClass.Melee) += 5;
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 10;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<BallOfHurtProjectile>(), item.damage + 30, item.knockBack, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			}
		}
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}

	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(6));
	}
}
public class BallOfHurtProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BallOHurt);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X * 0.85f;
		}
		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
		}
		return false;
	}
	public override void AI() {
		Projectile.rotation = Projectile.direction * MathHelper.ToRadians(Projectile.timeLeft * -10 - Projectile.velocity.Length());
		if (++Projectile.ai[0] <= 10) {
			return;
		}
		if (!Projectile.wet) {
			if (Projectile.velocity.Y <= 20)
				Projectile.velocity.Y += .5f;
		}
		else {
			if (Projectile.velocity.Y >= -10)
				Projectile.velocity.Y -= .5f;
		}

	}
}
public class TheMeatball : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TheMeatball;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Melee) += 0.2f;
		player.GetCritChance(DamageClass.Melee) += 5;
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {

				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 10;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<TheMeatBallProjectile>(), item.damage + 30, item.knockBack, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			}
		}
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, BossRushUtils.ToSecond(6));
	}
}
public class TheMeatBallProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.TheMeatball);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X * 0.85f;
		}
		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
		}
		return false;
	}
	public override void AI() {
		Projectile.rotation = Projectile.direction * MathHelper.ToRadians(Projectile.timeLeft * -10 - Projectile.velocity.Length());
		if (++Projectile.ai[0] <= 10) {
			return;
		}
		if (!Projectile.wet) {
			if (Projectile.velocity.Y <= 20)
				Projectile.velocity.Y += .5f;
		}
		else {
			if (Projectile.velocity.Y >= -10)
				Projectile.velocity.Y -= .5f;
		}

	}
}
public class SwordFish : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Swordfish;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		int projectiletype = ModContent.ProjectileType<SwordFishProjectile>();
		if (player.ownedProjectileCounts[projectiletype] < 1) {
			if (player.Center.LookForAnyHostileNPC(300)) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.UnitY - Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(6, 8), projectiletype, item.damage, item.knockBack, player.whoAmI, Main.rand.Next(60, 90));
			}
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ArmorPenetration += 5;
		if (Main.rand.NextBool(10)) {
			target.StrikeNPC(modifiers.ToHitInfo(proj.damage, false, 0, false, player.luck));
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ArmorPenetration += 5;
		if (Main.rand.NextBool(10)) {
			target.StrikeNPC(modifiers.ToHitInfo(item.damage, false, 0, false, player.luck));
		}
	}
	public class SwordFishProjectile : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Swordfish);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			for (int i = 0; i < 4; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Water);
				dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				dust.position += Main.rand.NextVector2Circular(60, 30).RotatedBy(Projectile.velocity.ToRotation());
			}

			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.rotation += Projectile.spriteDirection != 1 ? MathHelper.PiOver4 : MathHelper.PiOver4 - MathHelper.Pi + MathHelper.PiOver2;
			if (Projectile.ai[0] <= 0) {
				Projectile.ai[0]--;
				Projectile.timeLeft = 300;
				return;
			}
			Projectile.Center.LookForHostileNPCNotImmune(out NPC closestNPC, 1500, Projectile.owner, true);
			if (closestNPC == null) {
				Projectile.velocity.Y += 0.3f;
				return;
			}
			Projectile.velocity += (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 2f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(ModContent.BuffType<DeepSeaPressure>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 50; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Water);
				dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2Circular(10, 10);
			}
		}
	}
}
public class PalladiumSword : PalladiumEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalladiumSword;
	}
}

public class PalladiumPike : PalladiumEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalladiumPike;
	}
}

public class OrichalcumSword : OrichalcumEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.OrichalcumSword;
	}
}
public class OrichalcumHalberd : OrichalcumEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.OrichalcumHalberd;
	}
}

public class TitaniumSword : TitaniumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.TitaniumSword;
	}

}

public class TitaniumTrident : TitaniumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.TitaniumTrident;
	}

}

public class DeathSickle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DeathSickle;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritDamage, 1.5f);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!target.boss && Main.rand.NextBool(100)) {
			target.StrikeInstantKill();
			return;
		}
		globalItem.Item_Counter1[index]++;
		if (globalItem.Item_Counter1[index] >= 15) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center + new Vector2(100, 0), target.Center.DirectionTo(target.Center + new Vector2(100, 0) * 15), ModContent.ProjectileType<DeathSickleGhost>(), hit.Damage, 0, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ModContent.ProjectileType<DeathSickleGhost>() && proj.type != ModContent.ProjectileType<DeathSickleGhost>() &&
			proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			globalItem.Item_Counter1[index]++;

		if (globalItem.Item_Counter1[index] >= 15) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center + new Vector2(100, 0), target.Center.DirectionTo(target.Center + new Vector2(100, 0) * 15), ModContent.ProjectileType<DeathSickleGhost>(), hit.Damage, 0, player.whoAmI);
		}
	}
}
public class Umbrella : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Umbrella;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.slowFall = true;
		player.noFallDmg = true;
		player.endurance += .05f;
		if (player.velocity.Y != 0) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.15f);
		}
	}
}
public class TragicUmbrella : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TragicUmbrella;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.slowFall = true;
		player.noFallDmg = true;
		if (player.statLife <= player.statLifeMax2 * .2f) {
			player.endurance += .2f;
		}
		else if (player.statLife >= player.statLifeMax2 * .9f) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.1f);
		}
	}
}
public class CandyCaneSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CandyCaneSword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.05f);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(8) && hit.DamageType == DamageClass.Melee) {
			Item.NewItem(player.GetSource_OnHit(target), target.Hitbox, ItemID.CandyApple);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(16) && hit.DamageType == DamageClass.Melee) {
			Item.NewItem(player.GetSource_OnHit(target), target.Hitbox, ItemID.CandyApple);
		}
	}
}
public class Spear : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Spear;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetArmorPenetration(DamageClass.Melee) += 10;
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		for (int i = 0; i < 3; i++) {
			Vector2 pos = target.Center.Add(Main.rand.Next(-40, 40), Main.rand.Next(450, 500));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 6;
			Projectile.NewProjectile(player.GetSource_OnHit(target), pos, vel, ModContent.ProjectileType<SpearProjectile>(), (int)(hit.Damage * .8f), 2f, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ModContent.ProjectileType<SpearProjectile>()) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(3));
		for (int i = 0; i < 3; i++) {
			Vector2 pos = target.Center.Add(Main.rand.Next(-40, 40), Main.rand.Next(450, 500));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 6;
			Projectile.NewProjectile(player.GetSource_OnHit(target), pos, vel, ModContent.ProjectileType<SpearProjectile>(), (int)(hit.Damage * .8f), 2f, player.whoAmI);
		}
	}
}
public class SpearProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Spear);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.timeLeft = 900;
		Projectile.penetrate = 2;
		Projectile.tileCollide = false;
	}
	public override bool? CanDamage() {
		return Projectile.penetrate <= 1;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2;
		if (Projectile.velocity.Y < 14) {
			Projectile.velocity.Y += .5f;
		}
		if (Projectile.penetrate == 1) {
			if (Projectile.timeLeft > 20) {
				Projectile.timeLeft = 20;
			}
			Projectile.ProjectileAlphaDecay(20);
		}
	}
}
public class Trident : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Trident;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.AddStatsToPlayer(PlayerStats.MeleeCritChance, Base: 10);
		modplayer.AddStatsToPlayer(PlayerStats.MeleeCritDmg, 1.24f);
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
			int type = Main.rand.Next([ModContent.ProjectileType<TridentEnchantmentProjectile_Fish1>(), ModContent.ProjectileType<TridentEnchantmentProjectile_Fish2>()]);
			if (type == ModContent.ProjectileType<TridentEnchantmentProjectile_Fish1>()) {
				Vector2 pos = player.Center + Main.rand.NextVector2RectangleEdge(1000, 1000);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 10;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, type, hit.Damage, hit.Knockback, player.whoAmI);
			}
			else {
				Vector2 pos = player.Center + new Vector2(Main.rand.Next(300, 1000), 1000);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 14;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, type, hit.Damage, hit.Knockback, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0
			&& proj.type != ModContent.ProjectileType<TridentEnchantmentProjectile_Fish1>()
			&& proj.type != ModContent.ProjectileType<TridentEnchantmentProjectile_Fish2>()) {
			globalItem.Item_Counter1[index] = 120;
			int type = Main.rand.Next([ModContent.ProjectileType<TridentEnchantmentProjectile_Fish1>(), ModContent.ProjectileType<TridentEnchantmentProjectile_Fish2>()]);
			if (type == ModContent.ProjectileType<TridentEnchantmentProjectile_Fish1>()) {
				Vector2 pos = player.Center + Main.rand.NextVector2RectangleEdge(1000, 1000);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 10;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, type, hit.Damage, hit.Knockback, player.whoAmI);
			}
			else {
				Vector2 pos = player.Center + new Vector2(Main.rand.Next(300, 1000), 1000);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 14;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, type, hit.Damage, hit.Knockback, player.whoAmI);
			}
		}
	}
}
public class TridentEnchantmentProjectile_Fish1 : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Bass);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.timeLeft = 360;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.idStaticNPCHitCooldown = 90;
		Projectile.usesIDStaticNPCImmunity = true;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.ai[2] = Main.rand.Next(new int[] { ItemID.Trout, ItemID.Tuna });
		Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
	}
	public override void AI() {
		if (Projectile.ai[1] == 0) {
			Projectile.ai[1] = 1;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-15));
		}
		if (++Projectile.ai[0] >= 10) {
			Projectile.ai[1] *= -1;
			Projectile.ai[0] = 0;
		}
		Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 3));
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		if (Projectile.spriteDirection == -1) {
			Projectile.rotation += MathHelper.PiOver2;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		int type = (int)Projectile.ai[2];
		Main.instance.LoadProjectile(Type);
		Main.instance.LoadItem(type);
		Texture2D texture = TextureAssets.Item[type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin;
		SpriteEffects effect = SpriteEffects.None;
		if (Projectile.spriteDirection == -1) {
			effect = SpriteEffects.FlipHorizontally;
		}
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, 1, effect);
		return false;
	}
}
public class TridentEnchantmentProjectile_Fish2 : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Bass);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.timeLeft = 360;
		Projectile.penetrate = -1;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 90;
		Projectile.tileCollide = false;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.ai[2] = Main.rand.Next(new int[] { ItemID.Trout, ItemID.Tuna });
	}
	public override void AI() {
		if (++Projectile.ai[0] < 30) {
			Projectile.velocity.Y -= .5f;
		}
		else {
			Projectile.velocity.Y += .5f;
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2;
	}
	public override bool PreDraw(ref Color lightColor) {
		int type = (int)Projectile.ai[2];
		Main.instance.LoadProjectile(Type);
		Main.instance.LoadItem(type);
		Texture2D texture = TextureAssets.Item[type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin;
		SpriteEffects effect = SpriteEffects.None;
		if (Projectile.direction == -1) {
			effect = SpriteEffects.FlipHorizontally;
		}
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, 1, effect);
		return false;
	}
}

public class StylistKilLaKillScissorsIWish : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.StylistKilLaKillScissorsIWish;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		statplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 1, 1.11f);
		statplayer.AddStatsToPlayer(PlayerStats.MeleeCritDmg, 1, 1.12f);
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .2f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool()) {
			Vector2 spawnPosition = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height);
			Vector2 velocityToward = (target.Center - spawnPosition).SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), spawnPosition, velocityToward, ModContent.ProjectileType<SimplePiercingProjectile>(), (int)(hit.Damage * .85f), 0, player.whoAmI, 3);
		}
	}
}
public class Ruler : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Ruler;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		float damageincreases = (target.Center - player.Center).Length() / 5f;
		modifiers.SourceDamage.Base += damageincreases * .1f;
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float damageincreases = (target.Center - player.Center).Length() / 5f;
		modifiers.SourceDamage += damageincreases * .1f;
	}
}
public class ThunderSpear : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ThunderSpear;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		if (!player.ItemAnimationActive || globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(1));
		Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15;
		Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.ThunderSpearShot, player.GetWeaponDamage(item), 3f, player.whoAmI);
		proj.alpha -= 120;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (++globalItem.Item_Counter3[index] < 3) {
			return;
		}
		globalItem.Item_Counter3[index] = 0;
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, -Vector2.UnitY.Vector2RotateByRandom(40) * Main.rand.NextFloat(7, 9), ModContent.ProjectileType<ThunderSpearThrowProjectile>(), hit.Damage * 3, 8, player.whoAmI, 90);

	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type) && proj.type != ModContent.ProjectileType<ThunderSpearThrowProjectile>() && proj.type != ProjectileID.ThunderSpearShot && !proj.minion) {
			if (++globalItem.Item_Counter2[index] < 15) {
				return;
			}
			globalItem.Item_Counter2[index] = 0;
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, -Vector2.UnitY.Vector2RotateByRandom(40) * Main.rand.NextFloat(7, 9), ModContent.ProjectileType<ThunderSpearThrowProjectile>(), hit.Damage * 3, 8, player.whoAmI, 90);
		}
	}
}
public class ThunderSpearThrowProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.ThunderSpear);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 52;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.timeLeft = 9999;
	}
	public override bool? CanDamage() {
		return Projectile.ai[0] < 0;
	}
	Vector2 toMouse = Vector2.Zero;
	public override void AI() {
		if (--Projectile.ai[0] == 0) {
			toMouse = Main.MouseWorld;
			Projectile.velocity = Vector2.Zero;
		}
		if (Projectile.ai[0] <= -1) {
			Projectile.velocity = (toMouse - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
			if (Projectile.ai[0] == -1) {
				for (int i = 0; i < 50; i++) {
					var rotate = Main.rand.NextVector2CircularEdge(10, 3.5f).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2) * .7f;
					int dust3 = Dust.NewDust(Projectile.Center.PositionOFFSET(Projectile.velocity, 50), 0, 0, DustID.GemDiamond, newColor: Color.Blue);
					Main.dust[dust3].noGravity = true;
					Main.dust[dust3].velocity = rotate;
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2;
			if (Projectile.Center.IsCloseToPosition(toMouse, 36)) {
				Projectile.Kill();
			}
		}
		else if (Projectile.ai[0] > 0) {
			if (Projectile.ai[0] <= 30) {
				Projectile.rotation = MathHelper.Lerp(Projectile.rotation, (Main.MouseWorld - Projectile.Center).ToRotation(), 1 - Projectile.ai[0] / 30f) + MathHelper.PiOver4 + MathHelper.PiOver2;
			}
			else {
				Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]);
			}
			Projectile.velocity *= .98f;
		}
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 16; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(16, 360, i) * 15, ProjectileID.ThunderSpearShot, Math.Max(Projectile.damage / 3, 1), 3f, Projectile.owner);
			projectile.alpha -= 100;
			projectile.penetrate = 5;
			projectile.maxPenetrate = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
	}
}
public class TheRottedFork : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TheRottedFork;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter2[index] = BossRushUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ModContent.ProjectileType<SwordProjectileSpear>() && proj.ModProjectile is SwordProjectileSpear spear) {
			if (spear.ItemIDtextureValue == ItemIDType) {
				target.AddBuff<TheRottedForkEnchantmentDebuff>(BossRushUtils.ToSecond(Main.rand.Next(2, 5)));
			}
		}
		if (globalItem.Item_Counter2[index] <= 0 && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
			globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 150);
			SpawnFork(target, player.HeldItem, player);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
			SpawnFork(target, item, player);
		}
	}
	public void SpawnFork(NPC target, Item item, Player player) {
		Vector2 pos = target.Center;
		int type = ModContent.ProjectileType<SwordProjectileSpear>();
		pos += Main.rand.NextVector2CircularEdge(target.width + 150, target.height + 150);
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero), type, player.GetWeaponDamage(item), item.knockBack, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is SwordProjectileSpear swordspearproj) {
			swordspearproj.ItemIDtextureValue = ItemIDType;
			Main.projectile[proj].Resize(ContentSamples.ItemsByType[ItemIDType].width, ContentSamples.ItemsByType[ItemIDType].height);
			Main.projectile[proj].velocity = (target.Center - pos).SafeNormalize(Vector2.Zero);
		}
	}
}
public class TheRottedForkEnchantmentDebuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 5 + npc.buffTime[buffIndex] / 120;
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.buffTime[buffIndex] += time;
		return true;
	}
}
public class BeeKeeper : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BeeKeeper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BeeKeeperEnchantmentProjectile>()] > 0) {
			player.AddBuff(BuffID.Honey, 120);
		}
		if (globalItem.Item_Counter1[index] >= 30) {
			globalItem.Item_Counter1[index] = 0;
			int bee = 0;
			if (player.strongBees) {
				bee = 1;
			}
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.UnitY * -5, ModContent.ProjectileType<BeeKeeperEnchantmentProjectile>(), player.GetWeaponDamage(item), 3f, player.whoAmI, bee);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BeeKeeperEnchantmentProjectile>()] > 0) {
			return;
		}
		globalItem.Item_Counter1[index]++;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BeeKeeperEnchantmentProjectile>()] > 0) {
			return;
		}
		if (proj.minion || !proj.Check_ItemTypeSource(player.HeldItem.type) || proj.Check_ProjTypeSource<BeeKeeperEnchantmentProjectile>()) {
			return;
		}
		globalItem.Item_Counter1[index]++;
	}
}
public class BeeKeeperEnchantmentProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BeeKeeper);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 50;
		Projectile.friendly = true;
		Projectile.timeLeft = 360;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
	}
	public override bool? CanDamage() {
		return Projectile.timeLeft < 10;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (!Projectile.Center.IsCloseToPosition(player.Center, 400)) {
			Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (player.Center - Projectile.Center).Length() / 128f;
		}
		if (Projectile.timeLeft < 10) {
			Projectile.Center.LookForHostileNPC(out NPC npc, 1500, true);
			if (++Projectile.ai[1] < 30) {
				Projectile.timeLeft = 9;
				if (npc != null) {
					Projectile.rotation = (npc.Center - Projectile.Center).ToRotation() + MathHelper.PiOver4;
				}
				return;
			}
			if (Projectile.ai[1] == 30) {
				for (int i = 0; i < 30; i++) {
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Honey);
					dust.noGravity = true;
					dust.color.A = 0;
					dust.velocity = Main.rand.NextVector2CircularEdge(15, 15);
				}
				for (int i = 0; i < 12; i++) {
					SpawnBee(Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(12, 360, i) * 8);
				}
			}
			if (npc != null) {
				Projectile.timeLeft = 9;
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			return;
		}
		Projectile.velocity *= .97f;
		Projectile.rotation = -MathHelper.PiOver4;
		if (Projectile.timeLeft % 4 != 0) {
			return;
		}
		SpawnBee(Projectile.Center + Main.rand.NextVector2CircularEdge(32, 32), Main.rand.NextVector2CircularEdge(8, 8));
	}
	public void SpawnBee(Vector2 position, Vector2 velocity) {
		int damage = 5 + Math.Max(Projectile.damage / 10, 1);
		int type = ProjectileID.Bee;
		int extra = 0;
		//Detecting whenever player have strong bee perk
		if (Projectile.ai[0] == 1) {
			if (Main.rand.NextBool(3)) {
				type = ProjectileID.GiantBee;
				damage += Math.Max(Projectile.damage / 7, 1) + 10;
			}
			damage += Math.Max(Projectile.damage / 10, 1);
			extra = 1;
		}
		Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), position, velocity, type, damage, 2f, Projectile.owner);
		proj.penetrate = 1;
		proj.extraUpdates = extra;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 24; i++) {
			Vector2 velocity = Vector2.One.Vector2DistributeEvenlyPlus(24, 360, i) * 10;
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.QueenBeeStinger, Projectile.damage, Projectile.knockBack, Projectile.owner);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].usesLocalNPCImmunity = true;
			Main.projectile[proj].localNPCHitCooldown = 10;
		}
	}
}
public class Mace : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Mace;
	}
}
public class FlamingMace : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlamingMace;
	}
}
public class BlueMoon : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BlueMoon;
	}
}
