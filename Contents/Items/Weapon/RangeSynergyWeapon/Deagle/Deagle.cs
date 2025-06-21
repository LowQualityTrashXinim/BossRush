using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Deagle {
	internal class Deagle : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.PhoenixBlaster, $"[i:{ItemID.PhoenixBlaster}] You shoot out additional bullet but at a random position, getting crit will make the next shot shoot out a fire phoenix dealing quadruple damage");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.DaedalusStormbow, $"[i:{ItemID.DaedalusStormbow}] Upon critical hit, storm of bullet fly down at target, have a 10 second cool down");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(56, 30, 70, 5f, 21, 21, ItemUseStyleID.Shoot, ProjectileID.Bullet, 40, false, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 1000);
			Item.scale -= 0.25f;
			Item.UseSound = SoundID.Item38;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 0;
				weapon.OffSetPost = 50;
			}
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.PhoenixBlaster);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.DaedalusStormbow);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			var weapon = Item.GetGlobalItem<RangeWeaponOverhaul>();
			if (player.velocity != Vector2.Zero) {
				weapon.SpreadAmount = 120;
			}
			else {
				weapon.SpreadAmount = 0;
				velocity *= 1.5f;
				damage = (int)(damage * 1.5f);
				knockback *= 2f;
				if (type == ProjectileID.Bullet) {
					type = ProjectileID.BulletHighVelocity;
				}
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.PhoenixBlaster)) {
				Vector2 position2 = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 300, 300);
				if (player.GetModPlayer<DeaglePlayer>().Deagle_PhoenixBlaster_Critical) {
					Projectile.NewProjectile(source, position, (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * Item.shootSpeed, ProjectileID.DD2PhoenixBowShot, damage * 4, knockback, player.whoAmI);
					int proj = Projectile.NewProjectile(source, position2, (Main.MouseWorld - position2).SafeNormalize(Vector2.Zero) * Item.shootSpeed, ProjectileID.DD2PhoenixBowShot, damage * 2, knockback, player.whoAmI);
					Main.projectile[proj].scale = .5f;
					Main.projectile[proj].width = (int)(Main.projectile[proj].width * .5f);
					Main.projectile[proj].height = (int)(Main.projectile[proj].height * .5f);
					player.GetModPlayer<DeaglePlayer>().Deagle_PhoenixBlaster_Critical = false;
					CanShootItem = false;
					return;
				}
				Projectile.NewProjectile(source, position2, (Main.MouseWorld - position2).SafeNormalize(Vector2.Zero) * Item.shootSpeed, type, damage, knockback, player.whoAmI);
			}
			CanShootItem = true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-5, 2);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Handgun)
				.AddIngredient(ItemID.Musket)
				.Register();
		}
	}
	public class DeaglePlayer : ModPlayer {
		public int Deagle_DaedalusStormBow_coolDown = 0;
		public bool Deagle_PhoenixBlaster_Critical = false;
		public override void PostUpdate() {
			if (Player.HeldItem.type == ModContent.ItemType<Deagle>()) {
				if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<Deagle>(), ItemID.DaedalusStormbow)) {
					Deagle_DaedalusStormBow_coolDown = BossRushUtils.CountDown(Deagle_DaedalusStormBow_coolDown);
				}
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.Crit) {
				if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<Deagle>(), ItemID.PhoenixBlaster)) {
					Deagle_PhoenixBlaster_Critical = true;
				}
				if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<Deagle>(), ItemID.DaedalusStormbow)
					&& Deagle_DaedalusStormBow_coolDown <= 0) {
					for (int i = 0; i < 15; i++) {
						Vector2 positionAboveSky = target.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-1100, -1000));
						int projectile = Projectile.NewProjectile(
							Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.ammo),
							positionAboveSky,
							(target.Center - positionAboveSky).SafeNormalize(Vector2.Zero) * 20f,
							ProjectileID.BulletHighVelocity,
							hit.Damage,
							0,
							Player.whoAmI);
						Main.projectile[projectile].usesLocalNPCImmunity = true;
					}
					Deagle_DaedalusStormBow_coolDown = 600;
				}
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			if (item.type == ModContent.ItemType<Deagle>() && Player.velocity == Vector2.Zero) {
				crit += 55;
			}
		}
	}
}
