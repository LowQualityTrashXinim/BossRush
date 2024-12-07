using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossRush.Contents.Shaders;
public struct ShaderSettings {

	public string shaderType;
	public Color Color;
	public Vector2[] oldPos;
	public float[] oldRot;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Vector4 shaderData;
	public Vector2 offset;
}
