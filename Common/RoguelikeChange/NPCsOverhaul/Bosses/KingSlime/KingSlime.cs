using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria.DataStructures;
using Terraria.Audio;
using BossRush.Common.Graphics.Structs.QuadStructs;
using BossRush.Common.Graphics;
using Terraria.WorldBuilding;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace BossRush.Common.RoguelikeChange.NPCsOverhaul.Bosses.KingSlime;
internal class KingSlime : NPCReworker {
	public override int VanillaNPCType => NPCID.KingSlime;

	public override void SetDefaults(NPC entity) {
		entity.aiStyle = -1;
	}

	Vector2 currentScale = Vector2.One;
	Vector2 bouncyScale = new Vector2(2f, 0.8f);
	bool firedRuby = false;
	Vector2 crownPos = Vector2.One;
	float distanceToTarget = 0;
	bool lockedIn = false;
	Vector2 targetDirectionPredicate = Vector2.Zero;
	NPC crownNPC = null;
	State state {

		get { return (State)AIStateSync; }
		set {
			AIStateSync = (int)value;


			switch ((State)AIStateSync) {


				case State.Idle: {
						Timer = 0;
						Delay = 0;
						firedRuby = false;
						Counter3++;

						if (Main.rand.NextBool(7) && crownNPC == null) {

							switch (Main.rand.Next(1, 3)) {

								case 1: {

										AIStateSync = (int)State.LaserRotate;

										break;
									}

								case 2: {

										AIStateSync = (int)State.RubySpray;

										break;
									}


							}

						}
						else if (Main.rand.NextBool(7)) {

							AIStateSync = (int)State.SlamJump;

						}
						NeedsNetUpdate = true;
						break;
					}


			}


		}

	}

	enum State {

		Idle,
		SlamJump,
		Slamming,
		Jump,
		Jumping,
		RubySpray,
		Spike,
		LaserRotate,
		bouncing
	}
	public override void OnKill(NPC npc) {
		if (crownNPC != null)
			crownNPC.active = false;

		for (int i = 0; i < Main.maxTilesX; i++) {
			for (int j = 0; j < Main.maxTilesY; j++) {

				if (WorldGen.TileType(i, j) == ModContent.TileType<KingSlimeSludgeTile>())
					WorldGen.KillTile(i, j);


			}

		}

		foreach (var proj in Main.ActiveProjectiles) {

			if (proj.type == ModContent.ProjectileType<KingSlimeSludgeProjectile>())
				proj.Kill();

		}
	}

	public override void ReworkedAI(ref NPC npc, Player target) {
		npc.scale = 0.75f.ClampedLerp(1.25f, npc.life / (float)npc.lifeMax);
		currentScale = Vector2.Lerp(Vector2.One, bouncyScale, BossRushUtils.EaseOutBounce(Delay / 40f)) + npc.velocity * 0.025f;
		int jumpsBeforeSlamReady = 5;
		if (npc.collideY)
			npc.velocity.X = 0;

		if (AntiCheeseSlashStuckCheck(npc))
			state = State.SlamJump;


		if (Delay == 0)
			switch (state) {

				case State.Idle: {
						afterimages = true;
						if (crownNPC == null && npc.life <= npc.lifeMax * 0.4f) {

							crownNPC = NPC.NewNPCDirect(npc.GetSource_FromAI(), crownPos, ModContent.NPCType<KingSlimeCrown>(), 0, ai2: npc.whoAmI);

						}

						npc.velocity.X = 0;

						if (Timer >= 40) {

							state = State.Jump;

						}

						if (npc.velocity.Y != 0 && !npc.collideY) {
							state = State.Jumping;

						}


						break;
					}


				case State.Jump: {

						currentScale = Vector2.One.ClampedLerp(new Vector2(1.5f, 0.7f), Counter / 15f);

						Counter++;

						if (Counter < 15)
							return;

						Counter = 0;
						// i love calculas! (said by someone who got below F on calculas class)

						float t = 3;
						var distanceToTarget = new Vector2(MathF.Abs(npc.Center.X - npc.targetRect.Center.X), MathF.Abs(npc.Center.Y - npc.targetRect.Top));
						float angle = MathHelper.ToRadians(Main.rand.NextFloat(60, 80));
						float maxJumpHeight = 12 + distanceToTarget.Y / 32f * (npc.gravity * t);
						//float trajectoryFormula1 = MathF.Sqrt(MathF.Pow(maxJumpHeight, 4f) - npc.gravity * (npc.gravity * MathF.Pow(distanceToTarget.X, 2f) + 2f * distanceToTarget.Y * MathF.Pow(maxJumpHeight, 2f)));
						//float angle = (MathF.Atan((MathF.Pow(maxJumpHeight, 2f) - trajectoryFormula1) / (npc.gravity * distanceToTarget.X)));
						float velocityX = 10 * MathF.Cos(float.IsNaN(angle) ? 45 : angle) * npc.direction;
						float velocityY = maxJumpHeight * MathF.Sin(float.IsNaN(angle) ? 45 : angle) - npc.gravity * t;
						this.distanceToTarget = npc.Distance(npc.targetRect.Center());
						Delay2 = 25;
						npc.velocity = new Vector2(velocityX, -velocityY);
						state = State.Jumping;
						break;
					}

				case State.Jumping: {


						currentScale.X = 1f + npc.velocity.Y * 0.025f;
						currentScale.Y = 1f - npc.velocity.Y * 0.025f;
						afterimages = true;
						if (npc.oldVelocity.Y <= 1 && npc.velocity.Y > 0 && !firedRuby && crownNPC == null && Delay2 == 0) {
							firedRuby = true;
							NewProjectileWithMPCheck(npc.GetSource_FromAI(), crownPos, crownPos.DirectionTo(npc.targetRect.Center()) * 5, ModContent.ProjectileType<SlimeKingRubyBolt>(), npc.damage / 5, 0, -1, npc.whoAmI);

						}

						if (npc.velocity.X == 0)
							npc.velocity.X = npc.direction * 6;

						if (Counter3 >= jumpsBeforeSlamReady && npc.Center.X > npc.targetRect.Center.X - 16 && npc.Center.X < npc.targetRect.Center.X + 16 && npc.Center.Y < npc.targetRect.Center.Y && npc.velocity.Y < 6 && npc.Distance(npc.targetRect.Center()) > 125) {
							lockedIn = true;
						}

						if (lockedIn) {

							npc.Center -= npc.velocity;
							npc.Center += new Vector2(0, MathF.Sin(Counter2 * 0.125f + 90) * 14f.ClampedLerp(7f, Counter2 / 45f));
							Counter2++;

							if (Counter2 >= 45) {
								npc.velocity = new Vector2(0, 25);
								npc.MaxFallSpeedMultiplier *= 5;
								state = State.Slamming;
								Counter2 = 0;
								Counter3 = 0;
								afterimages = true;
								lockedIn = false;
								currentScale = Vector2.One.ClampedLerp(new Vector2(2, 0.5f), Counter2 / 45f);
							}
							break;
						}

						if (npc.collideY && !justMovedAwayFromTheGround) {
							state = State.Idle;
							Delay = 40;
							if (Delay2 > 0)
								npc.ai[0] = MathHelper.Clamp(npc.ai[0] += 1, 0, 5);
							else {
								npc.ai[0] = MathHelper.Clamp(npc.ai[0] -= 1, 0, 5);
							}
							SlamSound(npc);
							Counter = 0;
							Counter2 = 0;
						}
						distanceToTarget = npc.Distance(npc.targetRect.Center());


						break;



					}
				case State.Slamming: {


						currentScale.X = 1f - npc.velocity.Y * 0.025f;
						currentScale.Y = 1f + npc.velocity.Y * 0.025f;
						afterimages = true;
						npc.MaxFallSpeedMultiplier *= 5;


						if (npc.collideY && !justMovedAwayFromTheGround) {
							state = State.Idle;
							Delay = 40;
							SlamSound(npc);


						}
						distanceToTarget = npc.Distance(npc.targetRect.Center());

						if (Counter >= 5 && npc.Center.X > npc.targetRect.Center.X - 16 && npc.Center.X < npc.targetRect.Center.X + 16)
							state = State.Slamming;

						break;



					}

				case State.LaserRotate: {

						Counter++;
						npc.velocity.X = 0;
						if (Counter >= 110 * 5) {
							state = State.Jump;
							Counter = 0;
							break;
						}

						if (Counter % 110 == 0) {
							NewProjectileWithMPCheck(npc.GetSource_FromAI(), npc.Center - new Vector2(0, npc.Center.Y - crownPos.Y), Vector2.Zero, ModContent.ProjectileType<SlimeKingRubyBolt>(), npc.damage / 5, 0, -1, npc.whoAmI, 2, npc.Center.Y - crownPos.Y);



						}


						if(Counter % 220 == 0)
						{

							if(Main.netMode != NetmodeID.MultiplayerClient)
							{
								NPC.NewNPCDirect(npc.GetSource_FromAI(), npc.Center, ModContent.NPCType<KingSlimeMinion>(), 0, 0).velocity = new Vector2(7, -7);
								NPC.NewNPCDirect(npc.GetSource_FromAI(), npc.Center, ModContent.NPCType<KingSlimeMinion>(), 0, 0).velocity = new Vector2(-7, -7);

							}
						
						}

						currentScale = new Vector2(0.8f, 1).ClampedLerp(new Vector2(1.15f, 1), MathF.Cos(Counter * 0.1f) + 0.5f);



						break;
					}

				case State.RubySpray: {

						Counter++;

						npc.velocity.X = 0;


						if (Counter2 >= 8) {
							Counter2++;

							if (Counter2 >= 100) {
								Counter = 0;
								Counter2 = 0;
								state = State.Idle;
								break;
							}


						}
						else
							currentScale = Vector2.One.ClampedLerp(new Vector2(2f, 0.5f), Terraria.Utils.PingPongFrom01To010(BossRushUtils.OutSine(Counter / 40f)));


						if (Counter % 5 == 0 && Counter2 <= 9) {

							for (int i = 0; i < 2; i++) {

								NewProjectileWithMPCheck(npc.GetSource_FromAI(), crownPos, (npc.Center - new Vector2(0, 64)).DirectionTo(npc.targetRect.Center()).RotatedBy(Counter2 / 8f * -MathHelper.Pi + MathHelper.PiOver2) * 6, ModContent.ProjectileType<SlimeKingRubyBolt>(), npc.damage / 5, 0, -1, npc.whoAmI, 0);


							}
							Counter2++;


						}
						break;
					}
				case State.SlamJump: {
						currentScale.X = 1f.ClampedLerp(2f, Counter / 100f);
						currentScale.Y = 1f.ClampedLerp(0.5f, BossRushUtils.EaseOutBounce(Counter / 100f));
						Counter++;
						var tilePos = npc.Center.ToTileCoordinates();

						if (Counter == 100)
							targetDirectionPredicate = target.velocity.X >= target.maxRunSpeed ? target.velocity.SafeNormalize(Vector2.Zero) : Vector2.Zero;

						if (Counter >= 100 && Counter < 160) {
							npc.noTileCollide = true;
							npc.noGravity = true;
							npc.position.Y = npc.position.Y.ClampedLerp(npc.targetRect.Top - 1000, (Counter - 100f) / 60f);

							currentScale = new Vector2(0.5f, 2f);


							if (Counter < 160 && Counter > 140)
								npc.position.X = npc.position.X.ClampedLerp(npc.targetRect.Center().X + targetDirectionPredicate.X * 250 - npc.Hitbox.Width / 2f, 0.2f);
						}
						if (Counter >= 180) {

							npc.velocity.Y = 25;

						}

						if (npc.Center.Y + npc.height >= target.Bottom.Y && Counter >= 180 + 1000 / 40f - npc.Hitbox.Height / 40 && !WorldGen.TileEmpty(tilePos.X, tilePos.Y + 8)) {
							npc.noTileCollide = false;
							npc.noGravity = false;
							state = State.Idle;
							Counter = 0;
							Delay = 40;
							for (int i = 0; i < 12; i++) {
								var vel = Vector2.UnitX.Vector2RotateByRandom(30) * Main.rand.NextFloat(5, 11);

								NewProjectileWithMPCheck(npc.GetSource_FromAI(), npc.Center, vel.RotatedBy(-MathHelper.PiOver2), ModContent.ProjectileType<KingSlimeSludgeProjectile>(), 0, 0);


							}
							npc.ai[0] = 0;
							SlamSound(npc);
						}


						break;
					}

			}
		crownPos = npc.Center - new Vector2(-npc.velocity.X, MathHelper.Lerp(0, 40, npc.scale - (1 - npc.scale)) * currentScale.Y);

	}
	// also dust cuz it makes sense to be here also
	public void SlamSound(NPC npc) {

		SoundEngine.PlaySound(SoundID.Item56 with { PitchVariance = 1f, Volume = 3f });
		SoundEngine.PlaySound(SoundID.Item89 with { PitchVariance = 1f });
		for (int i = 0; i < npc.width; i++)
			Dust.NewDustPerfect(npc.position + new Vector2(i, npc.height), DustID.BubbleBurst_Blue, newColor: Color.White).noGravity = true;

	}

	public override bool? CanFallThroughPlatforms(NPC npc) {
		return state != State.LaserRotate ? base.CanFallThroughPlatforms(npc) : false;
	}

	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		//DrawAfterimages(npc, spriteBatch, screenPos, drawColor);

		Main.EntitySpriteDraw(TextureAssets.Npc[npc.type].Value, npc.Center - screenPos + new Vector2(0, npc.frame.Height / 2 * (1f - currentScale.Y + (1f - npc.scale)) + 8 * npc.scale), npc.frame, drawColor * 0.9f, npc.rotation, npc.frame.Size() / 2f, npc.scale * currentScale, SpriteEffects.None);

		if (crownNPC == null)
			Main.EntitySpriteDraw(TextureAssets.Extra[39].Value, crownPos - screenPos - new Vector2(0, 8), null, drawColor, npc.rotation, TextureAssets.Extra[39].Size() / 2f, 1f, SpriteEffects.None);

		if (state == State.SlamJump && Counter >= 140 && Counter < 180) {

			var shaderSettings = new ShaderSettings();
			shaderSettings.image1 = TextureAssets.Extra[193];
			shaderSettings.image2 = null;
			shaderSettings.image3 = null;
			shaderSettings.Color = Color.Cyan;
			shaderSettings.shaderData = new Vector4(1f, 1000, 0, 0);
			float progress = 1f - (Counter - 140) / 60f;
			default(ColorIndicatorQuad).Draw(npc.Center + new Vector2(64, 0), MathHelper.PiOver2, new Vector2(2000 * progress, 256), shaderSettings);
			default(ColorIndicatorQuad).Draw(npc.Center - new Vector2(64, 0), MathHelper.PiOver2, new Vector2(2000 * progress, 256), shaderSettings);

		}




		return false;
	}

	public bool AntiCheeseSlashStuckCheck(NPC npc) {
		return npc.ai[0] == 5 && state == State.Idle;
	}

	public override void OnHitNPC(NPC npc, NPC target, NPC.HitInfo hit) {

	}
	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {



	}
}
/// <summary>
/// please dont reuse the laser attack from this, its a mess
/// </summary>
public class SlimeKingRubyBolt : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override string Texture => BossRushTexture.MissingTexture_Default;
	NPC kingSlime;
	float progress = 0;
	float lasersRotation = 0;
	float targetLaserRotation = 0;
	float lastLaserRotation = 0;
	int laserRotationDir = 1;
	int numOfLasers {
		get => Main.masterMode ? 7 : Main.expertMode ? 6 : 5;
	}
	public Vector2[] oldPos = new Vector2[30];
	Vector2 startingPos;
	Vector2 startingVel;

	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
	}
	public override void OnSpawn(IEntitySource source) {
		kingSlime = Main.npc[(int)Projectile.ai[0]];
		SoundEngine.PlaySound(SoundID.Item68 with { Pitch = 1f });

		if (Projectile.ai[1] == 2) {

			Projectile.timeLeft = 60;
			lasersRotation = Main.rand.NextFloatDirection();
			laserRotationDir = Main.rand.NextBool() ? 1 : -1;
		}
		if (Projectile.ai[1] == 1)
			Projectile.timeLeft = 200;
		Projectile.rotation = Projectile.velocity.ToRotation();
		// IM FIGHTING WITH A NAN VECTOR2, DONT ASK QUESTIONS
		startingPos = Projectile.position + Projectile.Size / 2f - new Vector2(0, -1);
		startingVel = Projectile.velocity;
	}
	public override void AI() {



		BossRushUtils.Push(ref oldPos, Projectile.Center);

		switch (Projectile.ai[1]) {



			case 0: {

					Projectile.velocity *= 1.015f;

					break;
				}

			case 1: {

					Projectile.velocity = Vector2.Lerp(Projectile.Center.DirectionTo(startingPos) * startingVel.Length(), startingVel, Projectile.timeLeft / 100f);
					break;
				}

			case 2: {
					if (Projectile.timeLeft <= 30) {
						progress = 1f.ClampedLerp(0f, BossRushUtils.InOutExpo(Projectile.timeLeft / 30f, 3f));

					}

					lasersRotation += laserRotationDir * 0.01f * ((Projectile.timeLeft - 12) / 100f);


					if (Projectile.timeLeft == 12) {

						SoundEngine.PlaySound(SoundID.Item68, Projectile.Center);
						laserRotationDir *= 0;
					}
					Projectile.velocity = Vector2.Zero;
					Projectile.Center = kingSlime.Center - new Vector2(0, Projectile.ai[2]);


					break;
				}

		}
	}
	public override bool PreDraw(ref Color lightColor) {

		BossRushUtils.DrawPrettyStarSparkle(1f, SpriteEffects.None, startingPos - Main.screenPosition, Color.Red, Color.Red, (Projectile.timeLeft - 280f) / 20f, 0.2f, 1, 0.5f, 0f, Projectile.rotation, Vector2.One * 5f, Vector2.One);

		if (Projectile.ai[1] != 2) {
			var settings = new ShaderSettings();
			settings.Color = Color.IndianRed;
			settings.shaderData = new Vector4(0, 0, 0, 0);
			settings.image1 = TextureAssets.Extra[193];
			default(RubyBoltQuad).Draw(Projectile.Center, Projectile.rotation, new Vector2(64, 32), settings);

			return false;
		}

		for (float i = MathHelper.TwoPi; i > 0; i -= MathHelper.TwoPi / numOfLasers) {
			int length = 4000;
			var shaderSettings = new ShaderSettings();
			shaderSettings.image1 = TextureAssets.Extra[193];
			shaderSettings.image2 = null;
			shaderSettings.image3 = null;
			shaderSettings.Color = Color.IndianRed;
			shaderSettings.shaderData = new Vector4(progress, length, 0, 0);
			var shaderSettings2 = new ShaderSettings();
			shaderSettings2.image1 = TextureAssets.Extra[193];
			shaderSettings2.image2 = null;
			shaderSettings2.image3 = null;
			shaderSettings2.Color = Color.Lerp(Color.Red, Color.CornflowerBlue, Projectile.timeLeft / 140f);
			shaderSettings2.shaderData = new Vector4(1f - Projectile.timeLeft / 120f, length, 0, 0);
			if (progress < 0.25)
				default(ColorIndicatorQuad).Draw(Projectile.Center, i + lasersRotation, new Vector2(length, 256), shaderSettings2);
			else
				default(RubyLaserQuad).Draw(Projectile.Center, i + lasersRotation, new Vector2(length, 256 * Terraria.Utils.PingPongFrom01To010(progress)), shaderSettings);

		}

		return false;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {


		if (Projectile.ai[1] == 2 && progress > 0.25)
			for (float i = MathHelper.TwoPi; i > 0; i -= MathHelper.TwoPi / numOfLasers) {

				if (BossRushUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center + (i + lasersRotation).ToRotationVector2() * 4000))
					return true;

			}

		return projHitbox.Intersects(targetHitbox);
	}


}

public class KingSlimeSludgeTile : ModTile {
	public override void SetStaticDefaults() {
		Main.tileBlockLight[Type] = true;
		DustType = DustID.BubbleBurst_Blue;
		Main.tileCut[Type] = true;
		AddMapEntry(new Color(0, 0, 255));
		for (int i = 0; i < TileID.Count; i++)
			Main.tileMerge[Type][i] = true;

	}

	public override void PlaceInWorld(int i, int j, Item item) {

	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {

		return true;
	}
	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
		var t = Main.tile[i, j];

		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}
	public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight) {

	}

	public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight) {


	}

	public override void RandomUpdate(int i, int j) {

		if(Main.netMode != NetmodeID.MultiplayerClient)
			NPC.NewNPCDirect(null, new Point(i, j).ToWorldCoordinates(), ModContent.NPCType<KingSlimeMinionSpawner>());


	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
		return true;
	}

}

public class KingSlimeSludgeProjectile : ModProjectile {

	public override void SetDefaults() {
		Projectile.height = Projectile.width = 8;
		Projectile.hostile = false;
		Projectile.friendly = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 600;

	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.scale = Main.rand.NextFloat(1, 2.1f);
	}

	public override void AI() {
		Projectile.velocity.Y += 0.1f;
		Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y, -999, 20);
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		Dust.NewDustPerfect(Projectile.position, DustID.t_Slime, newColor: Color.CornflowerBlue);
		var tilePos = Projectile.position.ToTileCoordinates();

		if (!WorldGen.TileEmpty(tilePos.X, tilePos.Y) && TileID.Sets.Platforms[WorldGen.TileType(tilePos.X, tilePos.Y)] && Main.rand.NextBool(15)) {
			Projectile.Kill();
			Projectile.netUpdate = true;

		}
	}

	public override bool? CanDamage() {
		return false;
	}
	public override bool? CanCutTiles() {

		return false;
	}
	public override void OnKill(int timeLeft) {

		var tilePos = Projectile.oldPosition.ToTileCoordinates();

		for (int i = 0; i < 32; i++)
			Dust.NewDustPerfect(Projectile.position, DustID.t_Slime, Main.rand.NextVector2CircularEdge(5, 5), 0, Color.CornflowerBlue);

		var circle = new Shapes.Mound(2, Main.rand.Next(0, 4));

		WorldUtils.Gen(tilePos, circle, Actions.Chain(new GenAction[]
		{
			new PlaceTileWithCheck((ushort)ModContent.TileType<KingSlimeSludgeTile>())

		}));
	}
}

public class KingSlimePlayer : ModPlayer {

	public override void PreUpdateMovement() {

		if (WorldGen.TileType(Player.Center.ToTileCoordinates().X, (Player.Hitbox.Bottom >> 4) - 1) == ModContent.TileType<KingSlimeSludgeTile>()) {


			Player.velocity *= 0.75f;


		}

	}

	public override void PostUpdate() {
		var oldPos = (Player.oldPosition - Player.Hitbox.Size() / 2f + new Vector2(0, Player.Hitbox.Height)).ToTileCoordinates();
		if (WorldGen.TileType(oldPos.X, oldPos.Y) == ModContent.TileType<KingSlimeSludgeTile>() && WorldGen.TileEmpty(Player.Center.ToTileCoordinates().X, Player.Center.ToTileCoordinates().Y)) {
			WorldGen.KillTile(oldPos.X, oldPos.Y);

		}
	}


}

public class KingSlimeMinionSpawner : ModNPC {

	public override string Texture => BossRushTexture.MissingTexture_Default;

	ref float Duration => ref NPC.ai[0];

	public override void SetDefaults() {
		NPC.CloneDefaults(NPCID.BlueSlime);
		NPC.height = NPC.width = 8;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.noGravity = true;
		Duration = 160;
		NPC.noTileCollide = true;
		NPC.dontTakeDamage = true;
		NPC.ShowNameOnHover = false;

	}



	public override void OnSpawn(IEntitySource source) {
		Duration = 160;
		NPC.FindClosestPlayer();

	}
	public override void AI() {
		Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2CircularEdge(MathHelper.Lerp(64, 16, 1f - Duration / 160f), MathHelper.Lerp(64, 16, 1f - Duration / 160f)), DustID.GemSapphire, Vector2.Zero, 0, Color.Cyan, 2f).noGravity = true;

		if (--Duration < 0) {
			var npc = NPC.NewNPCDirect(null, NPC.Center, ModContent.NPCType<KingSlimeMinion>());
			npc.ai[3] = 1;
			npc.velocity = Main.player[npc.FindClosestPlayer()].DirectionFrom(npc.Center) * 15;
			npc.color = new Color(0, 20, 255, 100);
			NPC.active = false;
		}
	}



	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		//if (Duration == 160 || !NPC.active || Main.LocalPlayer.Center.Distance(NPC.Center) > 1000f)
		//	return false;

		//var player = Main.player[NPC.FindClosestPlayer()];
		//float length = NPC.Distance(player.Center);
		//ShaderSettings shaderSettings = new ShaderSettings();
		//shaderSettings.image1 = TextureAssets.Extra[193];
		//shaderSettings.image2 = null;
		//shaderSettings.image3 = null;
		//shaderSettings.Color = Color.Cyan;
		//shaderSettings.shaderData = new Vector4(1f, length, 0, 0);
		//default(ColorIndicatorQuad).Draw(NPC.Center, player.DirectionFrom(NPC.Center).ToRotation(), new Vector2(length, 128), shaderSettings);
		return false;
	}


}

public class KingSlimeCrown : ModNPC {

	enum AIState : byte {

		Idle,
		laserRotate,
		Charging,
		Spawning


	}

	AIState state = AIState.Idle;
	NPC magnetSlime = null;
	public override string Texture => "Terraria/Images/Extra_39";

	public override void SetDefaults() {

		NPC.height = NPC.width = 32;
		NPC.aiStyle = -1;
		NPC.friendly = false;
		NPC.damage = 40;
		NPC.noTileCollide = true;
		NPC.noGravity = true;
		NPC.dontTakeDamage = true;
		NPC.life = 69;
		NPC.lifeMax = 1;

	}

	ref float Counter => ref NPC.ai[0];
	ref float Delay => ref NPC.ai[1];
	ref float Counter2 => ref NPC.ai[2];
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		if (NPC.HasValidTarget && (state == AIState.Charging && Counter <= 15 || state == AIState.Idle && Counter < 260 && Counter > 245f && magnetSlime != null)) {
			var player = Main.player[NPC.target];
			float length = NPC.Distance(player.Center);
			var shaderSettings = new ShaderSettings();
			shaderSettings.image1 = TextureAssets.Extra[193];
			shaderSettings.image2 = null;
			shaderSettings.image3 = null;
			shaderSettings.Color = state == AIState.Charging ? Color.Gold : Color.Cyan;
			shaderSettings.shaderData = new Vector4(1f, length, 0, 0);
			default(ColorIndicatorQuad).Draw(NPC.Center, player.DirectionFrom(NPC.Center).ToRotation(), new Vector2(length, 256), shaderSettings);



		}
		BossRushUtils.DrawPrettyStarSparkle(1f, SpriteEffects.None, NPC.Center - screenPos, Color.IndianRed, Color.Red, MathF.Sin((float)Main.timeForVisualEffects * 0.1f), 0, 1, 1, 2, MathHelper.PiOver4 + MathF.Sin((float)Main.timeForVisualEffects * 0.1f), Vector2.One * 5, Vector2.One);
	}

	public override void OnSpawn(IEntitySource source) {
		state = AIState.Spawning;
		NPC.velocity = -Vector2.UnitY * 15;

	}

	public override void AI() {

		NPC.TargetClosest();

		if (!NPC.HasValidTarget)
			NPC.active = false;

		if (state != AIState.Idle) {

			Counter2 = NPC.DirectionFrom(NPC.targetRect.Center()).ToRotation();


		}

		if (Delay > 0)
			Delay--;
		if (Delay == 0)
			switch (state) {

				case AIState.Idle: {
						Counter++;
						NPC.velocity.X *= 0.9f;
						NPC.velocity.Y = MathF.Sin(Counter * 0.025f) * 0.2f;
						Counter2 += MathHelper.TwoPi / 120f;
						//slime magnet
						if (Counter == 60) {
							foreach (var npc in Main.ActiveNPCs) {

								if (npc.type == ModContent.NPCType<KingSlimeMinion>()) {
									magnetSlime = npc;
									magnetSlime.ai[3] = 1;
									break;
								}

							}
						}

						if (magnetSlime != null) {
							magnetSlime.velocity = Vector2.Zero;
							magnetSlime.noGravity = true;
							magnetSlime.noTileCollide = true;
							magnetSlime.Center = magnetSlime.Center.ClampedLerp(NPC.Center + new Vector2(0, -32), 0f.ClampedLerp(1f, (Counter - 60f) / 150f));

							for (int i = 0; i < 15; i++) {

								Dust.NewDustPerfect(magnetSlime.Center.ClampedLerp(NPC.Center, i / 15f), DustID.GemSapphire, Scale: 0.75f).noGravity = true;
								Dust.NewDustPerfect(magnetSlime.Center + Main.rand.NextVector2CircularEdge(30, 30), DustID.GemSapphire, Scale: 0.75f).noGravity = true;

							}


						}

						if (Counter >= 260 && magnetSlime != null) {

							magnetSlime.velocity = magnetSlime.DirectionTo(NPC.targetRect.Center()) * 15;
							magnetSlime.noGravity = false;
							magnetSlime.noTileCollide = false;
							magnetSlime.ai[3] = 1;
							magnetSlime = null;
						}

						if (Counter > 100f)
							NPC.Center = NPC.Center.ClampedLerp(NPC.targetRect.Center() + new Vector2(500, 0).RotatedBy(Counter2), 0.1f.ClampedLerp(0f, (Counter - 150) / 150f));

						if (Counter >= 300) {
							Counter = 0;
							state = AIState.Charging;
						}
						break;
					}
				case AIState.laserRotate: {
						NPC.velocity.Y = MathF.Sin(Counter * 0.025f) * 0.2f;

						Counter++;
						NPC.velocity.X = 0;

						if (Counter % 110 == 0) {
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 80), Vector2.Zero, ModContent.ProjectileType<SlimeKingRubyBolt>(), NPC.damage / 5, 0, -1, NPC.whoAmI, 2);
						}
						else if (Counter >= 110 * 5) {
							state = AIState.Idle;

							Counter = 0;
						}
						break;
					}
				case AIState.Spawning: {
						NPC.velocity *= 0.93f;
						if (NPC.velocity.Y > -1f)
							state = AIState.Idle;
						break;
					}
				case AIState.Charging: {
						NPC.velocity *= 0.97f;
						if (Counter == 15) {
							NPC.velocity = NPC.DirectionTo(NPC.targetRect.Center()) * 20;
						}
						if (Counter >= 100) {

							Counter = 0;
							switch (Main.rand.Next(0, 2)) {

								case 0:

									state = AIState.laserRotate;

									break;

								case 1:

									state = AIState.Idle;

									break;

							}
							NPC.netUpdate = true;

						}
						Counter++;
						break;
					}
			}
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
		return state == AIState.Charging;
	}

}

public class KingSlimeMinion : ModNPC {

	public override void SetStaticDefaults() {
		Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<NPC>(NPCID.BlueSlime);
	public override void SetDefaults() {
		NPC.CloneDefaults(NPCID.BlueSlime);
		AnimationType = NPCID.BlueSlime;
		AIType = NPCID.BlueSlime;
		NPC.lifeMax = 50;
	}
	public override Color? GetAlpha(Color drawColor) {
		var blend = Color.CornflowerBlue;
		blend.A = 100;
		return blend;
	}
	public override bool PreAI() {
		bool isGettingYeeted = NPC.ai[3] != 1;
		NPC.noGravity = !isGettingYeeted;

		if (!isGettingYeeted)
			Dust.NewDustPerfect(NPC.Center, DustID.GemSapphire).noGravity = true;

		if (NPC.collideY || NPC.collideX)
			NPC.ai[3] = 0;
		return isGettingYeeted;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
		NPC.ai[3] = 0;
	}
	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
		NPC.ai[3] = 0;
	}
	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
		NPC.ai[3] = 0;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit) {
		NPC.ai[3] = 0;
	}
	// DOESNT WORK BECAUSE SLIME AI RED CODE WOOHOOO
	public override bool? CanFallThroughPlatforms() {
		return NPC.ai[3] == 1 ? false : base.CanFallThroughPlatforms();
	}
}
