using BossRush.Common.Graphics;using BossRush.Common.Graphics.Primitives;using BossRush.Texture;using Microsoft.Xna.Framework;using Microsoft.Xna.Framework.Graphics;using Newtonsoft.Json.Linq;using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;using Terraria;using Terraria.Audio;using Terraria.DataStructures;using Terraria.GameContent;using Terraria.Graphics.Shaders;using Terraria.ID;using Terraria.ModLoader;namespace BossRush.Contents.Projectiles;public class FlamelashWEProjectile : ModProjectile {	public override string Texture => BossRushTexture.MissingTexture_Default;		SimpleQuad SimpleQuad;	float shaderOffset;	bool exploding = false;	int randomSize;	public override void SetDefaults() {		Projectile.friendly = true;		Projectile.width = Projectile.height = 24;		Projectile.aiStyle = -1;		Projectile.DamageType = DamageClass.Magic;		Projectile.ignoreWater = true;		Projectile.timeLeft = 160;		SimpleQuad = new SimpleQuad();	}	public override void OnSpawn(IEntitySource source) {		exploding = false;		shaderOffset = Main.rand.NextFloat(0, MathHelper.TwoPi);		//Projectile.ai[2] = Main.rand.Next(128, 256);	}	float explosionEaseOut = 0.1f;	public override void AI() {		Projectile.ai[0]++;		if (Projectile.timeLeft == 60) 		{			Projectile.ai[1] = 1;			for (int i = 0; i < 15; i++)				Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(35, 35), DustID.InfernoFork);			Projectile.ai[0] = 0;
			exploding = true;
			Projectile.ai[2] = 0.1f;			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode,Projectile.Center);
			explosionEaseOut = 0.1f;		}		if (exploding) 
		{
			explosionEaseOut *= 0.95f;
			Projectile.ai[2] +=explosionEaseOut;
		}	}	public override bool PreDraw(ref Color lightColor) {		SimpleQuad.position = [Projectile.Center];		SimpleQuad.size = [new Vector2(256, 256)];		SimpleQuad.color = [Color.Orange,Color.Orange, Color.Orange, Color.Orange];		SimpleQuad.shaderHandler = new ModdedShaderHandler(ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBallPrimitive").Value);		SimpleQuad.shaderHandler.enabled = true;		SimpleQuad.shaderHandler.setProperties(Color.Goldenrod, TextureAssets.Extra[193].Value, shaderData: new Vector4(exploding ? 60f : Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));		SimpleQuad.Draw();

		if (exploding) 
		{
			SimpleQuad.position = [Projectile.Center];
			SimpleQuad.size = [new Vector2(256, 256)];
			SimpleQuad.color = [Color.Orange, Color.Orange, Color.Orange, Color.Orange];
			SimpleQuad.shaderHandler = new ModdedShaderHandler(ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/ExplosionPrimitive").Value);
			SimpleQuad.shaderHandler.enabled = true;
			SimpleQuad.shaderHandler.setProperties(Color.OrangeRed, TextureAssets.Extra[193].Value, shaderData: new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));
			SimpleQuad.Draw();

		}

		return false;	}}