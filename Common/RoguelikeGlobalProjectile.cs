using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
			|| projectile.aiStyle == 99
			|| projectile.aiStyle == 19
			|| projectile.minion
			|| projectile.type == ModContent.ProjectileType<DiamondGemP>()) {
			return;
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
		}
	}
}
