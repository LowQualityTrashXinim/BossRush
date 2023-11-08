using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.DupeSynergy {
	internal class ComboConceptSword : ModItem {
		public override string Texture => BossRushUtils.GetTheSameTextureAs<ComboConceptSword>("GenericDupeSword");
		public override void SetDefaults() {
			Item.width = 44;
			Item.height = 44;

			Item.damage = 20;
			Item.knockBack = 3;
			Item.crit = 10;

			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAmmo = AmmoID.Arrow;
			Item.shootSpeed = 10;
		}
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.mouseLeft) {
				player.GetModPlayer<ComboPlayer>().m1Counter++;
			}
			if (Main.mouseRight) {
				player.GetModPlayer<ComboPlayer>().m2Counter++;
			}
			switch (player.GetModPlayer<ComboPlayer>().ReturnComboType()) {
				case 1:
					Projectile.NewProjectile(source, position, velocity, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
					break;
				case 2:
					Projectile.NewProjectile(source, position, velocity, ProjectileID.Fireball, damage, knockback, player.whoAmI);
					break;
				case 3:
					Projectile.NewProjectile(source, position, velocity, ProjectileID.UnholyArrow, damage, knockback, player.whoAmI);
					break;
				case 4:
					Projectile.NewProjectile(source, position, velocity, ProjectileID.LaserMachinegun, damage, knockback, player.whoAmI);
					break;
				default:
					break;
			}
			return false;
		}
	}
	public class ComboPlayer : ModPlayer {
		public int m1Counter = 0;
		public int m2Counter = 0;

		public int ReturnComboType() {
			if (m1Counter == 1 && m2Counter == 0) {
				m1Counter = 0;
				m2Counter = 0;
				return 1;
			}
			if (m1Counter == 0 && m2Counter == 1) {
				m1Counter = 0;
				m2Counter = 0;
				return 2;
			}
			if (m1Counter == 1 && m2Counter == 1) {
				m1Counter = 0;
				m2Counter = 0;
				return 3;
			}
			if (m1Counter == 2 && m2Counter == 0) {
				m1Counter = 0;
				m2Counter = 0;
				return 4;
			}
			if (m1Counter == 0 && m2Counter == 2) {
				m1Counter = 0;
				m2Counter = 0;
				return 5;
			}
			return 0;
		}
	}
}
