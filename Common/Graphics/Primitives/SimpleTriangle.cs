using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.Primitives;
public class SimpleTriangle : ModSystem {



	Asset<Effect> shader;
	static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice; // shorthand property

	public override void PostDrawTiles() {
		Vector2 playerCenter = Main.LocalPlayer.Center + new Vector2(0, -100);
		VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[]
		{
			new VertexPositionColorTexture(new Vector3(playerCenter + new Vector2(100, 100), 0f), Color.Red, new Vector2(1,1)), // bottomright
			new VertexPositionColorTexture(new Vector3(playerCenter + new Vector2(-100, 100), 0f), Color.Blue,new Vector2(0,1)), // bottmoleft
			new VertexPositionColorTexture(new Vector3(playerCenter + new Vector2(100, -100), 0f), Color.Green, new Vector2(1,0)), // top
			new VertexPositionColorTexture(new Vector3(playerCenter + new Vector2(-100, -100), 0f), Color.Gainsboro, new Vector2(0,0)), // lefttop
			

		};
		var viewport = GraphicsDevice.Viewport;
		shader?.Value.Parameters["viewWorldProjection"].SetValue(Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix * Matrix.CreateOrthographicOffCenter(left: 0, right: viewport.Width, bottom: viewport.Height, top: 0, zNearPlane: -1, zFarPlane: 10));
		shader?.Value.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly);
		shader?.Value.Parameters["uColor"].SetValue(Color.Orange.ToVector3());
		shader?.Value.Parameters["shaderData"].SetValue(new Vector4(60, 0, 0, 0));
		GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		GraphicsDevice.Textures[0] = TextureAssets.Extra[193].Value; // set a texture to draw, in this case just a pixel

		shader?.Value.CurrentTechnique.Passes[0].Apply();
		GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
	}
}
