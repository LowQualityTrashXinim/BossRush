﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using BossRush.Contents.Projectiles;
using BossRush.Common.RoguelikeChange;
using BossRush.Common;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.WeaponEnchantment {
	public class Musket : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Musket;
		}
		public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= .25f;
			player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
		}
		public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
			crit += 5;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (++globalItem.Item_Counter1[index] >= 25) {
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
					Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(15);
					Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, ammoID), player.Center, vel * speed, type, damage, knockback, player.whoAmI);
					globalItem.Item_Counter1[index] = 8;
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
	}
	public class WoodenBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.WoodenBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity *= multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class AshWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AshWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity *= multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class BorealWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.BorealWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity += velocity.SafeNormalize(Vector2.Zero) * multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class RichMahoganyBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.RichMahoganyBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity += velocity.SafeNormalize(Vector2.Zero) * multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class EbonwoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.EbonwoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity += velocity.SafeNormalize(Vector2.Zero) * multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class ShadewoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.ShadewoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity += velocity.SafeNormalize(Vector2.Zero) * multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
	}
	public class PalmWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PalmWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 1;
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float multiply = 1.1f;
			if (item.useAmmo == AmmoID.Arrow) {
				multiply += .1f;
			}
			velocity += velocity.SafeNormalize(Vector2.Zero) * multiply;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
			globalItem.Item_Counter1[index] = 12;
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
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (player.ZoneCorrupt) {
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorruptionTrail>(), damage, knockback, player.whoAmI);
			}
		}
	}
	public class TendonBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TendonBow;
		}
		public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
			globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		}
		public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.minion) {
				return;
			}
			if (globalItem.Item_Counter1[index] <= 0) {
				target.AddBuff(BuffID.Ichor, 240);
				globalItem.Item_Counter1[index] = 60;
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
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(4);
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
					Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(3, 30, i), ProjectileID.PainterPaintball, (int)(damage * .45f), knockback, player.whoAmI, 0, Main.rand.NextFloat());
				}
				globalItem.Item_Counter1[index] = 60;
			}
		}
	}
}
