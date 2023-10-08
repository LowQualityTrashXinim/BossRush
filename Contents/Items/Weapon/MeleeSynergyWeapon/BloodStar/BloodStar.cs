using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BloodStar {
	internal class BloodStar : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(34, 34, 37, 5f, 17, 17, ItemUseStyleID.Swing, false);
			Item.DamageType = DamageClass.Melee;
			Item.rare = 3;
			Item.shoot = ProjectileID.StarCannonStar;
			Item.shootSpeed = 20;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 newPos = new Vector2(Main.MouseWorld.X, player.Center.Y - 800);
			Vector2 FallVelocity = (Main.MouseWorld - newPos).SafeNormalize(Vector2.UnitX) * 20;
			for (int i = 0; i < 10; i++) {
				Projectile.NewProjectile(source, newPos.X + Main.rand.Next(-100, 100), newPos.Y + Main.rand.Next(-10, 10), FallVelocity.X, FallVelocity.Y, ProjectileID.BloodArrow, damage, knockback, player.whoAmI);
			}
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, newPos.X + Main.rand.Next(-100, 100), newPos.Y + Main.rand.Next(-10, 10), FallVelocity.X, FallVelocity.Y, ProjectileID.StarCannonStar, damage, knockback, player.whoAmI);
			}
			return false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Starfury)
				.AddIngredient(ItemID.BloodRainBow)
				.Register();
		}
	}
}
