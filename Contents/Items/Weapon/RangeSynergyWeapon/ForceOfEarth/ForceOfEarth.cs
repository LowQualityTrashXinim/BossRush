using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth {
	internal class ForceOfEarth : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.IceBlade, $"[i:{ItemID.IceBlade}] Your summoned bow have chance to shoot out super fast ice bolt");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Boomstick, $"[i:{ItemID.Boomstick}] Every 4th shoot, you shoot out a burst of arrow");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.IceBlade);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Boomstick);
		}
		int counter = 0;
		public override void SetDefaults() {
			Item.BossRushDefaultRange(26, 74, 22, 3, 10, 10, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 12, true, AmmoID.Arrow);
			Item.rare = ItemRarityID.Orange;
			Item.crit = 12;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item5;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-6, 0);
		}
		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				Item.noUseGraphic = true;
				Item.useStyle = ItemUseStyleID.None;
			}
			else {
				Item.noUseGraphic = false;
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			return base.CanUseItem(player);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (!player.ItemAnimationActive) {
				ForceOfEarthPlayer bowplayer = player.GetModPlayer<ForceOfEarthPlayer>();
				bool checkammo = player.HasAmmo(Item);
				foreach (var proj in bowplayer.ForceOfEarthProjectile) {
					if (proj.ModProjectile is BaseFOE modprojectile) {
						modprojectile.CanShootBecauseOfAmmo = checkammo;
					}
				}
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			player.AddBuff(ModContent.BuffType<EarthPower>(), 2);
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CopperBowP>()] < 1) {
				ForceOfEarthPlayer bowplayer = player.GetModPlayer<ForceOfEarthPlayer>();
				for (int i = 0; i < TerrariaArrayID.FoEProjectileCustom.Length; i++) {
					Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, new Vector2(0, 0), TerrariaArrayID.FoEProjectileCustom[i], player.GetWeaponDamage(Item), 0, player.whoAmI, 0, 0, i);
					bowplayer.ForceOfEarthProjectile.Add(projectile);
				}
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			ForceOfEarthPlayer bowplayer = player.GetModPlayer<ForceOfEarthPlayer>();
			float speed = velocity.Length();
			foreach (var proj in bowplayer.ForceOfEarthProjectile) {
				proj.ai[1] = type;
				proj.damage = damage;
				proj.knockBack = knockback;
				if (proj.ModProjectile is BaseFOE modprojectile) {
					modprojectile.CanShootBecauseOfAmmo = true;
					modprojectile.speed = speed;
				}
			}
			if (player.altFunctionUse != 2) {
				if (++counter >= 4) {
					counter = 0;
					if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Boomstick)) {
						for (int i = 0; i < 8; i++) {
							Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.67f, 1f), type, damage * 2, knockback, player.whoAmI);
						}
					}
				}
				Projectile.NewProjectile(source, position, velocity, type, damage * 2, knockback, player.whoAmI);
			}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CopperBow)
				.AddIngredient(ItemID.TinBow)
				.AddIngredient(ItemID.IronBow)
				.AddIngredient(ItemID.LeadBow)
				.AddIngredient(ItemID.SilverBow)
				.AddIngredient(ItemID.TungstenBow)
				.AddIngredient(ItemID.GoldBow)
				.AddIngredient(ItemID.PlatinumBow)
				.Register();
		}
	}
	public class ForceOfEarthPlayer : ModPlayer {
		public List<Projectile> ForceOfEarthProjectile = new();
		public override void Initialize() {
			ForceOfEarthProjectile = new();
		}
		public override void ResetEffects() {
			if (!Player.active) {
				return;
			}
			if (!Player.IsHeldingModItem<ForceOfEarth>()) {
				ForceOfEarthProjectile.Clear();
			}
		}
	}
}
