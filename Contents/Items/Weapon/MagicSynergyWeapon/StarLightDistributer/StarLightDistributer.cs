﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer {
	internal class StarLightDistributer : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.MagicMissile, $"[i:{ItemID.MagicMissile}] Shoot out magic missle");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.StarCannon, $"[i:{ItemID.StarCannon}] Create shooting star at your position");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMagic(45, 24, 16, 2f, 16, 16, ItemUseStyleID.Shoot, ProjectileID.GreenLaser, 10, 8, true);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item12;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.MagicMissile);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.StarCannon);
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) {
			if (BossRushUtils.Player_MeteoriteArmorSet(player)) {
				mult *= 0;
			}
		}
		public override Vector2? HoldoutOffset() => new Vector2(-2, 0);
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			position = position.PositionOFFSET(velocity, 30);
			float num = 1;
			if (Main.dayTime) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
				Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
			}
			else {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
				num = 5;
				for (int i = 0; i < num; i++) {
					Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 30, i);
					Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.ThunderStaffShot, (int)(damage * 1.25f), knockback, player.whoAmI);
				}
			}
			if (player.ZoneNormalSpace) {
				num = 5;
				for (int i = 0; i < num; i++) {
					Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 45, i);
					Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
				}
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.MagicMissile))
				for (int i = 0; i < num; i++) {
					Vector2 spread = velocity.Vector2DistributeEvenly(num, 60, i);
					Projectile.NewProjectile(source, position, spread, ProjectileID.MagicMissile, (int)(damage * 1.5f), knockback, player.whoAmI);
				}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.StarCannon))
				for (int i = 0; i < 3; i++) {
					int proj = Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(100, 100), velocity * Main.rand.NextFloat(1f, 2f), ProjectileID.StarCannonStar, damage * 2, knockback, player.whoAmI);
					Main.projectile[proj].timeLeft = 150;
				}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.SpaceGun)
				.AddIngredient(ItemID.ThunderStaff)
				.Register();
		}
	}
}
