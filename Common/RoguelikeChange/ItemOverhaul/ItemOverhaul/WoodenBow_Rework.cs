using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection;
using BossRush.Contents.Perks.WeaponUpgrade;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
public class Roguelike_WoodenBow : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.WoodenBow) {
			entity.shootSpeed += 3;
			entity.crit += 6;
			entity.damage += 5;
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type == ItemID.WoodenBow) {
			if (Main.rand.NextFloat() <= .3f) {
				Vector2 pos = Main.MouseWorld + Main.rand.NextVector2CircularEdge(2000, 700);
				Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 5;
				Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<WindShot>(), (int)(damage * .55f), 5f, player.whoAmI);
			}
			Vector2 newPos1 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(MathHelper.PiOver2), 5);
			Vector2 newVelocity1 = (Main.MouseWorld - newPos1).SafeNormalize(Vector2.Zero) * velocity.Length();
			Vector2 newPos2 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(-MathHelper.PiOver2), 5);
			Vector2 newVelocity2 = (Main.MouseWorld - newPos2).SafeNormalize(Vector2.Zero) * velocity.Length();
			Projectile arrow1 = Projectile.NewProjectileDirect(source, newPos1, newVelocity1, type, damage, knockback, player.whoAmI);
			Projectile arrow2 = Projectile.NewProjectileDirect(source, newPos2, newVelocity2, type, damage, knockback, player.whoAmI);
			if (ContentSamples.ProjectilesByType[type].arrow) {
				arrow1.extraUpdates += 1;
				arrow2.extraUpdates += 1;
			}
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.WoodenBow) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_WoodenBow", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
