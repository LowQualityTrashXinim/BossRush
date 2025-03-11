using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Projectiles;
using BossRush.Common.Systems;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
			Vector2 position = player.Center + Vector2.One.Vector2DistributeEvenlyPlus(3, 60, i) * 50;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), position, vel, ProjectileID.RubyBolt, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			Main.projectile[proj].usesLocalNPCImmunity = true;
		}
		int manaheal = (int)(player.statManaMax2 * .5f);
		player.ManaEffect(manaheal);
		player.statMana += manaheal;
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
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
				velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 10;
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
			modifiers.ArmorPenetration += 10;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(20)) {
			Item item = player.HeldItem;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.WeatherPainShot, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectile].timeLeft = 120;
		}
	}
}
class FlowerOfFire : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlowerofFire;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			if (player.CheckMana(5, true, true)) {
				damage += 20;
			}
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
public class SkyFacture : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SkyFracture;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MagicDMG, 1.22f);
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 12;
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(40, 50), Main.rand.NextFloat(40, 50));
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(7, 9);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.SkyFracture, 22 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 12;
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(40, 50), Main.rand.NextFloat(40, 50));
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(7, 9);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.SkyFracture, 22 + item.damage, item.knockBack, player.whoAmI);
		}
	}
}
public class CrystalSerpent : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrystalSerpent;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MagicDMG, 1.19f);
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0 && item.DamageType == DamageClass.Magic) {
			globalItem.Item_Counter1[index] = 42;
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(40, 50), Main.rand.NextFloat(40, 50));
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(7, 9);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.CrystalPulse, 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!proj.Check_ItemTypeSource(player.HeldItem.type)) {
			return;
		}
		if (Main.rand.NextBool(10)) {
			Item item = player.HeldItem;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 500), Main.rand.NextFloat(300, 500)), Vector2.Zero, ModContent.ProjectileType<CrystalSerpentProjectile>(), 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 500), Main.rand.NextFloat(300, 500)), Vector2.Zero, ModContent.ProjectileType<CrystalSerpentProjectile>(), 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
}
public class CrystalSerpentProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CrystalSerpent);
	public override void SetDefaults() {
		Projectile.width = 40;
		Projectile.height = 48;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
	}
	public override bool? CanDamage() {
		return false;
	}
	int TimeLeft = 300;
	NPC npc = null;
	bool OneTimeTimeLeftReset = true;
	public override void AI() {
		if (++Projectile.ai[1] <= 1) {
			for (int i = 0; i < 50; i++) {
				Dust dust1 = Dust.NewDustDirect(Projectile.Center, 0, 0, Main.rand.NextBool() ? DustID.CrystalPulse : DustID.CrystalPulse2);
				dust1.velocity = Main.rand.NextVector2CircularEdge(3, 5) * Main.rand.NextFloat(1, 3);
				dust1.position += Main.rand.NextVector2Circular(5, 5);
				dust1.noGravity = true;
			}
		}
		if (npc == null) {
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 1000)) {
				this.npc = npc;
			}
			return;
		}
		else {
			if (!npc.active || npc.life <= 0) {
				npc = null;
				return;
			}
		}
		Projectile.spriteDirection = BossRushUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X);
		Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();
		Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 + MathHelper.PiOver2 : MathHelper.PiOver4;
		Point point = Projectile.position.ToTileCoordinates();
		if (!WorldGen.TileEmpty(point.X, point.Y) || !Projectile.Center.IsCloseToPosition(npc.Center, 150 + npc.Size.Length())) {
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (npc.Center - Projectile.Center).Length() / 64f;
			if (OneTimeTimeLeftReset) {
				Projectile.timeLeft = TimeLeft;
			}
		}
		else {
			OneTimeTimeLeftReset = false;
			if (++Projectile.ai[0] >= 45) {
				Vector2 vel = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(vel, 30), vel * 15, ProjectileID.CrystalPulse, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Projectile.ai[0] = 0;
			}
			Projectile.velocity *= .9f;
		}
		Dust dust = Dust.NewDustDirect(Projectile.Center.IgnoreTilePositionOFFSET(
			(Projectile.rotation + (Projectile.spriteDirection == 1 ? -MathHelper.PiOver4 : -MathHelper.PiOver2 - MathHelper.PiOver4)).ToRotationVector2(), 24), 0, 0, Main.rand.NextBool() ? DustID.CrystalPulse : DustID.CrystalPulse2);
		dust.velocity = -Vector2.UnitY.Vector2RotateByRandom(3) * Main.rand.NextFloat(1, 3);
		dust.position += Main.rand.NextVector2Circular(5, 5);
		dust.noGravity = true;
	}
}
public class FlowerOfFrost : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlowerofFrost;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			if (player.CheckMana(5, true, true)) {
				damage += 20;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .27f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .27f;
		}
	}
}
/// <summary>
/// This is a example for mod enchantment and how to utilize most of the stuff
/// </summary>
class DirtBlock : ModEnchantment {
	public override void SetDefaults() {
		//This is important as it is required for the enchantment to be recognized and work
		ItemIDType = ItemID.DirtBlock;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		//We can add very basic stats increases, here we gonna increases player's defense by 10 when player held this item
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: 10);
		//This will teach you the what the index and the globalItem do and why it exist here ( tho mainly for ease of uses )
		//The following effect will spawn a ring of 12 dirt projectiles for every 1s
		if (++globalItem.Item_Counter1[index] >= BossRushUtils.ToSecond(5)) {
			//the globalItem.ItemCounter1 is a array of counter that applied to specific enchantment slot, the index is the index of this enchantment
			//Now we spawn a ring of dirt
			for (int i = 0; i < 12; i++) {
				Vector2 rotate = Vector2.UnitX.Vector2DistributeEvenlyPlus(12, 360, i) * 6f;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, rotate, ModContent.ProjectileType<DirtProjectile>(), 20, 3f, player.whoAmI);
			}
			//Now we reset the counter of this enchantment
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		//This is a enchantment hack with enchantment slot ID, do this only if you know the in and out of the system
		//The following code attempt to activate enchantment that have OnHit effect with NPC but only if the projectile type is this mod Dirt Projectile
		if (proj.type != ModContent.ProjectileType<DirtProjectile>()) {
			return;
		}
		//Iterating through enchantment array of the item
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			//we skip this index cause we don't want to cause infinite loop
			if (i == index) {
				continue;
			}
			//The Enchantment array of this item consist of ItemID, so it make sense to use EnchantmentLoader.GetEnchantmentItemID(int)
			ModEnchantment enchantment = EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]);
			//We are checking if the enchantment exist or if the enchantment is the same as this enchantment, preventing null exception and infinite loop
			if (enchantment == null || enchantment.ItemIDType == ItemIDType) {
				continue;
			}
			//Activate the enchantment effect
			enchantment.OnHitNPCWithProj(i, player, globalItem, proj, target, hit, damageDone);
		}
	}
}
