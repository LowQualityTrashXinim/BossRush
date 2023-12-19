using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange;
using BossRush.Common.Systems;

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
			if (Main.rand.NextBool(4))
				Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class Minishark : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Minishark;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextBool(10)) {
				type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
		}
		public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
			useSpeed += .05f;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class TheUndertaker : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TheUndertaker;
		}
		public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextBool(10)) {
				type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage += .1f;
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
	public class WoodenBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.WoodenBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class AshWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AshWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class BorealWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.BorealWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class RichMahoganyBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.RichMahoganyBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class EbonwoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.EbonwoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class ShadewoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.ShadewoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class PalmWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PalmWoodBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class CopperBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.CopperBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class TinBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TinBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class IronBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.IronBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class LeadBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.LeadBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class SilverBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.SilverBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class TungstenBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TungstenBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class GoldBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.GoldBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
	public class PlatinumBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PlatinumBow;
		}
		public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
			damage.Base += 5f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .15f;
			}
		}
		public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.2f;
			}
		}
	}
}
