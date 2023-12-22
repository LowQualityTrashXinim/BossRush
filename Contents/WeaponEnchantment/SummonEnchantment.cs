using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.WeaponEnchantment;

public class FlinxStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlinxStaff;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.maxMinions++;
		player.flinxMinion = true;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (player.ownedProjectileCounts[ProjectileID.FlinxMinion] < 1) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.FlinxMinion, damage, knockback, player.whoAmI);
		}
	}
}
