using System;
using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.WeaponEnchantment;
public class AmethystStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AmethystStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		item.shoot = ProjectileID.AmethystBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .65f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			int proj = Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
			Main.projectile[proj].extraUpdates += 1;
		}
		if (Main.rand.NextFloat() <= .37f) {
			for (int i = 0; i < 3; i++) {
				Vector2 newVel = velocity.Vector2DistributeEvenlyPlus(3, 30, i);
				Projectile.NewProjectile(source, position, newVel, ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
			}
		}
	}
}
public class TopazStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TopazStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .25f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		item.shoot = ProjectileID.TopazBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .47f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.TopazBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class SapphireStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SapphireStaff;
	}
	public override void OnConsumeMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int consumedMana) {
		if (Main.rand.NextFloat() <= .18f) {
			player.statMana = Math.Clamp(player.statMana + consumedMana, 0, player.statManaMax2);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		item.shoot = ProjectileID.SapphireBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .37f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.SapphireBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class EmeraldStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EmeraldStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		item.shoot = ProjectileID.EmeraldBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .47f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			int proj = Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.EmeraldBolt, damage, knockback, player.whoAmI);
			if (Main.rand.NextFloat() <= .32f) {
				Main.projectile[proj].extraUpdates++;
				Main.projectile[proj].damage += (int)(damage * .5f);
			}
		}
	}
}
public class RubyStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RubyStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .2f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.RubyBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.RubyBolt, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(7);
		Vector2 TowardMouse = Main.MouseWorld - player.Center;
		for (int i = 0; i < 3; i++) {
			Vector2 vel = Vector2.UnitX.RotatedBy(TowardMouse.ToRotation()) * 10;
			Vector2 position = player.Center + vel.Vector2DistributeEvenlyPlus(3, 60, i);
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), position, vel, ProjectileID.RubyBolt, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			Main.projectile[proj].usesLocalNPCImmunity = true;
		}
		player.statMana += (int)(player.statManaMax2 * .5f);
	}
}
public class DiamondStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DiamondStaff;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.DiamondBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.DiamondBolt, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(4);
		for (int i = 0; i < 5; i++) {
			Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i) * 2;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.DiamondBolt, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
		}
	}
}
public class AmberStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AmberStaff;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.DamageType == DamageClass.Magic) {
			damage.Flat += 5;
			damage += .15f;
		}
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (item.shoot == ProjectileID.None) {
			item.shoot = ProjectileID.AmberBolt;
		}
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.AmberBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.AmberBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class WandOfSparking : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WandofSparking;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (proj.DamageType == DamageClass.Magic) {
			target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(3));
		}
		if (Main.rand.NextBool() && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.WandOfSparkingSpark, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
		}
	}
}
public class WandOfFrosting : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WandofFrosting;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (proj.DamageType == DamageClass.Magic) {
			target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(3));
		}
		if (Main.rand.NextBool() && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.WandOfFrostingFrost, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
		}
	}
}
public class WaterBolt : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WaterBolt;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .08f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			for (int i = 0; i < 3; i++) {
				int proj = Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(3, 12, i), ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
				Main.projectile[proj].timeLeft = BossRushUtils.ToSecond(3);
			}
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(2);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			return;
		}
		int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
			player.Center + Main.rand.NextVector2Circular(700, 700),
			Main.rand.NextVector2CircularEdge(4, 4), ProjectileID.WaterBolt, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
		Main.projectile[projectile].timeLeft = BossRushUtils.ToSecond(5);
		globalItem.Item_Counter2[index] = BossRushUtils.ToSecond(5);
	}
}
public class DemonScythe : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DemonScythe;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage *= 1.1f;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(3, 12, i), ProjectileID.DemonScythe, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
		}
	}
}
public class BookOfSkulls : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BookofSkulls;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(3)) {
			if (velocity == Vector2.Zero) {
				velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length();
			}
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
		}
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(5);
			int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(item.damage);
			for (int i = 0; i < 5; i++) {
				Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i) * 5;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.BookOfSkullsSkull, damage, item.knockBack, player.whoAmI);
			}
		}
	}
}
public class ThunderStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ThunderStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(150, 150);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 10, ProjectileID.ThunderStaffShot, player.GetWeaponDamage(player.HeldItem), 4, player.whoAmI);
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(0.2f);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(150, 150);
			Projectile.NewProjectile(player.GetSource_FromAI(), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 10, ProjectileID.ThunderStaffShot, player.GetWeaponDamage(player.HeldItem), 4, player.whoAmI);
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(0.2f);
		}
	}
}
public class ZapinatorGray : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ZapinatorGray;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (Main.rand.NextBool(20)) {
			damage += 100;
		}
		if (Main.rand.NextBool(20)) {
			damage *= 10;
		}
		if (Main.rand.NextBool(20)) {
			position = position.PositionOFFSET(velocity.Vector2RotateByRandom(360), Main.rand.Next(100, 150));
		}
		if (Main.rand.NextBool(20)) {
			velocity *= .1f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(20)) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
	}
}
public class SpaceGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SpaceGun;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		if (player.spaceGun) {
			multi *= 0;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 3) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
			if (globalItem.Item_Counter1[index] >= 6) {
				globalItem.Item_Counter1[index] = 0;
			}
		}
	}
}
public class BeeGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BeeGun;
	}
	public override void OnHitByAnything(Player player) {
		player.AddBuff(BuffID.Honey, 360);
		int damage = (int)(30 + player.GetWeaponDamage(player.HeldItem) * .1f);
		for (int i = 0; i < 5; i++) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2Circular(5, 5), ProjectileID.Bee, damage, 4f, player.whoAmI);
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int damage2 = (int)(20 + damage * .1f);
		if (Main.rand.NextBool(5) || player.strongBees && Main.rand.NextBool(2)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bee, damage2, knockback, player.whoAmI);
		}
	}
}
public class Vilethorn : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Vilethorn;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .05f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.VilethornBase && proj.type != ProjectileID.VilethornTip && globalItem.Item_Counter1[index] <= 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(14, 14);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornBase, player.GetWeaponDamage(player.HeldItem), 1f, player.whoAmI);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornTip, player.GetWeaponDamage(player.HeldItem), 1f, player.whoAmI);
			globalItem.Item_Counter1[index] = 60;
		}
	}
}
public class CrimsonRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrimsonRod;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive && globalItem.Item_Counter1[index] <= 0) {
			Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld.PositionOFFSET(Vector2.UnitX, Main.rand.NextFloat(-30, 30)), Vector2.UnitY * Main.rand.NextFloat(4, 12), ProjectileID.BloodRain, player.GetWeaponDamage(item), .2f, player.whoAmI);
			globalItem.Item_Counter1[index] = 9;
		}
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .08f;
	}
}
public class MagicMissile : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MagicMissile;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (item.shoot != ProjectileID.None) {
			return;
		}
		globalItem.Item_Counter2[index] = BossRushUtils.CountDown(globalItem.Item_Counter2[index]);
		if (!player.CheckMana(14, true)) {
			return;
		}
		if (player.ItemAnimationActive && globalItem.Item_Counter2[index] <= 0) {
			Vector2 velToMouse = (player.Center - Main.MouseWorld).SafeNormalize(Vector2.Zero);
			Vector2 positionBehindPlayer = player.Center.PositionOFFSET(velToMouse.Vector2RotateByRandom(25), Main.rand.Next(80, 110));
			Vector2 vel = (Main.MouseWorld - positionBehindPlayer).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), positionBehindPlayer, vel, ProjectileID.MagicMissile, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[proj].tileCollide = false;
			globalItem.Item_Counter2[index] = player.itemAnimationMax * 2;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 3) {
			if (!player.CheckMana(14, true)) {
				return;
			}
			Vector2 velocityNew = velocity.SafeNormalize(Vector2.Zero) * 14;
			Projectile.NewProjectile(source, position, velocityNew.Vector2RotateByRandom(30), ProjectileID.MagicMissile, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
		else {
			globalItem.Item_Counter1[index]++;
		}
	}
}
class Flamelash : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Flamelash;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(5)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Flamelash, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .33f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .33f;
		}
	}
}
class AquaScepter : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AquaScepter;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .09f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEnemy(player, target, (int)player.GetDamage(DamageClass.Magic).ApplyTo(item.damage * .5f), 3);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.WaterStream) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
				HitEnemy(player, target, (int)player.GetDamage(DamageClass.Magic).ApplyTo(proj.damage * .5f), 3);
			}
		}
		else {
			player.statMana += 10;
		}
	}
	public void HitEnemy(Player player, NPC target, int damage, float knockback) {
		if (!Main.rand.NextBool(3)) {
			return;
		}
		Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(10, 10) * 30;
		Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 15;
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
	}
}
class WeatherPain : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WeatherPain;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .11f;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.DamageType == DamageClass.Magic) {
			modifiers.SourceDamage += .12f;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ownedProjectileCounts[ProjectileID.WeatherPainShot] < 1 && player.ItemAnimationActive) {
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.WeatherPainShot, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[proj].timeLeft = 120;
		}
	}
}
class FlowerOfFire : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlowerofFire;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 2) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFire, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .27f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .27f;
		}
	}
}
