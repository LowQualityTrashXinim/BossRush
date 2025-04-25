using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.Transfixion.WeaponEnchantment.BroadSwordEnchantment;
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
