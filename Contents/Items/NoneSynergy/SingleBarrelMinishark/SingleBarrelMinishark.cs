﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.NoneSynergy.SingleBarrelMinishark {
	internal class SingleBarrelMinishark : ModItem {
		public override void SetDefaults() {
			Item.damage = 8;
			Item.knockBack = 4f;
			Item.useAmmo = AmmoID.Bullet;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.rare = ItemRarityID.Orange;

			Item.width = 82;
			Item.height = 20;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 15;
			Item.value = Item.buyPrice(gold: 50);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			var muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 70f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			if (player.velocity == Vector2.Zero) {
				Item.useTime = 4;
				Item.useAnimation = 4;
			}
			else {
				Item.useTime = 7;
				Item.useAnimation = 7;
			}
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-2f, -2f);
		}
	}
}
