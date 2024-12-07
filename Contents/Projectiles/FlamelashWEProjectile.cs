using BossRush.Contents.Shaders;
using BossRush.RenderTargets;
using BossRush.Texture;
using Microsoft.Xna.Framework;
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
public class FlamelashWEProjectile : ShaderModProjectile {

	public override string Texture => BossRushTexture.MissingTexture_Default; 
	public override void SetDefaults() {
		Projectile.friendly = true;
		Projectile.width = Projectile.height = 24;
		Projectile.aiStyle = -1;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = 120;
	}

	int shaderOffset;
	bool exploding = false;

	public override void OnSpawn(IEntitySource source) {
		exploding = false;
		shaderOffset = Main.rand.Next(0, 10000);
	}

	public override void DrawShader(ref Color lightColor) {
		RT128x128LoaderAndUnloader.rt.Request();


		if (RT128x128LoaderAndUnloader.rt.IsReady) {

			_shader = GameShaders.Misc[ShadersID.FlameBallShader];
			_shader.UseImage1("Images/Extra_" + (short)193);
			_shader.UseColor(Color.Orange);
			_shader.UseShaderSpecificData(new Vector4(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], shaderOffset));
			_shader.Apply();

			Main.EntitySpriteDraw(RT128x128LoaderAndUnloader.rt.GetTarget(), Projectile.position - new Vector2(192f / 2f) / 2f - Main.screenPosition, null, Color.White, 0, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);


		}

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
}
