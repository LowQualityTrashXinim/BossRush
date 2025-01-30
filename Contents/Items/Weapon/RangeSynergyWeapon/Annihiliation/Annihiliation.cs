using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
internal class Annihiliation : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 78, 3f, 2, 6, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, true, AmmoID.Bullet);

	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
}
public class AnnihiliationBullet : SynergyModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = BossRushUtils.ToSecond(30);
		Projectile.extraUpdates = 10;
	}
}
