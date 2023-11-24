using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.WeaponEnchantment {
	public class Musket : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Musket;
		}
		public override void Update(Player player) {
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= .25f;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 5;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class FlintlockPistol : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.FlintlockPistol;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextBool(10))
				Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class Minishark : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Minishark;
		}
		public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
			useSpeed += .05f;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class TheUndertaker : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TheUndertaker;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextBool(10)) {
				type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class Boomstick : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Boomstick;
		}
		int counter = 0;
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (++counter >= 15) {
				type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
				for (int i = 0; i < 4; i++) {
					Projectile.NewProjectile(source, position, 
						velocity.Vector2RandomSpread(2,Main.rand.NextFloat(.9f, 1.1f)).Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI);
				}
				counter = 0;
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
	}
	public class WoodenSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.WoodenSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class AshWoodSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AshWoodSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class BorealWoodSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.BorealWoodSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class PalmWoodSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PalmWoodSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class RichMahoganySword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.RichMahoganySword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class ShadewoodSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.ShadewoodSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class EbonwoodSword : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.EbonwoodSword;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			scale += .1f;
		}
	}
	public class WoodenBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.WoodenBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if(item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if(item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class AshWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AshWoodBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class BorealWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.BorealWoodBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class RichMahoganyBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.RichMahoganyBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class EbonwoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.EbonwoodBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class ShadewoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.ShadewoodBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class PalmWoodBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PalmWoodBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class CopperBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.CopperBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class TinBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TinBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class IronBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.IronBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class LeadBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.LeadBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class SilverBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.SilverBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class TungstenBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TungstenBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class GoldBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.GoldBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class PlatinumBow : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.PlatinumBow;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .05f;
			if (item.DamageType == DamageClass.Ranged) {
				damage += .1f;
			}
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 1.1f;
			}
		}
	}
	public class AmethystStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AmethystStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.AmethystBolt;
		}
	}
	public class TopazStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TopazStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.TopazBolt;
		}
	}
	public class SapphireStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.SapphireStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.SapphireBolt;
		}
	}
	public class EmeraldStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.EmeraldStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.EmeraldBolt;
		}
	}
	public class RubyStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.RubyStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.RubyBolt;
		}
	}
	public class DiamondStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.DiamondStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.DiamondBolt;
		}
	}
	public class AmberStaff : ModEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.AmberStaff;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			reduce -= .05f;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ProjectileID.AmberBolt;
		}
	}
}
