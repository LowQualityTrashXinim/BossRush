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

namespace BossRush.Common.Graphics.Structs.TrailStructs;

// first, we make a struct, because that is how vanilla does it
public struct GenericTrail {
	// VertexStrip is basically Line2D Node from Godot, you create one as private static inside the struct
	private static VertexStrip _vertexStrip = new VertexStrip();
	// we call the Draw method inside a Draw hook like this: default(GenericTrail).Draw(x,y,z); because vanilla does it that way
	public void Draw(TrailShaderSettings GenericTrailSettings, VertexStrip.StripHalfWidthFunction stripWidth, VertexStrip.StripColorFunction stripColor) {
		// you can draw the Trail without a shader, it looks almost exactly like Line2D from Godot without shaders, here we want to add shader to the trail, so we copy a shader (also Known as Effect) from a Dictionary i made (vanilla also has one called GameShaders.xyz)
		ModdedShaderHandler shader = EffectsLoader.shaderHandlers[GenericTrailSettings.shaderType];
		shader.enabled = true;
		// here we set the C# input for our shader, basically we pass the texture, color, extra info, etc... and pass it over to HLSL
		shader.setProperties(GenericTrailSettings.Color, GenericTrailSettings.image1?.Value, GenericTrailSettings.image2?.Value,GenericTrailSettings.image3?.Value, shaderData: GenericTrailSettings.shaderData);
		//here we apply the shader and set it as the current shader inside spritebatch, basically everything after this line is going to have the shader applied, to prevent this we add this line after we are done with the shader: Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		shader.apply();

		// here we setup the strip, so basically the pos array, rot array, color delegate, width delegate, and the offset (like -Main.screenPosition)
		_vertexStrip.PrepareStripWithProceduralPadding(GenericTrailSettings.oldPos, GenericTrailSettings.oldRot, stripColor, stripWidth, GenericTrailSettings.offset - Main.screenPosition, true);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
