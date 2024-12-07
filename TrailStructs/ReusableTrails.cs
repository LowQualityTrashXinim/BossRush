using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Drawing;

namespace BossRush.TrailStructs;
internal class ReusableTrails {
}

public struct GenericTrailSettings 
{
	public string shaderType;
	public Color Color;
	public Vector2[] oldPos;
	public float[] oldRot;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Vector4 shaderData;
	public Vector2 offset;
}

public struct GenericTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(GenericTrailSettings GenericTrailSettings, VertexStrip.StripHalfWidthFunction stripWidth, VertexStrip.StripColorFunction stripColor) {

		MiscShaderData miscShaderData = GameShaders.Misc[GenericTrailSettings.shaderType];
		miscShaderData.UseImage1(GenericTrailSettings.image1);
		miscShaderData.UseImage2(GenericTrailSettings.image2);
		miscShaderData.UseShaderSpecificData(GenericTrailSettings.shaderData);
		miscShaderData.UseColor(GenericTrailSettings.Color);
		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(GenericTrailSettings.oldPos, GenericTrailSettings.oldRot, stripColor, stripWidth, -Main.screenPosition +GenericTrailSettings.offset, null, true);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
