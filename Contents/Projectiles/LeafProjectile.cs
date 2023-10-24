using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
internal class LeafProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BladeOfGrass);
	public override void SetDefaults() {
		base.SetDefaults();
	}
	public override void AI() {
		base.AI();
	}
}
