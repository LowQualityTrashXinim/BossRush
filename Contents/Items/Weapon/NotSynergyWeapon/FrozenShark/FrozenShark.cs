using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.NotSynergyWeapon.FrozenShark {
	internal class FrozenShark : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(64, 20, 16, 1f, 8, 8, ItemUseStyleID.Shoot, ProjectileID.IceBolt, 12, true);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 50);
			velocity = velocity.Vector2RotateByRandom(3);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextBool(5)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(6), type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-2f, -3f);
		}
	}
}
