using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.NotSynergyWeapon.SnowballRifle {
	class SnowballRifle : ModItem {
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Snowball Rifle");
			// Tooltip.SetDefault("The legend, the legacy\nHave 67% to not consume ammo");
		}
		public override void SetDefaults() {
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Snowball;

			Item.width = 62;
			Item.height = 32;

			Item.damage = 11;
			Item.knockBack = 0.5f;

			Item.shoot = ProjectileID.SnowBallFriendly;
			Item.shootSpeed = 15f;

			Item.rare = 1;
			Item.value = Item.buyPrice(gold: 50);

			Item.useTime = 5;
			Item.useAnimation = 5;

			Item.useStyle = 5;

			Item.UseSound = SoundID.Item11;
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) {
			return Main.rand.NextFloat() >= 0.67f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			Vector2 RandSpread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
			Projectile.NewProjectile(source, position, RandSpread, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-8, 2);
		}
	}
}
