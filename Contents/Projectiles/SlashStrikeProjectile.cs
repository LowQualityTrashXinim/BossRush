using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;

public class SlashStrikeProjectile : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 34;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 999;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	public override void AI() {
		Projectile.alpha = 255;
	}
	public override Color? GetAlpha(Color lightColor) {
		return base.GetAlpha(lightColor);
	}
}
