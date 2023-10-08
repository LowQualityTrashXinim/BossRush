using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.NotSynergyWeapon.Gunmerang {
	internal class Gunmerang : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(34, 26, 34, 1f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.WoodenBoomerang, 14, true);
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-2, 1);
		}
	}
}