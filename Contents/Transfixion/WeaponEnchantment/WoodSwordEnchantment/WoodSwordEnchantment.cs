using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.WeaponEnchantment.WoodSwordEnchantment;

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
