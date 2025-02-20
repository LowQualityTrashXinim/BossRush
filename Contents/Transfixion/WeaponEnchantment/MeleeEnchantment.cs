using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmberBoneSpear;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
public class WoodenSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 5;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .2f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
}
public class AshWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AshWoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = target.Center.Subtract(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(50 + target.height, 100 + target.height));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectileSpear>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectileSpear woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = target.Center.Subtract(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(50 + target.height, 100 + target.height));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectileSpear>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectileSpear woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class BorealWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BorealWoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class PalmWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalmWoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class RichMahoganySword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RichMahoganySword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class ShadewoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadewoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class EbonwoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EbonwoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class CopperBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Electrified)) {
			modifiers.SourceDamage += .34f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Electrified)) {
			modifiers.SourceDamage += .34f;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && Main.rand.NextBool(10)) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(3);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<ElectricChainBolt>(), hit.Damage, hit.Knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && Main.rand.NextBool(10)) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(3);
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<ElectricChainBolt>(), hit.Damage, hit.Knockback, player.whoAmI);
		}
	}
}
public class TinBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class IronBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class LeadBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class SilverBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class TungstenBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class GoldBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4)) {
			target.AddBuff(BuffID.Midas, BossRushUtils.ToSecond(6));
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4)) {
			target.AddBuff(BuffID.Midas, BossRushUtils.ToSecond(6));
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class PlatinumBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 45;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), proj.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
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
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(6);
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
			globalItem.Item_Counter1[index] = player.itemAnimationMax * 3;
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
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = player.itemAnimationMax * 3;
			Vector2 positionAbovePlayer = position + new Vector2(Main.rand.Next(-200, 200), -1000);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), positionAbovePlayer, (Main.MouseWorld - positionAbovePlayer).SafeNormalize(Vector2.Zero) * 10, ProjectileID.Starfury, (int)(damage * 1.5f), knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.shoot == ProjectileID.None && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1.5f);
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
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (player.ZoneSnow) {
			damage += .12f;
		}
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (player.ZoneSnow) {
			crit += 3;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			if (velocity.Length() < 5) {
				velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 5;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = item.useAnimation * 3;
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
			globalItem.Item_Counter1[index] = 12;//0.2 second
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
	private void BoneExplosion(int index, Player player, EnchantmentGlobalItem globalItem, Vector2 position, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0 && Main.rand.NextBool(4)) {
			globalItem.Item_Counter1[index] = 60;
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
		float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(500);
		player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
		foreach (NPC target in npclist) {
			player.StrikeNPCDirect(target, npc.CalculateHitInfo((int)(player.GetWeaponDamage(player.HeldItem) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
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
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
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
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		target.AddBuff(BuffID.TentacleSpike, 900);
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
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
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		Vector2 vel = Main.rand.NextVector2CircularEdge(5, 5);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), target.Center.PositionOFFSET(vel, -60), vel, ProjectileID.LightsBane, player.GetWeaponDamage(player.HeldItem), player.HeldItem.knockBack, player.whoAmI, 1);
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(3);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		Vector2 vel = Main.rand.NextVector2CircularEdge(5, 5);
		Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center.PositionOFFSET(vel, -60), vel, ProjectileID.LightsBane, player.GetWeaponDamage(item), player.HeldItem.knockBack, player.whoAmI, 1);
		globalItem.Item_Counter2[index] = BossRushUtils.ToSecond(1);
	}
}
public class BladeOfGrass : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BladeofGrass;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (player.ZoneJungle) {
			damage += .1f;
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
		target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(6));
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(125);
		player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(player.GetWeaponDamage(player.HeldItem) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		for (int i = 0; i < 35; i++) {
			int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(4);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(6));
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(125);
		player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(player.GetWeaponDamage(item) * .55f),
				BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		for (int i = 0; i < 35; i++) {
			int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		globalItem.Item_Counter2[index] = BossRushUtils.ToSecond(1);
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
public class CopperShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritDamage, 1.25f);
		player.GetModPlayer<GlobalItemPlayer>().ShortSword_ThrownCD *= 0;
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool() && hit.Crit && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 60;
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
		if (Main.rand.NextBool(10) && hit.Crit && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 60;
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
public class TinShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class IronShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class LeadShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class SilverShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class TungstenShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class GoldShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (hit.Crit) {
			target.AddBuff(BuffID.Midas, BossRushUtils.ToSecond(6));
		}
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class PlatinumShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 2;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<ShortSwordProjectile>()) {
			Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projectile].ModProjectile is ShortSwordProjectile shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
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
			globalItem.Item_Counter1[index] = 15;
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
					Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
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

	public override void Update(Player player) {
		player.GetDamage(DamageClass.Melee) += 0.2f;
		player.GetCritChance(DamageClass.Melee) += 5;
	}

	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(6));
	}


}

public class TheMeatball : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.TheMeatball;
	}

	public override void Update(Player player) {
		player.GetDamage(DamageClass.Melee) += 0.2f;
		player.GetCritChance(DamageClass.Melee) += 5;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, BossRushUtils.ToSecond(6));
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
