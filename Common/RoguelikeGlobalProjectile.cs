using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BossRush.Common;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		ProjectileSourceCheck(this, projectile, source);
	}
	public virtual void ProjectileSourceCheck(RoguelikeGlobalProjectile globalproj, Projectile projectile, IEntitySource source) {

	}
}
