using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace BossRush.Contents.Projectiles;

/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Do not touch ai2
/// </summary>
public class SimplePiercingProjectile : ModProjectile {
	public Color ProjectileColor = Color.White;
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 60;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 40;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	public override void OnSpawn(IEntitySource source) {
		if (Projectile.ai[0] <= 0) {
			Projectile.ai[0] = 1;
		}
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] <= 0) {
			Projectile.ai[1] = 15;
		}
		Projectile.timeLeft = (int)Projectile.ai[1];
	}
	public override Color? GetAlpha(Color lightColor) {
		ProjectileColor.A = 0;
		return ProjectileColor * Projectile.ai[2];
	}
	public override void AI() {
		Projectile.ai[2] = Projectile.timeLeft / Projectile.ai[1];
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
		Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		//DrawTrail2(texture, lightColor, origin);
		return false;
	}
	public void DrawTrail2(Texture2D texture, Color color, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
			color = color * .45f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.oldRot[k], origin, (Projectile.scale - k / 100f) * .5f, SpriteEffects.None, 0);
		}
	}
}
