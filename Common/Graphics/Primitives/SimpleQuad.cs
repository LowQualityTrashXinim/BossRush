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
/// <summary>
/// position[0] only
/// size[0].X = width, size[0].Y = height.
/// each corner has a different color, color[0] bottomright corner, color[1] bottonleft corner, etc...
/// </summary>
public class SimpleQuad : PrimitivesBase {


	public override VertexPositionColorTexture[] DrawPrimitive(Vector2[] position, Vector2[] size, Color[] color) {

		VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[]
		{
			new VertexPositionColorTexture(new Vector3(position[0] + new Vector2(size[0].X/2f, size[0].Y/2f), 0f), color[0], new Vector2(1,1)), // bottomright
			new VertexPositionColorTexture(new Vector3(position[0] + new Vector2(-(size[0].X/2f), size[0].Y / 2f), 0f), color[1],new Vector2(0,1)), // bottmoleft
			new VertexPositionColorTexture(new Vector3(position[0] + new Vector2(size[0].X/2f, -(size[0].Y/2f)), 0f), color[2], new Vector2(1,0)), // topright
			new VertexPositionColorTexture(new Vector3(position[0] + new Vector2(-(size[0].X/2f), -(size[0].Y/2f)), 0f), color[3], new Vector2(0,0)), // lefttop
			
			
		};

		return vertices;

	}


}

