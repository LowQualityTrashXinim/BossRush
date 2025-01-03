using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace BossRush.Contents.Projectiles;
public class TomahawkProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperAxe);
	public override void SetDefaults() {
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 999;
		Projectile.aiStyle = 2;
		Projectile.tileCollide = true;
	}
	float intialvelocity;
	public override void AI() {
		if (Projectile.timeLeft == 999) {
			intialvelocity = Projectile.velocity.Length();
		}
		Player player = Main.player[Projectile.owner];
		if (++Projectile.ai[0] >= BossRushUtils.ToSecond(3) || !player.ItemAnimationActive && (player.altFunctionUse == 2 || Main.mouseRight && Projectile.owner == Main.myPlayer)) {
			Projectile.ai[1] = 1;
		}
		if (Projectile.ai[1] == 1) {
			Projectile.timeLeft = 60;
			Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * intialvelocity;
			if (Vector2.DistanceSquared(player.Center, Projectile.Center) <= 60 * 60) {
				Projectile.Kill();
			}
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0] * Projectile.direction * 10);
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.ai[1] = 1;
		Projectile.tileCollide = false;
		return false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.damage = (int)(Projectile.damage * .95f);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
		Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		return false;
	}
}
