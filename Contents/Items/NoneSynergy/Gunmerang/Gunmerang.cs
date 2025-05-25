using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy.Gunmerang {
	internal class Gunmerang : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(34, 26, 34, 1f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 14, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item11;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (player.ownedProjectileCounts[ProjectileID.WoodenBoomerang] < 1) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.WoodenBoomerang, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-2, 1);
		}
	}
}
