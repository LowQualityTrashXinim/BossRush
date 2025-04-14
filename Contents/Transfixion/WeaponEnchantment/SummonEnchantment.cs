using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;

public class BabyBirdStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BabyBirdStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.BabyBird);
		Main.projectile[globalItem.Item_Counter2[index]].Kill();
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ownedProjectileCounts[ProjectileID.BabyBird] < 1) {
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.BabyBird, player.GetWeaponDamage(item), 0, player.whoAmI);
			Main.projectile[proj].minionSlots = 0;
			globalItem.Item_Counter2[index] = proj;
		}
		player.AddBuff(BuffID.BabyBird, 60);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!ContentSamples.ProjectilesByType[type].minion) {
			Projectile proj = Main.projectile[globalItem.Item_Counter2[index]];
			Projectile.NewProjectile(source, proj.Center, (Main.MouseWorld - proj.Center).SafeNormalize(Vector2.Zero) * velocity.Length(), type, (int)(damage * .25f), knockback * .25f, player.whoAmI);
		}
	}
}
public class BabySlimeStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SlimeStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.BabySlime);
		Main.projectile[globalItem.Item_Counter2[index]].Kill();
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ownedProjectileCounts[ProjectileID.BabySlime] < 1) {
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.BabySlime, player.GetWeaponDamage(item), 0, player.whoAmI);
			Main.projectile[proj].minionSlots = 0;
			globalItem.Item_Counter2[index] = proj;
		}
		player.AddBuff(BuffID.BabySlime, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.whoAmI == globalItem.Item_Counter2[index] && proj.type == ProjectileID.BabySlime) {
			float Amount = Main.rand.Next(1, 3);
			for (int i = 0; i < Amount; i++) {
				Vector2 vel = Main.rand.NextVector2Unit(-(MathHelper.PiOver2 + MathHelper.PiOver4 * .5f), MathHelper.PiOver4) * Main.rand.NextFloat(5, 7);
				int projec = Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, vel, ProjectileID.SpikedSlimeSpike, proj.damage, 0, player.whoAmI);
				Main.projectile[projec].friendly = true;
				Main.projectile[projec].hostile = false;
			}
		}
	}
}
public class FlinxStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlinxStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.FlinxMinion);
		Main.projectile[globalItem.Item_Counter1[index]].Kill();
		Main.projectile[globalItem.Item_Counter2[index]].Kill();
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ownedProjectileCounts[ProjectileID.FlinxMinion] < 1) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.FlinxMinion, player.GetWeaponDamage(item), 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
			int proj2 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.FlinxMinion, player.GetWeaponDamage(item), 0, player.whoAmI);
			Main.projectile[proj2].minionSlots = 0;
			globalItem.Item_Counter2[index] = proj2;
		}
		player.AddBuff(BuffID.FlinxMinion, 60);
	}
}

public class LeatherWhip : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BlandWhip;
	}
	public override void Update(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += 0.10f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.10f;
		player.GetDamage(DamageClass.Summon).Base += 1;
	}

}

public class Snapthorn : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.ThornWhip;
	}
	public override void Update(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += 0.15f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
		player.GetDamage(DamageClass.Summon).Base += 2;
	}

	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		player.AddBuff(BuffID.ThornWhipPlayerBuff, BossRushUtils.ToSecond(1.5f));
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		player.AddBuff(BuffID.ThornWhipPlayerBuff, BossRushUtils.ToSecond(1.5f));
	}


}

public class SpinalTap : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.BoneWhip;
	}
	public override void Update(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += 0.2f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.2f;
		player.GetDamage(DamageClass.Summon).Base += 3;
	}
}

public class ImpStaff : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.ImpStaff;
		ForcedCleanCounter = true;
	}

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {

		globalItem.Item_Counter1[index]++;


		foreach (Projectile minion in Main.ActiveProjectiles) {
			NPC target = minion.FindTargetWithinRange(200);
			if (minion.minion && minion.owner == player.whoAmI && globalItem.Item_Counter1[0] >= 30 && target != null) {

				Projectile.NewProjectile(player.GetSource_FromAI(), minion.position, (target.Center - minion.Center).SafeNormalize(Vector2.UnitY) * 15, ProjectileID.ImpFireball, 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = 0;

			}

		}
	}
}


public class HornetStaff : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.HornetStaff;
		ForcedCleanCounter = true;
	}

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {

		globalItem.Item_Counter1[index]++;


		foreach (Projectile minion in Main.ActiveProjectiles) {
			NPC target = minion.FindTargetWithinRange(200);
			if (minion.minion && minion.owner == player.whoAmI && globalItem.Item_Counter1[0] >= 40 && target != null) {

				Projectile.NewProjectile(player.GetSource_FromAI(), minion.position, (target.Center - minion.Center).SafeNormalize(Vector2.UnitY) * 5, ProjectileID.HornetStinger, 12, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = 0;

			}

		}
	}
}

