using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;


namespace BossRush.Common.Graphics;
public struct TrailShaderSettings {

	public string shaderType;
	public Color Color;
	public Vector2[] oldPos;
	public float[] oldRot;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Vector4 shaderData;
	public Vector2 offset;
}

public struct ShaderSettings {

	public string shaderType;
	public Color Color;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Vector4 shaderData;
}
