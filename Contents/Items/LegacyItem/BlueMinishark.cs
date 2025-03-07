using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.LegacyItem {
	class BlueMinishark : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(64, 20, 10, 4f, 8, 8, ItemUseStyleID.Shoot, ProjectileID.Bullet, 9f, true, AmmoID.Bullet);
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new(-5f, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			position = position.PositionOFFSET(velocity, 50);
			int amount = Main.rand.Next(1, 4);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
