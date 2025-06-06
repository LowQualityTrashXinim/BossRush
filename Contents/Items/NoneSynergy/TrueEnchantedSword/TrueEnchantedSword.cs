﻿using BossRush.Common.RoguelikeChange;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy.TrueEnchantedSword {
	internal class TrueEnchantedSword : ModItem {
		int count = 0;
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeShootCustomProjectile(100, 100, 150, 7f, 19, 19, ItemUseStyleID.Swing, ProjectileID.EnchantedBeam, 20, true);
			Item.crit = 30;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item1;

			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
				global.SwingType = BossRushUseStyle.Swipe;
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			count++;
			float num = 10f;
			float rotation = MathHelper.ToRadians(10);
			for (int i = 0; i < 6; i++) {
				var SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y - 800 + Main.rand.Next(-300, 100));
				var Aimto = Main.MouseWorld - SkyPos;
				var safeAim = Aimto.SafeNormalize(Vector2.UnitX);
				if (i != 5) {
					int proj = Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.Starfury, (int)(damage * 1.25f), knockback, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
				else {
					Projectile.NewProjectile(source, SkyPos, safeAim * 15, ProjectileID.SuperStar, (int)(damage * 1.25f), knockback, player.whoAmI);
				}
			}
			for (int i = 0; i < 6; i++) {
				var NewPos = position + Main.rand.NextVector2Circular(100f, 100f);
				var Aimto2 = Main.MouseWorld - NewPos;
				var safeAim = Aimto2.SafeNormalize(Vector2.UnitX);
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
				var rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
				Projectile.NewProjectile(source, position, rotate * 0.6f, ProjectileID.IceBolt, (int)(damage * 0.9), knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
