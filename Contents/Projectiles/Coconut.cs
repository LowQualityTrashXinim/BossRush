using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
internal class Coconut : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Coconut);
	public override void SetStaticDefaults() {
		base.SetStaticDefaults();
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		Projectile.velocity *= .98f;
		if (Projectile.velocity.Y >= 20)
			Projectile.velocity.Y += .75f;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if(Projectile.velocity.X != oldVelocity.X)
			Projectile.velocity.X = -oldVelocity.X;
		if (Projectile.velocity.Y != oldVelocity.Y)
			Projectile.velocity.Y = -oldVelocity.Y;
		return false;
	}
}
