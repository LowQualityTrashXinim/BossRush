using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Projectiles;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Transfixion.WeaponEnchantment;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
public class Musket : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Musket;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().Range_CritDamage += .25f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 15) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			int proj = Projectile.NewProjectile(source, position, velocity * 2f, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].CritChance = 101;
			globalItem.Item_Counter1[index] = 0;
		}
	}
}
public class FlintlockPistol : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlintlockPistol;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(4)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.Bullet || proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			target.AddBuff(ModContent.BuffType<Marked>(), BossRushUtils.ToSecond(10));
		}
	}
}
public class Revolver : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Revolver;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(5)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter2[index] = BossRushUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .05f * globalItem.Item_Counter1[index];
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5 * globalItem.Item_Counter1[index];
	}
	public override void OnHitByNPC(int index, EnchantmentGlobalItem globalItem, Player player, NPC npc, Player.HurtInfo hurtInfo) {
		globalItem.Item_Counter1[index] = 0;
	}
	public override void OnHitByProjectile(int index, EnchantmentGlobalItem globalItem, Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		globalItem.Item_Counter1[index] = 0;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 6);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 6);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
	}
}
public class Minishark : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Minishark;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {
				int type = ProjectileID.Bullet;
				if (player.PickAmmo(item, out int proj, out float speed, out int damage, out float knockback, out int ammoID)) {
					if (item.useAmmo == AmmoID.Bullet) {
						type = proj;
					}
				}
				if (speed < 3) {
					speed = 7;
				}
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(15);
				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, ammoID), player.Center, vel * speed, type, damage, knockback, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 8);
			}
		}
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed += .05f;
	}
}
public class TheUndertaker : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TheUndertaker;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(4)) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.TheUndertaker) {
			if (Main.rand.NextBool(10)) {
				player.Heal(1);
			}
			target.AddBuff(ModContent.BuffType<CrimsonAbsorbtion>(), 240);
		}
	}
}
public class Boomstick : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Boomstick;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 7) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(source, position,
					velocity.Vector2RandomSpread(2, Main.rand.NextFloat(.9f, 1.1f)).Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
}
public class QuadBarrelShotgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.QuadBarrelShotgun;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 21) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			for (int i = 0; i < 6; i++) {
				Projectile.NewProjectile(source, position,
					velocity.Vector2RandomSpread(2, Main.rand.NextFloat(.8f, 1.15f)).Vector2RotateByRandom(40), type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .12f;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		++globalItem.Item_Counter1[index];
	}
}
public class Handgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Handgun;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool()) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.Bullet || proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			target.AddBuff(ModContent.BuffType<Marked>(), BossRushUtils.ToSecond(10));
		}
	}
}
public class Marked : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
}
public class CopperBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class TinBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class IronBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class LeadBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class SilverBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class TungstenBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class GoldBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class PlatinumBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, damageDone, 2, player.whoAmI, 0, 0, 9999);
	}
}
public class DemonBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DemonBow;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.useAmmo == AmmoID.Arrow && type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.UnholyArrow;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.FullHPDamage, Additive: 2.5f);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (player.ZoneCorrupt) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorruptionTrail>(), damage, knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.UnholyArrow && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && Main.rand.NextFloat() <= .15f) {
			target.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(4));
		}
	}
}
public class TendonBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TendonBow;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (globalItem.Item_Counter3[index] == 0) {
			player.statLife = Math.Clamp(player.statLife - 5, 100, player.statLifeMax2);
			damage = (int)(damage * 1.35);
			for (int i = 0; i < 15; i++) {
				Vector2 vec = Main.rand.NextVector2Circular(7.5f, 7.5f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Crimson, Scale: Main.rand.NextFloat(.75f, 1.25f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = vec;
			}
			globalItem.Item_Counter3[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(3));
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter3[index] = BossRushUtils.CountDown(globalItem.Item_Counter3[index]);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.FullHPDamage, Additive: 2.5f);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion) {
			return;
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			target.AddBuff(BuffID.Ichor, 240);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
			if (++globalItem.Item_Counter2[index] >= 5) {
				player.Heal(1);
				globalItem.Item_Counter2[index] = 0;
			}
		}
	}
}
public class SnowballCannon : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SnowballCannon;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed += .05f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), ProjectileID.SnowBallFriendly, (int)(damage * .55f), knockback, player.whoAmI);
	}
}
public class Blowpipe : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Blowpipe;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (globalItem.Item_Counter1[index] <= 0) {
			crit += 15;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, BossRushUtils.ToSecond(4));
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), ProjectileID.Seed, (int)(damage * .35f), knockback, player.whoAmI);
	}
}
public class PainterPaintballGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PainterPaintballGun;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 30, i), ProjectileID.PainterPaintball, (int)(damage * .45f), knockback, player.whoAmI, 0, Main.rand.NextFloat());
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		}
	}
}
public class RedRyder : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RedRyder;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().Range_CritDamage += .15f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 7) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			int proj = Projectile.NewProjectile(source, position, velocity * 2f, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].CritChance = 101;
			globalItem.Item_Counter1[index] = 0;
		}
	}
}
public class BloodRainBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BloodRainBow;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RangeDMG, 1.05f);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Vector2 pos = Main.rand.NextVector2Circular(25, 300).Add(0, Main.rand.NextFloat(-100, 100) + 800) + Main.MouseWorld;
		Vector2 vel = (Main.MouseWorld + Main.rand.NextVector2Circular(20, 20) - pos).SafeNormalize(Vector2.Zero) * (item.shootSpeed > 0 ? item.shootSpeed : 5);
		Projectile.NewProjectile(source, pos, vel, ProjectileID.BloodArrow, (int)(damage * .85f), knockback, player.whoAmI);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		IEntitySource entitySource = player.GetSource_ItemUse(item);
		int damage = (int)(hit.Damage * .85f);
		for (int i = 0; i < 3; i++) {
			Vector2 pos = Main.rand.NextVector2Circular(25, 100).Add(0, Main.rand.NextFloat(-100, 100) + 800) + Main.MouseWorld;
			Vector2 vel = (pos - Main.MouseWorld).SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(entitySource, pos, vel, ProjectileID.BloodArrow, damage, hit.Knockback, player.whoAmI);
		}
	}
}
public class MoltenFury : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MoltenFury;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, 1.15f);
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.FireArrow;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (velocity.Length() < 3) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
		}
		for (int i = 0; i < 2; i++) {
			int proj = Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(40, 40), velocity, ProjectileID.FireArrow, damage, knockback, player.whoAmI);
			Main.projectile[proj].extraUpdates += 1;
		}
	}
}
public class BeeKnees : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BeesKnees;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RangeDMG, 1.12f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 3);
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.BeeArrow;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType != player.HeldItem.type) {
			return;
		}

		target.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(8, 15)));

		if (Main.rand.NextBool(3)) {
			int amount = Main.rand.Next(1, 4);
			float speed = proj.velocity.Length();
			int damage = (int)(proj.damage * .77f);
			if (player.strongBees) {
				damage = (int)(damage * 1.12f);
				amount += 2;
				speed *= 1.5f;
			}
			for (int i = 0; i < amount; i++) {
				Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height) * 1.2f;
				Vector2 vel = (pos - target.Center).SafeNormalize(Vector2.Zero) * speed;
				Projectile.NewProjectile(proj.GetSource_OnHit(target), pos, vel, ProjectileID.Bee, damage, proj.knockBack, player.whoAmI);
			}
		}
	}
}
public class ClockworkAssaultRifle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ClockworkAssaultRifle;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ItemAnimationJustStarted) {
			globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 11);
		}
		if (globalItem.Item_Counter2[index] == 1) {
			SoundEngine.PlaySound(item.UseSound);
			for (int i = 0; i < 3; i++) {
				Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(20) * ((item.shootSpeed > 3 ? item.shootSpeed : 4) + Main.rand.NextFloat(1, 3));
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, item.shoot, item.damage, item.knockBack, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
			globalItem.Item_Counter2[index] = 0;
		}
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		if (globalItem.Item_Counter2[index] == 0 && globalItem.Item_Counter1[index] > 3) {
			if (globalItem.Item_Counter1[index] >= 7) {
				globalItem.Item_Counter2[index] = 1;
			}
			useSpeed += 2;
		}
		else {
			useSpeed += .2f;
		}
	}
}

public class SandGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Sandgun;
	}

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index]--;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: 10);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .35f) {
			if (!velocity.IsLimitReached(7)) {
				velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
			}
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SandProjectile>(), damage / 2, 0, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.life - damageDone <= 0 && globalItem.Item_Counter1[index] <= 0)
			for (int i = 0; i < 15; i++) {
				Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Circular(15, 15), ModContent.ProjectileType<SandProjectile>(), 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 5);
			}
	}

	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.life - damageDone <= 0 && globalItem.Item_Counter1[index] <= 0)
			for (int i = 0; i < 15; i++) {
				Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Circular(15, 15), ModContent.ProjectileType<SandProjectile>(), 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 5);
			}
	}
}

public class PalladiumRepeater : PalladiumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.PalladiumRepeater;
	}

}

public class OrichalcumRepeater : OrichalcumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.OrichalcumRepeater;
	}

}

public class TitaniumRepeater : TitaniumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.TitaniumRepeater;
	}

}

public class OnyxBlaster : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.OnyxBlaster;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (velocity.IsLimitReached(10)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 10;
		}
		type = ModContent.ProjectileType<OnyxBulletProjectile>();
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int chance = Math.Max(1, player.itemAnimationMax - 50);
		if (Main.rand.NextBool(chance)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback * 3, player.whoAmI);
		}
	}
}
public class PhoenixBlaster : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PhoenixBlaster;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ItemAnimationActive && globalItem.Item_Counter1[index] >= 90) {
			globalItem.Item_Counter1[index] = -player.itemAnimationMax;
			if (player.ItemAnimationJustStarted) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15, ProjectileID.DD2PhoenixBowShot, player.GetWeaponDamage(item) * 3, 8f, player.whoAmI);
			}
			return;
		}
		if (++globalItem.Item_Counter1[index] > 90) {
			globalItem.Item_Counter1[index] = 90;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter2[index] >= 10) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * 1.5f), knockback, player.whoAmI);
			globalItem.Item_Counter2[index] = -1;
		}
	}
}
public class StarCannon : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.StarCannon;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (++globalItem.Item_Counter2[index] >= 5) {
			globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		}
		if(globalItem.Item_Counter1[index] >= 100) {
			player.GetDamage(DamageClass.Generic) += .15f;
			player.GetCritChance(DamageClass.Generic) += 5;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 200) {
			globalItem.Item_Counter1[index] -= 200;
			damage = 100 + (int)(damage * .35f);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.StarCannonStar, damage, knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
			globalItem.Item_Counter1[index] += hit.Damage;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		globalItem.Item_Counter1[index] += hit.Damage;
	}
}
