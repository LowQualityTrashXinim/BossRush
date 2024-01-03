using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection;

namespace BossRush.Contents.Items.Accessories.Crystal {
	class NatureCrystal : ModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statLifeMax2 += 40;
			player.statManaMax2 += 40;
			player.GetModPlayer<NatureSelectionPlayer>().PowerUP = player.HeldItem.type == ModContent.ItemType<NatureSelection>();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (Main.LocalPlayer.GetModPlayer<NatureSelectionPlayer>().PowerUP) {
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ModContent.ItemType<NatureSelection>()}] Nature Selection will also shoot out star and heart"));
			}
		}
	}
	class NatureSelectionPlayer : ModPlayer {
		public bool PowerUP;

		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (PowerUP) {
				for (int i = 0; i < 2; i++) {
					var RandomPos = position + Main.rand.NextVector2Circular(100f, 100f);
					var Aimto = (Main.MouseWorld - RandomPos).SafeNormalize(Vector2.UnitX) * 15;
					if (i == 1) Projectile.NewProjectile(source, RandomPos, Aimto, ModContent.ProjectileType<HeartP>(), damage, knockback, Player.whoAmI);
					else Projectile.NewProjectile(source, RandomPos, Aimto, ProjectileID.StarCannonStar, damage, knockback, Player.whoAmI);
				}
			}
			return true;
		}
		public override void ResetEffects() {
			PowerUP = false;
		}
	}
}
