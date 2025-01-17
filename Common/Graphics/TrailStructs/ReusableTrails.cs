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
using BossRush.Common.Graphics;

namespace BossRush.Common.Graphics.TrailStructs;
public struct GenericTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(TrailShaderSettings GenericTrailSettings, VertexStrip.StripHalfWidthFunction stripWidth, VertexStrip.StripColorFunction stripColor) {

		ModdedShaderHandler shader = EffectsLoader.shaderHandlers[GenericTrailSettings.shaderType];
		shader.enabled = true;
		shader.setProperties(GenericTrailSettings.Color, GenericTrailSettings.image1.Value, null, shaderData: GenericTrailSettings.shaderData);
		shader.apply();

		_vertexStrip.PrepareStrip(GenericTrailSettings.oldPos, GenericTrailSettings.oldRot, stripColor, stripWidth, -Main.screenPosition + GenericTrailSettings.offset, null, true);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
