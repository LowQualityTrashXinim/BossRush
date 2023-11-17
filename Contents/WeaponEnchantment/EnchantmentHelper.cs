using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.WeaponEnchantment;
public static class EnchantmentHelper {
	public static void WoodenEnchantment(Player player, Item item) {
		Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<AcornProjectile>(), item.damage, item.knockBack, player.whoAmI);
	}
}
