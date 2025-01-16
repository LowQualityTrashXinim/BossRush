using BossRush.Common.Graphics;using BossRush.Common.Graphics.Primitives;using BossRush.Common.Graphics.RenderTargets;using BossRush.Texture;using Microsoft.Xna.Framework;using Microsoft.Xna.Framework.Graphics;using Newtonsoft.Json.Linq;using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;using Terraria;using Terraria.Audio;using Terraria.DataStructures;using Terraria.GameContent;using Terraria.Graphics.Shaders;using Terraria.ID;using Terraria.ModLoader;using Terraria.ModLoader.IO;namespace BossRush.Contents.Projectiles;public class FlamelashWEProjectile : ModProjectile {	public override string Texture => BossRushTexture.MissingTexture_Default;
	private static ModdedShaderHandler shader;
	private static ModdedShaderHandler explosionShader;
	public override void Load() {
		explosionShader = new ModdedShaderHandler(EffectsLoader.loadedShaders["ExplosionPrimitive"].Value);
		shader = new ModdedShaderHandler(EffectsLoader.loadedShaders["FlameBallPrimitive"].Value);

	}

	float shaderOffset;	bool exploding = false;	int randomSize;
	PrimitiveDrawer quad0;
	PrimitiveDrawer quad1;	public override void SetDefaults() {		Projectile.friendly = false;		Projectile.width = Projectile.height = 128;		Projectile.aiStyle = -1;		Projectile.DamageType = DamageClass.Magic;		Projectile.ignoreWater = true;		Projectile.timeLeft = 160;	}	public override void OnSpawn(IEntitySource source) {		exploding = false;		shaderOffset = Main.rand.NextFloat(0, MathHelper.TwoPi);
		//Projectile.ai[2] = Main.rand.Next(128, 256);
		quad0 = new PrimitiveDrawer(PrimitiveShape.Quad);
		quad1 = new PrimitiveDrawer(PrimitiveShape.Quad);

	}	float explosionEaseOut = 0.1f;	public override void AI() {		Projectile.ai[0]++;		if (Projectile.timeLeft == 60) 		{			Projectile.ai[1] = 1;			for (int i = 0; i < 15; i++)				Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(35, 35), DustID.InfernoFork);			Projectile.ai[0] = 0;
			exploding = true;
			Projectile.ai[2] = 0.1f;			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode,Projectile.Center);
			explosionEaseOut = 0.1f;		}		if (exploding) 
		{
			Projectile.friendly = true;
			explosionEaseOut *= 0.95f;
			Projectile.ai[2] +=explosionEaseOut;
		}	}


	
	public override bool PreDraw(ref Color lightColor) {

		shader.setupTextures();
		shader.setProperties(Color.Orange, TextureAssets.Extra[193].Value, shaderData: new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));
		shader.apply();
		quad0.Draw([Projectile.Center], [Color.White], [new Vector2(128)]);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		if (exploding) 
		{
		

			explosionShader.setupTextures();
			explosionShader.setProperties(Color.Goldenrod, TextureAssets.Extra[193].Value, shaderData: new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));
			explosionShader.apply();
			quad1.Draw([Projectile.Center], [Color.White], [new Vector2(128)]);

			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		}
		

		return false;	}
}