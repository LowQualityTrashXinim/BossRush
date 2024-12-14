using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using BossRush.Common.Graphics.TrailStructs;

namespace BossRush.Contents.Projectiles;
public class TitaniumShuriken : ModProjectile {

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.TitaniumStormShard);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.Shuriken);
		Projectile.aiStyle = -1;
		Projectile.tileCollide = true;

	}

	public override bool PreDraw(ref Color lightColor) {

		Asset<Texture2D> texture = TextureAssets.Projectile[ProjectileID.Shuriken];
		Main.instance.LoadProjectile(ProjectileID.Shuriken);
		default(BeamTrail).Draw(Projectile, Color.White);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, Color.Turquoise, Projectile.ai[0] * 0.33f, texture.Size() / 2f, 1.25f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.ai[0] * 0.33f, texture.Size() / 2f, 1f, SpriteEffects.None);

		return false;
	}

	public override void AI() {
		Projectile.ai[0]++;
	}
}
