﻿using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.TrueEnchantedSword {
	internal class TrueEnchantedSword : SynergyModItem {
		int count = 0;
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeShootCustomProjectile(100, 100, 150, 7f, 19, 19, ItemUseStyleID.Swing, ProjectileID.EnchantedBeam, 20, true);
			Item.crit = 30;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item1;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			count++;
			float num = 10f;
			float rotation = MathHelper.ToRadians(10);
			for (int i = 0; i < 6; i++) {
				Vector2 SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y - 800 + Main.rand.Next(-300, 100));
				Vector2 Aimto = Main.MouseWorld - SkyPos;
				Vector2 safeAim = Aimto.SafeNormalize(Vector2.UnitX);
				if (i != 5) {
					int proj = Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.Starfury, (int)(damage * 1.25f), knockback, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
				else {
					Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.SuperStar, (int)(damage * 1.25f), knockback, player.whoAmI);
				}
			}
			for (int i = 0; i < 6; i++) {
				Vector2 NewPos = position + Main.rand.NextVector2Circular(100f, 100f);
				Vector2 Aimto2 = Main.MouseWorld - NewPos;
				Vector2 safeAim = Aimto2.SafeNormalize(Vector2.UnitX);
				if (i != 5) {
					Projectile.NewProjectile(source, NewPos, safeAim * 10f, ProjectileID.EnchantedBeam, (int)(damage * 0.75), knockback, player.whoAmI);
				}
				else {
					Projectile.NewProjectile(source, NewPos, safeAim * 14f, ProjectileID.SuperStar, (int)(damage * 0.75), knockback, player.whoAmI);
				}
			}
			if (count == 5) {
				Projectile.NewProjectile(source, position, velocity * 0.3f, ModContent.ProjectileType<TrueEnchantedSwordBeam>(), damage * 2, knockback, player.whoAmI);
				count = 0;
			}
			for (int i = 0; i < num; i++) {
				Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
				Projectile.NewProjectile(source, position, rotate * 0.6f, ProjectileID.IceBolt, (int)(damage * 0.9), knockback, player.whoAmI);
			}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe().AddIngredient(ItemID.MoonLordPetItem).Register();
		}
	}
}
