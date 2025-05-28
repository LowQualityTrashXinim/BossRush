using BossRush.Texture;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles;
internal class SoulWoodSword : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.timeLeft = BossRushUtils.ToMinute(.25f);
	}
	public float State { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
	public int ItemIDtextureValue = ItemID.WoodenSword;
	Vector2 directionTo = Vector2.Zero;
	float outrotation = 0;
	int directionLooking = 1;
	Vector2 oldCenter = Vector2.Zero;
	int continuousTimer = 0;
	bool ResetStats = true;
	NPC npc = null;
	public override void AI() {
		continuousTimer++;
		switch (State) {
			case 1:
				SpearAI();
				break;
			case 2:
				EnergySword_Code1AI();
				break;
			case 3:
				ResetState();
				break;
			case 4:
				MoveToTarget();
				break;
			default:
				State = 3;
				break;
		}
	}
	Vector2 AnywhereNearNPC = Vector2.Zero;
	private void MoveToTarget() {
		ResetStats = true;
		if (npc == null || !npc.active) {
			if (!Projectile.Center.IsCloseToPosition(Main.player[Projectile.owner].Center.Add(0, 50), 30)) {
				Projectile.velocity = (Main.player[Projectile.owner].Center.Add(0, 50) - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.velocity = Vector2.Zero;
				Projectile.rotation = -MathHelper.PiOver4;
			}
			AnywhereNearNPC = Vector2.Zero;
			State = 3;
			return;
		}
		if (AnywhereNearNPC == Vector2.Zero) {
			AnywhereNearNPC = npc.Center + Main.rand.NextVector2CircularEdge(npc.width, npc.height);
		}
		Projectile.velocity = (AnywhereNearNPC - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		if (Projectile.Center.IsCloseToPosition(AnywhereNearNPC, 25f)) {
			State = Main.rand.Next(1, 3);
		}
	}
	private void ResetState() {
		if (Projectile.rotation != 0) {
			if (Projectile.rotation > 0) {
				Projectile.rotation -= MathHelper.ToRadians(1);
			}
			else {
				Projectile.rotation += MathHelper.ToRadians(1);
			}
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 1500)) {
			this.npc = npc;
		}
		AnywhereNearNPC = Vector2.Zero;
		State++;
		ResetStats = true;
	}
	private void SpearAI() {
		int duration = 10;
		if (ResetStats) {
			oldCenter = Projectile.Center;
			ResetStats = false;
			continuousTimer = 0;
			directionTo = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
		}

		float halfDuration = duration * 0.5f;
		float progress = Math.Clamp(continuousTimer / halfDuration, 0, 1);
		Vector2 vel = Vector2.SmoothStep(directionTo * 20, directionTo * 220, progress);
		Projectile.Center = oldCenter + vel;
		Projectile.rotation = directionTo.ToRotation() + MathHelper.PiOver4;
		if (continuousTimer > 20) {
			ResetStats = true;
			State = 3;
		}
	}
	private void EnergySword_Code1AI() {
		if (ResetStats) {
			continuousTimer = 0;

			directionTo = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

			oldCenter = Projectile.Center.PositionOFFSET(directionTo, -10);
			directionLooking = Main.rand.NextBool().ToDirectionInt();
			ResetStats = false;
		}
		float percentDone = 1 - continuousTimer / 20f;
		percentDone = Math.Clamp(BossRushUtils.InExpo(percentDone), 0, 1);
		Projectile.spriteDirection = directionLooking;
		float baseAngle = directionTo.ToRotation();
		float angle = MathHelper.ToRadians(150) * directionLooking;
		float start = baseAngle + angle;
		float end = baseAngle - angle;
		float rotation = MathHelper.Lerp(start, end, percentDone);
		outrotation = rotation;
		Projectile.rotation = rotation + MathHelper.PiOver4;
		Projectile.Center = oldCenter + Vector2.UnitX.RotatedBy(rotation) * 60f;
		if (continuousTimer > 30) {
			ResetStats = true;
			State = 3;
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		Player player = Main.player[Projectile.owner];

		int directionTo = (player.Center.X < target.Center.X).ToDirectionInt();
		modifiers.HitDirectionOverride = directionTo;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 10; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Cloud);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(3, 3) + directionTo * 3;
			dust.scale = Main.rand.NextFloat(3f, 3.5f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Color yellowish = new Color(255, 255, 0, 100);

		Main.EntitySpriteDraw(texture, drawPos.Add(3, 3), null, yellowish, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(texture, drawPos.Add(-3, 3), null, yellowish, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(texture, drawPos.Add(3, -3), null, yellowish, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(texture, drawPos.Add(-3, -3), null, yellowish, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
