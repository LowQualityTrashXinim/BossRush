using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;
using BossRush.Contents.Projectiles;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Contents.Items.BuilderItem;

namespace BossRush.Common;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public int Source_ItemType = -1;
	public int OnKill_ScatterShot = -1;
	bool Source_FromDeathScatterShot = false;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is EntitySource_ItemUse parent) {
			Source_ItemType = parent.Item.type;
		}
		if (source is EntitySource_Misc parent2 && parent2.Context == "OnKill_ScatterShot") {
			Source_FromDeathScatterShot = true;
		}
	}
	public override void OnKill(Projectile projectile, int timeLeft) {
		if (Source_FromDeathScatterShot
			|| OnKill_ScatterShot <= 0
			|| projectile.hostile
			|| !projectile.friendly
			|| projectile.minion
			|| projectile.aiStyle == 4
			|| projectile.aiStyle == 19
			|| projectile.aiStyle == 39
			|| projectile.aiStyle == 46
			|| projectile.aiStyle == 75
			|| projectile.aiStyle == 99
			|| projectile.aiStyle == 101
			|| projectile.minion
			|| projectile.sentry
			|| projectile.type == ProjectileID.PhantasmArrow
			|| projectile.type == ProjectileID.IchorDart
			|| projectile.type == ProjectileID.ExplosiveBunny
			|| projectile.type == ProjectileID.FinalFractal
			|| projectile.type == ProjectileID.PortalGun
			|| projectile.type == ProjectileID.PortalGunBolt
			|| projectile.type == ProjectileID.PortalGunGate
			|| projectile.type == ProjectileID.LightsBane
			|| projectile.type == ModContent.ProjectileType<LeafProjectile>()
			|| projectile.type == ModContent.ProjectileType<DiamondGemP>()
			|| projectile.type == ModContent.ProjectileType<ArenaMakerProj>()
			|| projectile.type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| projectile.type == ModContent.ProjectileType<TowerDestructionProjectile>()) {
			return;
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
		}
	}
}
