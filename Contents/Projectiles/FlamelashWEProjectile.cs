using BossRush.Common.Graphics;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
public class FlamelashWEProjectile : ModProjectile, IDrawsShader {

	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.friendly = true;
		Projectile.width = Projectile.height = 24;
		Projectile.aiStyle = -1;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = 120;

	}

	float shaderOffset;
	bool exploding = false;
	int randomSize;


	public override void OnSpawn(IEntitySource source) {
		exploding = false;
		shaderOffset = Main.rand.NextFloat(0, MathHelper.TwoPi);
		//Projectile.ai[2] = Main.rand.Next(128, 256);
	}



	public override void AI() {
		Projectile.ai[0]++;


		if (Projectile.timeLeft == 40) 
		{
			Projectile.ai[1] = 1;
			for (int i = 0; i < 15; i++)
				Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2CircularEdge(35, 35), DustID.InfernoFork);

			exploding = true;
		}

		if (exploding)
			Projectile.ai[2] += 0.1f;
	}

	public void updateShader() {
		Projectile.GetGlobalProjectile<ShaderGlobalProjectile>().shader = new ModdedShaderHandler(EffectsLoader.flameBall.Value);
		Projectile.GetGlobalProjectile<ShaderGlobalProjectile>().shader.enabled = true;
		Projectile.GetGlobalProjectile<ShaderGlobalProjectile>().shader.setProperties(Color.Orange, TextureAssets.Extra[193].Value, shaderData: new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));
		
	}
	public override bool PreDraw(ref Color lightColor) {

		Main.EntitySpriteDraw(TextureAssets.Extra[183].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2f, 5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);


		return false;
	}
	public void preDrawWithoutShader(ref Color lightColor) {
		Main.EntitySpriteDraw(ModContent.Request<Texture2D>(BossRushTexture.MissingTexture_Default).Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2f, 15f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);

	}

	public void postDrawWithoutShader(Color lightcolor) {
		Main.EntitySpriteDraw(ModContent.Request<Texture2D>(BossRushTexture.MissingTexture_Default).Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2f, 0.5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);

	}

	public override void PostDraw(Color lightColor) {
		Main.EntitySpriteDraw(ModContent.Request<Texture2D>(BossRushTexture.MissingTexture_Default).Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2f, 5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);

	}
}
