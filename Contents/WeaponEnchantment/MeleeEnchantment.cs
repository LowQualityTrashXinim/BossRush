using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.WeaponEnchantment;
public class WoodenSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class AshWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AshWoodSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class BorealWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BorealWoodSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class PalmWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalmWoodSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class RichMahoganySword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RichMahoganySword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class ShadewoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadewoodSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class EbonwoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EbonwoodSword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class CopperBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class TinBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class IronBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class LeadBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class SilverBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class TungstenBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class GoldBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
	}
}
public class PlatinumBroadsword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumBroadsword;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<WoodSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is WoodSwordProjectile woodproj)
			woodproj.ItemIDtextureValue = ItemIDType;
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
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(6, 8), ProjectileID.RollingCactus, player.GetWeaponDamage(item) * 3, 0, player.whoAmI);
			Main.projectile[projectile].friendly = true;
			Main.projectile[projectile].hostile = false;
			Main.projectile[projectile].penetrate = -1;
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(10);
		}
	}
}
