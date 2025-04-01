using BossRush.Common.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Terraria;

namespace BossRush.Common.Graphics.Structs.QuadStructs;
public struct LaserQuad {
	private static PrimitiveDrawer QuadDrawer = new PrimitiveDrawer(PrimitiveShape.Quad);

	public void Draw(Vector2 position, float rotation, Vector2 size,ShaderSettings shaderSettings) 
	{

		ModdedShaderHandler shader = EffectsLoader.shaderHandlers["LaserEffect"];
		shader.setProperties(shaderSettings);
		shader.apply();


		QuadDrawer.Draw([position + rotation.ToRotationVector2() * (size.X * 0.5f)], [Color.White], [size], rotation, position + rotation.ToRotationVector2() * size.X / 2f);

	}

}
