using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Common.Graphics.Primitives;

public enum PrimitiveShape : byte
{

	Quad = 0,
	Trail = 1,

}

public class PrimitiveDrawer {

	public PrimitiveDrawer(PrimitiveShape shape, int verticesAmount = 4) 
	{
		
		this.Shape = shape;
		Array.Resize(ref vertices, verticesAmount);
		Array.Resize(ref indices, (verticesAmount) * 6);
		InitIndices();

	}


	private struct CustomVertex : IVertexType {
		public VertexDeclaration VertexDeclaration => new VertexDeclaration(new VertexElement(0,VertexElementFormat.Vector2,VertexElementUsage.Position,0), new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
		public Vector2 Position;
		public Color Color;
		public Vector2 TexCoord;

		public override string ToString() {
			return Position.ToString();
		}
	}
	private CustomVertex[] vertices = new CustomVertex[1];
	private PrimitiveShape Shape;
	private short[] indices = new short[1];
	private static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	public void Draw(Vector2[] positions, Color[] colors = default, Vector2[] size = default, float rotation = 0, Vector2 rotationCenter = default) 
	{


		switch (Shape) 
		{
		
			case PrimitiveShape.Quad:

				vertices[0].Position = (positions[0] + new Vector2(-size[0].X / 2f, -size[0].Y / 2f)).RotatedBy(rotation, rotationCenter);
				vertices[1].Position = (positions[0] + new Vector2(size[0].X / 2f, -size[0].Y / 2f)).RotatedBy(rotation, rotationCenter);
				vertices[2].Position = (positions[0] + new Vector2(-size[0].X / 2f, size[0].Y / 2f)).RotatedBy(rotation, rotationCenter);
				vertices[3].Position = (positions[0] + new Vector2(size[0].X / 2f, size[0].Y / 2f)).RotatedBy(rotation, rotationCenter);

				vertices[0].TexCoord = Vector2.Zero;
				vertices[1].TexCoord = new Vector2(1, 0);
				vertices[2].TexCoord = new Vector2(0, 1);
				vertices[3].TexCoord = Vector2.One;

				vertices[0].Color = colors[0];
				vertices[1].Color = colors[0];
				vertices[2].Color = colors[0];
				vertices[3].Color = colors[0];

				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0,vertices.Length, indices, 0, 2);

			break;

			case PrimitiveShape.Trail:

				 


			break;


		}


	}

	private void InitIndices() 
	{
		
		for (int i = 0; i < vertices.Length - 1; i++) 
		{
			indices[i * 6]	   = (short)(i);
			indices[i * 6 + 1] = (short)(i + 1);
			indices[i * 6 + 2] = (short)(i + 2);
			
			indices[i * 6 + 3] = (short)(i + 3);
			indices[i * 6 + 4] = (short)(i + 1);
			indices[i * 6 + 5] = (short)(i + 2);
			
			

		}


	}

}


