using BossRush.Common.Graphics.Primitives;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BossRush.Common.Graphics.Structs.QuadStructs;
public struct RubyLaserQuad {
	private static PrimitiveDrawer QuadDrawer = new PrimitiveDrawer(PrimitiveShape.Quad);

	public void Draw(Vector2 position, float rotation, Vector2 size, ShaderSettings shaderSettings) {
		ModdedShaderHandler shader = EffectsLoader.shaderHandlers["RubyLaserEffect"];
		shader.setProperties(shaderSettings);
		shader.apply();
		QuadDrawer.Draw([position + rotation.ToRotationVector2() * (size.X * 0.5f)], [Color.White], [size], rotation, position + rotation.ToRotationVector2() * size.X / 2f);
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}

}
