using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Merciless {
	internal class Merciless : SynergyModItem, IRogueLikeRangeGun {
		public float OffSetPosition => 60;
		public float Spread { get; set; }
		public override void SetDefaults() {
			Item.BossRushDefaultRange(102, 26, 20, 5f, 15, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 8, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.crit = 10;
			Item.reuseDelay = 30;
			Item.scale -= 0.15f;
			Item.UseSound = SoundID.Item38;
			Spread = 20;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-26, 0);
		}
		int count = 0;
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.ExplosiveBullet;
			}
			for (int i = 0; i < 10; i++) {
				Vector2 velocityRotate = velocity.Vector2RotateByRandom(20).Vector2RandomSpread(3, 0.55f);
				Projectile.NewProjectile(source, position, velocityRotate, type, damage, knockback, player.whoAmI);
			}
			if (count == 0) {
				Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.CannonballFriendly, damage * 4, knockback, player.whoAmI);
				count++;
			}
			else {
				count = 0;
			}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.QuadBarrelShotgun)
				.Register();
		}
	}
}
