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
using BossRush.Contents.Shaders;

namespace BossRush.TrailStructs;
internal class ReusableTrails {
}

public struct GenericTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(ShaderSettings GenericTrailSettings, VertexStrip.StripHalfWidthFunction stripWidth, VertexStrip.StripColorFunction stripColor) {

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
