using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.NotSynergyWeapon.SnowballShotgunCannon {
	class SnowballShotgunCannon : ModItem {
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Snowball Shotgun");
			// Tooltip.SetDefault("weirdly deadly");
		}
		public override void SetDefaults() {
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Snowball;

			Item.width = 80;
			Item.height = 32;

			Item.damage = 10;
			Item.knockBack = 1.5f;

			Item.shoot = ProjectileID.SnowBallFriendly;
			Item.shootSpeed = 15f;

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 50);

			Item.useTime = 25;
			Item.useAnimation = 25;

			Item.useStyle = ItemUseStyleID.Shoot;

			Item.UseSound = SoundID.Item11;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			float projectileNum = 2 + Main.rand.Next(3);
			float rotation = MathHelper.ToRadians(5);
			for (int i = 0; i < projectileNum; i++) {
				Vector2 Rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (projectileNum - 1)));
				float RandomSpeedX = Main.rand.NextFloat(0.75f, 1f);
				float RandomSpeedY = Main.rand.NextFloat(0.75f, 1f);
				Projectile.NewProjectile(source, position.X, position.Y, Rotate.X * RandomSpeedX, Rotate.Y * RandomSpeedY, type, damage, knockback, player.whoAmI);
			}
			return true;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-20, 2);
		}
	}
}
