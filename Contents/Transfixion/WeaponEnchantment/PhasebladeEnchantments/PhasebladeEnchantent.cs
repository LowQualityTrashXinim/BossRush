using BossRush.Common.Global;
using BossRush.Common.Graphics;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.WeaponEnchantment.PhasebladeEnchantments;
public abstract class PhaseBlade : ModEnchantment {
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {
				Vector2 velocity = (Main.MouseWorld - player.Center);
				Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.Center, velocity.SafeNormalize(Vector2.Zero) * 1, ModContent.ProjectileType<StarWarSwordProjectile>(), player.GetWeaponDamage(item), 2f, player.whoAmI);
				if (projectile.ModProjectile is StarWarSwordProjectile starwarProjectile) {
					starwarProjectile.ColorOfSaber = SwordSlashTrail.averageColorByID[ItemIDType] * 2;
					starwarProjectile.ItemTextureID = ItemIDType;
				}
				projectile.width = item.width;
				projectile.height = item.height;
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 120);
			}
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ArmorPenetration += 10;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!proj.minion) {
			modifiers.ArmorPenetration += 10;
		}
	}
}
public class BluePhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.BluePhaseblade;
	}
}
public class RedPhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.RedPhaseblade;
	}
}
public class YellowPhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.YellowPhaseblade;
	}
}
public class OrangePhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.OrangePhaseblade;
	}
}
public class GreenPhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.GreenPhaseblade;
	}
}
public class PurplePhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.PurplePhaseblade;
	}
}
public class WhitePhaseblade : PhaseBlade {
	public override void SetDefaults() {
		ItemIDType = ItemID.WhitePhaseblade;
	}
}
