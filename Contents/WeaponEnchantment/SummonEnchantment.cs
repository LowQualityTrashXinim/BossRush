using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.WeaponEnchantment;

public class BabyBirdStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BabyBirdStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		Main.projectile[globalItem.Item_Counter2[index]].Kill();
		if (player.ownedProjectileCounts[ProjectileID.BabyBird] < 1) {
			player.ClearBuff(BuffID.BabyBird);
		}
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
