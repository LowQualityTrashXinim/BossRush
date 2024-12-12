using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;

namespace BossRush.Common.Graphics.Primitives;
public abstract class PrimitivesBase {

	protected static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	public ModdedShaderHandler shaderHandler = null;
	public PrimitiveType type = PrimitiveType.TriangleStrip;
	public int primitiveCount = 1;
	public Vector2[] size;
	public Vector2[] position;
	public Color[] color;
	public VertexPositionColorTexture[] vertices;
	public abstract VertexPositionColorTexture[] DrawPrimitive(Vector2[] position,Vector2[] size,Color[] color);

	public virtual void Draw() {

		vertices = DrawPrimitive(position,size,color);

		if (shaderHandler != null)
			shaderHandler.apply();

		GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	
}
