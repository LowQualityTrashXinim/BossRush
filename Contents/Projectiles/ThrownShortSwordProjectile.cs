using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria.ID;
using Terraria;

namespace BossRush.Contents.Projectiles;
public class ThrowShortSwordProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperShortsword);
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
class ThrowShortSwordCoolDown : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<ThrownShortSwordPlayer>().OnCoolDown = true;
	}
}
class ThrownShortSwordPlayer : ModPlayer {
	public bool OnCoolDown = false;
	public override void ResetEffects() {
		OnCoolDown = false;
	}
}
