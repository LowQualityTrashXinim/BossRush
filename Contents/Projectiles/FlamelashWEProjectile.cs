using BossRush.Common.General;
using BossRush.Contents.Shaders;
using BossRush.RenderTargets;
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
using static tModPorter.ProgressUpdate;

namespace BossRush.Contents.Projectiles;
public class FlamelashWEProjectile : ModProjectile, IDrawsShader {

	public override string Texture => BossRushTexture.MissingTexture_Default;
	public ShaderGlobalProjectile globalProjectile;
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

	public override bool PreDraw(ref Color lightColor) {

		Main.EntitySpriteDraw(TextureAssets.Extra[182].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2f, 3f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);


		return true;
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
		ModShaderData sd = new ModShaderData();
		sd.shaderSettings.shaderType = ShadersID.FlameBallShader;
		sd.shaderSettings.Color = Color.Orange;
		sd.shaderSettings.shaderData = new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset);
		sd.enabled = true;
		sd.position = Projectile.Center;
		sd.shaderSettings.image1 = TextureAssets.Extra[193];
		Projectile.GetGlobalProjectile<ShaderGlobalProjectile>().shaderData = sd;
	}

	public void PreShaderDraw(ref Color lightColor) {

		Main.EntitySpriteDraw(ModContent.Request<Texture2D>(BossRushTexture.MissingTexture_Default).Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size()/2f,0.5f,Microsoft.Xna.Framework.Graphics.SpriteEffects.None);

	}
}

