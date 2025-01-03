using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria.ID;
using Terraria;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Projectiles;
public class TomahawkProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperAxe);
	public override void SetDefaults() {
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 150;
		Projectile.aiStyle = 2;
		Projectile.tileCollide = true;
	}

	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		Projectile.rotation += MathHelper.ToRadians(Projectile.timeLeft * -Projectile.direction * 30);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
		Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
