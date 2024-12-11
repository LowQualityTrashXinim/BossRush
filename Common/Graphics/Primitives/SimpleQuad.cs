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
public class SimpleQuad : PrimitivesBase {

	
	public override void DrawPrimitive(Effect effect, Vector2 position, float Size) {

		VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[]
		{
			new VertexPositionColorTexture(new Vector3(position + new Vector2(Size/2f, Size/2f), 0f), Color.Red, new Vector2(1,1)), // bottomright
			new VertexPositionColorTexture(new Vector3(position + new Vector2(-(Size/2f), Size / 2f), 0f), Color.Blue,new Vector2(0,1)), // bottmoleft
			new VertexPositionColorTexture(new Vector3(position + new Vector2(Size/2f, -(Size/2f)), 0f), Color.Green, new Vector2(1,0)), // topright
			new VertexPositionColorTexture(new Vector3(position + new Vector2(-(Size/2f), -(Size/2f)), 0f), Color.Gainsboro, new Vector2(0,0)), // lefttop
			

		};
		if(shaderHandler != null)
			shaderHandler.apply();

		GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}


}

