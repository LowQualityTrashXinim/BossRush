using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.LegacyItem.LongerMusket {
	internal class LongerMusket : ModItem {
		public override void SetDefaults() {
			Item.width = 80;
			Item.height = 20;

			Item.damage = 75;
			Item.crit = 35;
			Item.knockBack = 9f;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.shootSpeed = 40;

			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-13, 0);
		}
	}
}
