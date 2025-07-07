using BossRush.Common.Graphics;
using BossRush.Common.Graphics.AnimationSystems;
using BossRush.Common.Graphics.Primitives;
using BossRush.Common.Graphics.Structs.QuadStructs;
using BossRush.Common.Graphics.Structs.TrailStructs;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ObjectSystem;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace BossRush.Common.RoguelikeChange.NPCsOverhaul.Bosses.EyeofCuthulu;
public class EyeofCuthulu : NPCReworkerFSM {
	public override int VanillaNPCType => NPCID.EyeofCthulhu;

	public override int frameHeight => 996 / 6;
	public bool isPhase2 = false;
	public override int startingFrame => isPhase2 ? 3 : 0;
	public override int maxFrames => 2;
	public override void ReworkedAI(ref NPC npc) {
		states.Update();
	}

	public override int[] RegisterStates() => [
			AIState.StateType<EoC_Idle>(),
			AIState.StateType<EoC_Dashing>(),
			AIState.StateType<EoC_PrepareDash>(),
			AIState.StateType<EoC_SummonSpin>(),
			AIState.StateType<EoC_Projectile>(),
			AIState.StateType<EoC_GoingPhase2>(),
			AIState.StateType<EoC_DashTeleport>(),
			AIState.StateType<EoC_RandomTeleport>(),
			AIState.StateType<EoC_DashAndFire>(),
			AIState.StateType<EoC_ParryStun>(),
			AIState.StateType<Boss_Despawn>(),
			AIState.StateType<EoC_CircleAroundTarget>(),
			AIState.StateType<EoC_SwarmShield>()];

	public override bool UseCustomAnimation() => true;

	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		if (states == null)
			return true;

		if (states.currentState is not EoC_RandomTeleport)
			spriteBatch.Draw(TextureAssets.Npc[VanillaNPCType].Value, npc.Center - screenPos, frameRect, drawColor, npc.rotation - MathHelper.PiOver2, new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f, frameHeight / 2f), npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

		if (states.currentState is EoC_Projectile state) {
			float progress = (float)state.counter / 30;

			BossRushUtils.DrawPrettyStarSparkle(1f, SpriteEffects.None, npc.Center + npc.rotation.ToRotationVector2() * 64 - screenPos, Color.Purple, Color.Red, progress, 0, 0.5f, 0.5f, 1f, npc.rotation, Vector2.One * 4, Vector2.One);
		}
		if (states.currentState is EoC_DashAndFire state2) {
			float progress = (float)state2.counter / 60;

			BossRushUtils.DrawPrettyStarSparkle(1f, SpriteEffects.None, npc.Center + npc.rotation.ToRotationVector2() * 128 - screenPos, Color.Purple, Color.Red, progress, 0, 0.5f, 0.5f, 1f, npc.rotation + MathHelper.TwoPi * progress, Vector2.One * 4, Vector2.One);
		}


		return false;
	}
}

public class ServerntOfCuthulu : NPCReworker {
	public override int VanillaNPCType => NPCID.ServantofCthulhu;
	NPC EoC = null;
	public override void ReworkedAI(ref NPC npc, Player target) {

		switch (state) {
			case State.Following: {

					float accel = Main.masterMode ? 0.015f : Main.expertMode ? 0.01f : 0.0075f;
					float d = npc.Distance(target.Center);
					float targetSpeed = d * accel;

					npc.rotation = target.DirectionFrom(npc.Center).ToRotation() - MathHelper.PiOver2;
					npc.velocity = Vector2.Lerp(npc.velocity, (npc.rotation + MathHelper.PiOver2).ToRotationVector2() * targetSpeed, 0.02f);

					foreach (var n in Main.ActiveNPCs) {

						if (n.getRect().Intersects(npc.getRect()) && n != npc)
							npc.position = npc.position + n.DirectionTo(npc.position);

					}
					if (++Counter >= 220)
						npc.StrikeInstantKill();
					break;
				}

			case State.Circling: {

					if (EoC.active) {
						Counter++;

						if (Counter >= 240) {
							Counter2++;
							if (Counter2 == 60) {

								npc.velocity = npc.rotation.ToRotationVector2() * 20;
								npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;


							}
							if(Counter2 < 60){
								npc.velocity *= 0.94f;
								ClearOldCache(npc);
							}

							afterimages = true;
						}
						else {
							npc.Center = npc.Center.ClampedLerp(new Vector2(MathF.Cos(-Counter * 0.15f + MathHelper.TwoPi * npc.ai[0] / 16), MathF.Sin(-Counter * 0.15f + MathHelper.TwoPi * npc.ai[0] / 16)) * 125 + EoC.Center, 0.5f);
							npc.rotation = npc.velocity.ToRotation();
							npc.velocity = EoC.Center.DirectionFrom(npc.Center) * 10;

						}

					}

					break;
				}
		}


	}
	enum State {

		Following,
		Circling,
		ChargingUp
	}

	State state {

		get { return (State)AIStateSync; }

		set {

			AIStateSync = (int)value;
		}
	}
	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		if (state == State.ChargingUp) {

			var shaderSettings = new ShaderSettings();
			shaderSettings.image1 = TextureAssets.Extra[193];
			shaderSettings.image2 = null;
			shaderSettings.image3 = null;
			shaderSettings.Color = Color.Cyan;
			shaderSettings.shaderData = new Vector4(0.5f, 500, 0, 0);
			default(ColorIndicatorQuad).Draw(npc.Center, npc.rotation - MathHelper.PiOver2, new Vector2(1000, 256), shaderSettings);
		}
		DrawAfterimages(npc,spriteBatch,screenPos,Color.Red );
		return true;
	}
	public override bool UseCustomAnimation() {
		return false;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		if (npc.ai[1] != 0) {
			EoC = Main.npc[(int)npc.ai[1]];
			state = State.Circling;
		}
	}
}

public class EvilFlamesProj : ModProjectile {

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override string Texture => "Terraria/Images/Projectile_0";
	public override void SetDefaults() {
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.width = Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
	}

	public override bool PreDraw(ref Color lightColor) {

		TrailShaderSettings settings = new TrailShaderSettings();
		settings.Color = Color.Purple;
		settings.image1 = TextureAssets.Extra[193];
		settings.shaderType = "EvilFlames";
		settings.oldPos = Projectile.oldPos;
		settings.oldRot = Projectile.oldRot;
		settings.offset = Projectile.Hitbox.Size() / 2f;

		default(GenericTrail).Draw(settings, (p => 15 * 0f.ClampedLerp(1, 5 * Projectile.timeLeft / 120f)), (_ => Color.White));

		return false;
	}

	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.velocity *= 1.01f;
		if (Projectile.ai[0] == 1)
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4 * 0.05f * Projectile.ai[1]);
		if (Projectile.ai[0] == 2) {

			Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.ai[1], MathHelper.PiOver4 * 0.05f).ToRotationVector2() * 14;
		}
	}

	public override void OnKill(int timeLeft) {

	}
}

public struct EyeRift {
	private static PrimitiveDrawer quad = new PrimitiveDrawer(PrimitiveShape.Quad);
	public void Draw(Vector2 position, float rotation, Vector2 size) {
		var moddedShaderHandler = EffectsLoader.shaderHandlers["Rift"];
		moddedShaderHandler.setProperties(Color.Purple, TextureAssets.Extra[193].Value);
		moddedShaderHandler.apply();
		quad.Draw([position], [Color.White], [size], rotation, position);
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
public struct LingeringEvilFlamesQuad {
	private static PrimitiveDrawer quad = new PrimitiveDrawer(PrimitiveShape.Quad);
	public void Draw(Vector2 position, float rotation, Vector2 size, float random, float progress) {
		var moddedShaderHandler = EffectsLoader.shaderHandlers["LingeringEvilFlames"];
		moddedShaderHandler.setProperties(Color.Purple, TextureAssets.Extra[193].Value, shaderData: new Vector4(random, progress, 0, 0));
		moddedShaderHandler.apply();
		quad.Draw([position], [Color.White], [size], rotation, position);
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}

public class EyeRiftObject : ModObject {
	public const int MaxTimeLeft = 120;
	TweenHandler<Vector2> tweenHandler = new TweenHandler<Vector2>();
	public override void SetDefaults() {
		timeLeft = MaxTimeLeft;
		tweenHandler = new TweenHandler<Vector2>();
		tweenHandler.tweens.Add(new Tween<Vector2>(Vector2.Lerp, false).SetProperties(new Vector2(0, 0), new Vector2(256, 256), TweenEaseType.None, 10));
		tweenHandler.tweens.Add(new Tween<Vector2>(Vector2.Lerp, true).SetProperties(new Vector2(256, 256), new Vector2(256, 256), TweenEaseType.InExpo, 20));
		tweenHandler.tweens.Add(new Tween<Vector2>(Vector2.Lerp, false).SetProperties(new Vector2(256, 256), new Vector2(0, 0), TweenEaseType.None, 20));
		tweenHandler.PlayTweens();
	}
	public override void AI() {
		tweenHandler.Update();
		Lighting.AddLight(position, TorchID.Crimson);
	}
	public override void Draw(SpriteBatch spritebatch) {
		default(EyeRift).Draw(position, rotation, tweenHandler.currentTween.currentProgress);
	}
}
public class LingeringEvilFlames : ModProjectile {

	public override string Texture => "Terraria/Images/Projectile_0";
	float randomOffset = Main.rand.NextFloat();
	public override void SetDefaults() {
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.width = Projectile.height = 150;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 360;
		Projectile.tileCollide = false;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.velocity = Vector2.Zero;
		tweenHandler = new();
		tweenHandler.tweens.Add(new Tween<float>(MathHelper.Lerp, false).SetProperties(0, 1, TweenEaseType.None, 10));
		tweenHandler.tweens.Add(new Tween<float>(MathHelper.Lerp, false).SetProperties(1, 1, TweenEaseType.None, 320));
		tweenHandler.tweens.Add(new Tween<float>(MathHelper.Lerp, false).SetProperties(1, 0, TweenEaseType.None, 30));
		tweenHandler.PlayTweens();

	}
	TweenHandler<float> tweenHandler;
	public override void AI() {
		Projectile.rotation = randomOffset * MathHelper.TwoPi;
		tweenHandler.Update();
		Projectile.scale = tweenHandler.currentTween.currentProgress;
	}

	public override bool PreDraw(ref Color lightColor) {
		default(LingeringEvilFlamesQuad).Draw(Projectile.Center - Main.screenPosition, Projectile.rotation, new Vector2(150 * Projectile.scale), randomOffset, MathHelper.Lerp(1, 0, BossRushUtils.InExpo((float)Projectile.timeLeft / 60, 11f)));

		return false;
	}
}
//public class LingeringEvilFlames : ModProjectile {

//	public Vector2[] points = new Vector2[30];
//	public float[] rots = new float[30];
//	int index = 0;
//	int maxIndex = 30;
//	NPC EoC => Main.npc[(int)Projectile.ai[0]];

//	public override string Texture => "Terraria/Images/Projectile_0";
//	public override void SetDefaults() {
//		Projectile.hostile = true;
//		Projectile.friendly = false;
//		Projectile.width = Projectile.height = 16;
//		Projectile.aiStyle = -1;
//		Projectile.penetrate = -1;
//		Projectile.timeLeft = 360;
//		Projectile.tileCollide = false;
//	}

//	public override void OnSpawn(IEntitySource source) {
//		Projectile.velocity = Vector2.Zero;
//	}

//	public override bool PreDraw(ref Color lightColor) {

//		TrailShaderSettings settings = new TrailShaderSettings();
//		settings.Color = Color.Purple;
//		settings.image1 = TextureAssets.Extra[193];
//		settings.shaderType = "LingeringEvilFlames";
//		settings.oldPos = points;
//		settings.oldRot = rots;
//		settings.offset = Projectile.Hitbox.Size() / 2f;
//		settings.shaderData = new Vector4(points[0].DirectionTo(points[1]).X,points[0].DirectionTo(points[1]).Y,0,0);

//		default(GenericTrail).Draw(settings, (p => 24), (_ => Color.White));
//		default(EyeRift).Draw(points[0],0,new Vector2(128));
//		default(EyeRift).Draw(points[index - 1],0,new Vector2(128));
//		return false;
//	}


//	public override void AI() {
//		if(index < maxIndex && EoC.active && EoC != null)
//		{
//			BossRushUtils.Push(ref points, EoC.Center);
//			BossRushUtils.Push(ref rots, EoC.rotation);		
//			index++;
//		}

//	}

//	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {

//		for(int i = 0; i < index - 1; i++)
//			if(BossRushUtils.Collision_PointAB_EntityCollide(targetHitbox,points[i],points[i + 1]))
//				return true;


//		return false;
//	}

//}
public class EoC_Idle : AIState {

	int randomDuration = 0;
	int attackCounter = 0;
	int nextAttack = StateType<EoC_Dashing>();

	public override void OnEntered(int oldState) {
		randomDuration = Main.rand.Next(300, 360);
		counter = Main.rand.Next(0, 240);
		mag = 0;

		switch (attackCounter) {
			case 1:
				nextAttack = StateType<EoC_PrepareDash>();
				break;
			case 2:
				if (npc.GetGlobalNPC<EyeofCuthulu>().isPhase2)
					nextAttack = StateType<EoC_DashTeleport>();
				else
					nextAttack = StateType<EoC_Projectile>();
				break;
			case 3:
				if (npc.GetGlobalNPC<EyeofCuthulu>().isPhase2)
					nextAttack = StateType<EoC_SwarmShield>();
				else
					nextAttack = StateType<EoC_DashAndFire>();
				break;
			default:
				attackCounter = 0;
				if (npc.GetGlobalNPC<EyeofCuthulu>().isPhase2)
					nextAttack = StateType<EoC_CircleAroundTarget>();
				else
					nextAttack = StateType<EoC_PrepareDash>();
				break;
		}
		attackCounter++;
		nextAttack = StateType<EoC_SwarmShield>();

	}

	float mag = 0;

	public override void OnStateUpdate(CommonNPCInfo info) {

		if (npc.life <= npc.lifeMax / 2f && !npc.GetGlobalNPC<EyeofCuthulu>().isPhase2)
			ChangeState(StateType<EoC_GoingPhase2>());

		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		npc.direction = Target.Center.X > npc.Center.X ? -1 : 1;
		npc.rotation = npc.rotation.AngleTowards(npc.DirectionTo(Target.Center).ToRotation(), MathHelper.Pi / 30f);
		Vector2 targetPos = Target.Center - new Vector2(0, 450);
		float d = npc.Distance(targetPos);

		float targetVelocity = d / 32;

		if (mag < 8 && mag < targetVelocity)
			mag += .08f;
		if (mag > targetVelocity)
			mag -= .08f;

		if (d > 32)
			npc.velocity = npc.DirectionTo(targetPos) * mag;
		else
			mag *= 0.95f;

		if (counter % 30 == 0)
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.NewNPCDirect(npc.GetSource_FromAI(), npc.Center, NPCID.ServantofCthulhu).velocity = Main.rand.NextVector2Circular(15, 15);

		if (counter >= randomDuration) {
			ChangeState(nextAttack);
		}
	}


}

public class EoC_Dashing : AIState {
	Vector2 targetVel = Vector2.Zero;
	int reDashCounter = 0;
	public override void OnEntered(int oldState) {
		targetVel = npc.rotation.ToRotationVector2() * 15 * (npc.GetGlobalNPC<EyeofCuthulu>().isPhase2 ? 1.25f : 1);

		if (npc.GetGlobalNPC<EyeofCuthulu>().isPhase2)
			SoundEngine.PlaySound(SoundID.ForceRoarPitched, npc.Center);

		if (oldState != StateType<EoC_Dashing>())
			reDashCounter = 0;
		else
			reDashCounter++;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {

		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		npc.velocity = targetVel * (1f - ((float)counter / 60));
		npc.rotation = npc.rotation.AngleTowards(Target.DirectionFrom(npc.Center).ToRotation(), (((float)counter / 60)));
		if (counter == 60)
			if (reDashCounter < 2)
				ChangeState(AIState.StateType<EoC_Dashing>());
			else
				ChangeState(AIState.StateType<EoC_SummonSpin>());

	}
}

public class EoC_PrepareDash : AIState {
	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.velocity *= 0.95f;

		if (counter == 30)
			ChangeState(StateType<EoC_Dashing>());
	}
}

public class EoC_SummonSpin : AIState {
	public override void OnStateUpdate(CommonNPCInfo info) {

		npc.rotation += 0.650f * npc.direction * Terraria.Utils.PingPongFrom01To010(((float)counter / 60));
		if (counter % 10 == 0)
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.NewNPCDirect(npc.GetSource_FromAI(), npc.Center, NPCID.ServantofCthulhu).velocity = Main.rand.NextVector2Circular(15, 15);

		if (counter == 60)
			ChangeState(StateType<EoC_Idle>());

	}
}

public class EoC_Projectile : AIState {
	int tpCounter = 0;
	public override void OnEntered(int oldState) {
		if (oldState != StateType<EoC_RandomTeleport>())
			tpCounter = 0;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.velocity *= 0.92f;

		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		if (counter < 20)
			npc.rotation = npc.DirectionTo(Target.Center).ToRotation();

		if (counter == 20) {
			npc.velocity = npc.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi) * 5;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				for (int i = 0; i < 5; i++)
					Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedByRandom(0.2f) * 15, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0);
		}

		if (counter == 50) {
			if (tpCounter < 5) {
				ChangeState(StateType<EoC_RandomTeleport>());
				tpCounter++;
			}
			else
				ChangeState(StateType<EoC_Idle>());
		}


	}
}

public class EoC_RandomTeleport : AIState {

	int prevState = 0;
	Vector2 sideTarget;
	public override void OnEntered(int oldState) {
		prevState = oldState;
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		if (npc.Center.X < Target.Center.X)
			sideTarget = new Vector2(400, 0);
		else
			sideTarget = new Vector2(-400, 0);

		ModObject.NewModObject(npc.Center, Vector2.Zero, ModObject.GetModObjectType<EyeRiftObject>());
	}

	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.velocity *= 0;

		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;

		if (counter == 30) {
			npc.Center = Target.Center + Main.rand.NextVector2CircularEdge(0, 400) + sideTarget;
			ModObject.NewModObject(npc.Center, Vector2.Zero, ModObject.GetModObjectType<EyeRiftObject>());
		}

		if (counter == 60) {
			ChangeState(prevState);
		}
	}

}

public class EoC_GoingPhase2 : AIState {
	float rot = 0;
	public override void OnEntered(int oldState) {
		rot = npc.rotation;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.rotation += 0.650f * npc.direction * Terraria.Utils.PingPongFrom01To010(((float)counter / 60));

		if (counter == 30)
			npc.GetGlobalNPC<EyeofCuthulu>().isPhase2 = true;

		if (counter == 60)
			ChangeState(StateType<EoC_Idle>());
	}

}

public class EoC_DashTeleport : AIState {
	int tpCount = 0;
	public override void OnEntered(int oldState) {
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;

		if (oldState == StateType<EoC_RandomTeleport>())
			tpCount++;
		else
			tpCount = 0;


		SoundEngine.PlaySound(SoundID.ForceRoarPitched, npc.Center);
		npc.rotation = Target.DirectionFrom(npc.Center).ToRotation();

		if (Main.netMode != NetmodeID.MultiplayerClient)
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 5 * i) * 15, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 1, -1);
				Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 5 * i) * 15, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 1, 1);

			}
	}

	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.velocity = npc.rotation.ToRotationVector2() * 20 * BossRushUtils.InExpo(1f - (float)counter / 60, ((float)counter / 60) * 5f);
		if (counter == 60)
			if (tpCount < 6)
				ChangeState(StateType<EoC_RandomTeleport>());
			else
				ChangeState(StateType<EoC_Idle>());
	}
}

public class EoC_DashAndFire : AIState {
	Vector2 sideTarget = Vector2.Zero;
	int dashCount = 0;
	public override void OnEntered(int oldState) {
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;

		if (npc.Center.X < Target.Center.X)
			sideTarget = Target.Center - new Vector2(600, Main.rand.NextFloat(-400, 400));
		else
			sideTarget = Target.Center + new Vector2(600, Main.rand.NextFloat(-400, 400));

		if (oldState == StateType<EoC_DashAndFire>() || oldState == StateType<EoC_RandomTeleport>())
			dashCount++;
		else
			dashCount = 0;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;

		npc.velocity *= 0.92f;
		npc.rotation = Target.DirectionFrom(npc.Center).ToRotation();
		if (counter <= 30) {
			npc.rotation = Target.DirectionFrom(npc.Center).ToRotation();
			npc.Center = npc.Center.ClampedLerp(sideTarget, (float)counter / 30);
		}

		if (counter == 30) {
			if (Main.netMode != NetmodeID.MultiplayerClient)
				for (int i = 0; i < 4; i++) {

					Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi + (MathHelper.Pi / 6 * i)) * 6, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 2, npc.rotation);
					Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi - (MathHelper.Pi / 6 * i)) * 6, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 2, npc.rotation);
				}
			Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2() * 15, ModContent.ProjectileType<EvilFlamesProj>(), npc.damage, 0, -1, 2, npc.rotation);

			npc.velocity = -npc.rotation.ToRotationVector2() * 25;

		}
		if (counter == 100) {
			if (dashCount < 7)
				if (dashCount == 3)
					ChangeState(StateType<EoC_RandomTeleport>());
				else
					ChangeState(StateType<EoC_DashAndFire>());
			else
				ChangeState(StateType<EoC_Idle>());
		}

	}
}

public class EoC_ParryStun : AIState {
	public override void OnEntered(int oldState) {
		npc.velocity = Target.Center.DirectionTo(npc.Center) * 11;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.velocity *= 0.95f;
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		npc.rotation += 1f * npc.direction * BossRushUtils.InExpo(1f - (float)counter / 100, 4f);
		if (counter == 160)
			ChangeState(StateType<EoC_Idle>());
	}
}

public class EoC_CircleAroundTarget : AIState {

	Vector2 targetPos;
	int dir;
	public override void OnEntered(int oldState) {
		ModObject.NewModObject(npc.Center, Vector2.Zero, ModObject.GetModObjectType<EyeRiftObject>());
		npc.velocity = Vector2.Zero;
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;
		targetPos = Target.Center;
		dir = npc.direction;
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;

		if (counter == 30) {
			npc.Center = targetPos + new Vector2(0, 450);
			ModObject.NewModObject(npc.Center, Vector2.Zero, ModObject.GetModObjectType<EyeRiftObject>());
		}

		if (counter > 30) {
			Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<LingeringEvilFlames>(), (int)(npc.damage * 0.15f), 0, -1);
			npc.Center = npc.Center.RotatedBy(0.2f * -dir * ((float)counter / 60), targetPos);
			npc.rotation = npc.position.DirectionFrom(npc.oldPosition).ToRotation();
		}

		if (counter == 60) {
			ChangeState(StateType<EoC_Idle>());
		}
	}

}

public class EoC_SwarmShield : AIState {
	public override void OnEntered(int oldState) {
		for (int i = 0; i < 16; i++) {
			NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, 0, i, npc.whoAmI);
		}
	}
	public override void OnStateUpdate(CommonNPCInfo info) {
		if (ChangeStateIfTargetNull(StateType<Boss_Despawn>()))
			return;


		if (counter < 120) {
			npc.velocity *= 0.96f;
			npc.rotation = npc.rotation.AngleTowards(Target.DirectionFrom(npc.Center).ToRotation(), 0.35f);
		}
		if (counter == 120) {
			npc.velocity = npc.rotation.ToRotationVector2() * 30;
		}
		if (counter > 120) {
			if (Main.netMode != NetmodeID.MultiplayerClient && counter % 5 == 0) {
				Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * 5, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 0, 0);
				Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, npc.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) * 5, ModContent.ProjectileType<EvilFlamesProj>(), (int)(npc.damage * 0.15f), 0, -1, 0, 0);
			}
			if (npc.justHit && Target.itemAnimation >= (Target.itemAnimationMax / 2) && Target.HeldItem.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
				if (meleeItem.SwingType == BossRushUseStyle.Swipe) {
					ChangeState(StateType<EoC_ParryStun>());
					return;
				}
			}
		}

		if(counter > 150)
			npc.velocity *= 0.92f;
		if (counter == 190) {
			ChangeState(StateType<EoC_Idle>());
		}

	}
}
