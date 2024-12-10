using BossRush.Common.Graphics;
using BossRush.Texture;
using BossRush.TrailStructs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Projectiles;
public class DeathSickleGhost : ModProjectile {

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
	}
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.DeathSickle);
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 1000;

	}
	TrailShaderSettings gts;

	public override void OnSpawn(IEntitySource source) {
		SoundEngine.PlaySound(SoundID.Item71,Projectile.Center);
		for (int i = 0; i < 15; i++)
			Dust.NewDust(Projectile.position, (int)Projectile.Size.X, (int)Projectile.Size.Y, DustID.CrystalSerpent_Pink);
	}

	public override bool PreDraw(ref Color lightColor) {

		Asset<Texture2D> texture = TextureAssets.Item[ItemID.DeathSickle];
		Main.instance.LoadItem(ItemID.DeathSickle);

		float offset = 5f + MathF.Sin(Projectile.frameCounter );

		SpriteEffects spriteEffect = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


		if (currentState == State.dashing || currentState == State.readjust) {
			gts = new();
			gts.oldPos = [Projectile.Center, preDashPosition];
			gts.oldRot = [Projectile.rotation + MathHelper.PiOver4, Projectile.velocity.ToRotation()];
			gts.image1 = ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE);
			gts.Color = Color.Purple;
			gts.offset = Vector2.Zero;
			gts.shaderType = MiscShadersID.TrailShader;
			gts.shaderData = new Vector4(0, 0, 0, 0);
			gts.image2 = ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE);

			default(GenericTrail).Draw(gts, stripWidth, stripColor);
			for (float i = 0; i < Projectile.oldPos.Length; i++)
				Main.EntitySpriteDraw(texture.Value, Projectile.oldPos[(int)i] - Main.screenPosition + Projectile.Size / 2f, null, Color.Purple * Utils.GetLerpValue(0.5f,0f,i / 30f,true), delay * 0.7f * i / 20f, texture.Size() / 2f, 1f, spriteEffect);


		}


		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(offset,0) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(-offset, 0) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(0, offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(0, -offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);

		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(offset, offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(-offset, offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(-offset, offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);
		Main.EntitySpriteDraw(texture.Value, Projectile.Center + new Vector2(-offset, -offset) - Main.screenPosition, null, Color.Purple, Projectile.rotation, texture.Size() / 2f, 1f, spriteEffect);



		Main.EntitySpriteDraw(texture.Value,Projectile.Center - Main.screenPosition, null,Color.White,Projectile.rotation,texture.Size()/2f, 1f, spriteEffect);




		return false;
	}

	private Color stripColor(float p) 
	{

		return new Color(255,255,255,0);

	}


	private float stripWidth(float p) {

		return 7;

	}

	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 15; i++)
			Dust.NewDust(Projectile.position,(int)Projectile.Size.X,(int)Projectile.Size.Y,DustID.CrystalSerpent_Pink);
	}

	Vector2 dashTarget = Vector2.Zero;
	Vector2 preDashPosition = Vector2.Zero;
	public override void AI() {

		Projectile.frameCounter++;

		if (Projectile.velocity.X < 0)
			Projectile.direction = 1;
		else
			Projectile.direction = -1;

		float maxDelay = 60f;

		switch(currentState) 
		{

			case State.readjust:

				Projectile.velocity += -Vector2.UnitY * 15;
				currentState = State.idle;

				timer1 = 0;
				timer2 = 60f;
				delay = maxDelay;

				break;

			case State.idle:

				delay--;
				timer1--;
				timer2--;

				if (delay > 0) 
				{
					Projectile.velocity *= 0.95f;
					Projectile.rotation += MathHelper.Lerp(0f,1f,timer2 / maxDelay);
					return;
				}


				dashTarget = BossRushUtils.LookForHostileNPCPositionClosest(Projectile.Center,2000, false);

				if(dashTarget == Vector2.Zero) 
				{

					Projectile.velocity.Y = MathF.Sin(timer1);
					Projectile.rotation = Projectile.rotation.AngleTowards(0,0.1f);
				
				} else 
				{

					currentState = State.charging;
					timer1 = 0;
					timer2 = 0;
					delay = maxDelay / 2f;

				}

				break;

			case State.charging:


				delay--;

				Projectile.rotation += MathHelper.Lerp(0.33f * Projectile.direction, 0f * Projectile.direction, delay / (maxDelay / 2f));


				if(delay <= 0) 
				{

					Projectile.velocity = Projectile.Center.DirectionTo(dashTarget) * 65;
					currentState = State.dashing;
					delay = (int)(Projectile.Center.Distance(dashTarget) / 65);
					SoundEngine.PlaySound(SoundID.Item71 with { Pitch = -1f }, Projectile.Center);

					preDashPosition = Projectile.Center;
				}

				break;


			case State.dashing:

				delay--;
				Projectile.rotation =  Projectile.velocity.ToRotation() * Projectile.direction;

				//preDashPosition = Vector2.Lerp(Projectile.Center, preDashPosition, delay / 30f);

				if (delay <= 0) 
				{
					currentState = State.readjust;
					Projectile.velocity = Vector2.Zero;


					dashTarget = BossRushUtils.LookForHostileNPCPositionClosest(Projectile.Center, 2000, false);

					if (dashTarget != Vector2.Zero) {
						for (int i = 0; i < 7; i++)
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.Center.DirectionTo(dashTarget).RotatedBy(MathHelper.ToRadians(i * 360 / 7)) * 15, ModContent.ProjectileType<DeathSickleClone>(), Projectile.damage / 10, 0, Projectile.owner);

					}


					dashTarget = Vector2.Zero;
					Projectile.Kill();
				}

				break;


		}


	}
	public float timer1 {
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public float timer2 {
		get => (int)Projectile.ai[1];
		set => Projectile.ai[1] = value;
	}

	public float delay {
		get => (int)Projectile.ai[2];
		set => Projectile.ai[2] = value;
	}
	public State currentState = State.readjust;

	public enum State 
	{
	
		readjust,
		idle,
		charging,
		dashing
	
	}

}

public class DeathSickleClone : ModProjectile 
{

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.DeathSickle);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.DeathSickle);
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.penetrate = 1;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}

}
