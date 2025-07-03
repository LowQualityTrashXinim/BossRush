using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Texture;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MythrilBeamSword;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
public static class EnchantmentMisc {

	public static void SpawnPetal(Player player, NPC victim, int damage) {
		Vector2 pos = victim.Center + (Main.rand.NextBool(2) == true ? new Vector2(500, Main.rand.Next(-100, 101)) : new Vector2(-500, Main.rand.Next(-100, 101)));
		Projectile.NewProjectile(player.GetSource_OnHit(victim), pos, pos.DirectionTo(victim.Center) * 15, ProjectileID.FlowerPetal, damage, 0, player.whoAmI);

	}

	public static void SpawnHearts(Player player, NPC victim) {

		if (Main.myPlayer == player.whoAmI)
			Item.NewItem(player.GetSource_OnHit(victim), victim.Center, 0, 0, ItemID.Heart, 1);

	}

	public static void SpawnShuriken(Player player, int damage) {

		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, player.DirectionTo(Main.MouseWorld) * 15, ModContent.ProjectileType<TitaniumShuriken>(), damage, 0, player.whoAmI);

	}
}

public abstract class PalladiumEnchantment : ModEnchantment {

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {

		if (globalItem.Item_Counter1[index] > 0)
			return;



		if (Main.rand.NextBool(4)) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(15);
			EnchantmentMisc.SpawnHearts(player, target);

		}
	}

	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0)
			return;


		if (Main.rand.NextBool(4)) {
			globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(15);
			EnchantmentMisc.SpawnHearts(player, target);

		}
	}
}

public abstract class OrichalcumEnchantment : ModEnchantment {

	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		EnchantmentMisc.SpawnPetal(player, target, 44);
	}

	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.FlowerPetal)
			return;

		EnchantmentMisc.SpawnPetal(player, target, 22);
	}
}

public abstract class TitaniumEnchantment : ModEnchantment {

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
	}

	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] > 0)
			return;

		globalItem.Item_Counter1[index] = BossRushUtils.ToSecond(2);


		EnchantmentMisc.SpawnShuriken(player, damage * 3);
	}


}
/*
 TODO; Add these missing enchantment
- flymeal
- breathing reed ( wtf )
- exotic scimitar
- purple clubberfish
- fruitcake chakram
- bloody machete
- thorn chakram
- combat wrench
- mace
- flaming mace
- blue moon
- sunfury
- helwing bow
- pew-matic horn
- harpoon
- starcannon
- ale tosser
- blowgun ( not to be confused with blowpipe )
- flinx staff
- abigail flower
- vampire flog staff
- hondius shootius
- lighting aura rod
- flameburst rod
- explosive trap rod
- ballista rod
- flare gun ( rework honestly )
 */
