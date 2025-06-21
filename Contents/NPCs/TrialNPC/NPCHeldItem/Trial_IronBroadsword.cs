using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs.TrialNPC.NPCHeldItem;
internal class Trial_IronBroadsword : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.IronBroadsword);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 900;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
	}
	public int NPCwhoAmI { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public int MaxAnimation { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
	NPC npc = null;
	Vector2 directionToMouse = Vector2.Zero;
	float outrotation = 0;
	int directionLooking = 1;
	public override void AI() {
		if (NPCwhoAmI < 0 || NPCwhoAmI >= 255 || MaxAnimation <= 0) {
			Projectile.Kill();
			return;
		}
		npc = Main.npc[NPCwhoAmI];

		if (!npc.active) {
			NPCwhoAmI = -1;
			return;
		}
		if (Projectile.timeLeft > MaxAnimation) {
			directionToMouse = Projectile.velocity;
			Projectile.timeLeft = MaxAnimation;
			directionLooking = Projectile.velocity.X > 0 ? 1 : -1;
		}
		if (Projectile.timeLeft <= 10) {
			Projectile.ProjectileAlphaDecay(10);
		}
		float percentDone = Projectile.timeLeft / (float)MaxAnimation;
		percentDone = Math.Clamp(BossRushUtils.InOutExpo(percentDone, 11f), 0, 1);
		Projectile.spriteDirection = directionLooking;
		float baseAngle = directionToMouse.ToRotation();
		float angle = MathHelper.ToRadians(135) * directionLooking;
		float start = baseAngle + angle;
		float end = baseAngle - angle;
		float rotation = MathHelper.Lerp(start, end, percentDone);
		outrotation = rotation;
		Projectile.rotation = rotation + MathHelper.PiOver4;
		Projectile.velocity.X = directionLooking;
		Projectile.Center = npc.Center + Vector2.UnitX.RotatedBy(rotation) * 50f;
	}
	public override void ModifyDamageHitbox(ref Rectangle hitbox) {
		if (NPCwhoAmI < 0 || NPCwhoAmI >= 255) {
			Projectile.Kill();
			return;
		}
		NPC npc = Main.npc[NPCwhoAmI];
		if (!npc.active) {
			NPCwhoAmI = -1;
			return;
		}
		BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, npc.Center, outrotation, Projectile.width, Projectile.height, 10);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemID.IronBroadsword)).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
