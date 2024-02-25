using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BossRush.Common;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public int Source_ItemType = -1;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is EntitySource_ItemUse parent) {
			Source_ItemType = parent.Item.type;
		}
	}
}
