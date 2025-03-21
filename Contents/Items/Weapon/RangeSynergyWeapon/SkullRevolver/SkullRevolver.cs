﻿using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver {
	internal class SkullRevolver : SynergyModItem {
		int counter = 0;
		public override void SetDefaults() {
			Item.BossRushDefaultRange(26, 52, 25, 3f, 10, 60, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, false, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.crit = 15;
			Item.reuseDelay = 57;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item41;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 10;
				weapon.OffSetPost = 50;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			SoundEngine.PlaySound(Item.UseSound);
			counter++;
			if (counter == 2) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
			}
			if (counter == 4) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.ClothiersCurse, damage, knockback, player.whoAmI);
				counter = 0;
			}
			CanShootItem = true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-3, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.Revolver)
			.AddIngredient(ItemID.BookofSkulls)
			.Register();
		}
	}
}
