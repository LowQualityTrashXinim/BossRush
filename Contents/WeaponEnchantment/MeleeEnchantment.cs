using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.WeaponEnchantment;
public class WoodenSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenSword;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class AshWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AshWoodSword;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
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
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(1);
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 90;
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
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(10);
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
			Projectile.NewProjectile(player.GetSource_ItemUse(item), position, velocity, ProjectileID.EnchantedBeam, (int)(damage * 1.25f), knockback, player.whoAmI);
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
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 5, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
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
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeSteal(index, player, globalItem);
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
public class CopperShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperShortsword;
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
