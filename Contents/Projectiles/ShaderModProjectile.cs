using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
public abstract class ShaderModProjectile : ModProjectile {
	public MiscShaderData _shader;
	public sealed override bool PreDraw(ref Color lightColor) {
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate,Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend);

		DrawShader(ref lightColor);

		return false;
	}

	public sealed override void PostDraw(Color lightColor) {
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		Main.spriteBatch.End();
		Main.spriteBatch.Begin();
	}

	public virtual void DrawShader(ref Color lightColor) { }

}
