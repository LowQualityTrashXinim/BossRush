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
public static class PrimitivesDrawer {
	private struct CustomVertex : IVertexType {
		public VertexDeclaration VertexDeclaration => new VertexDeclaration(new VertexElement(0,VertexElementFormat.Vector2,VertexElementUsage.Position,0), new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
		public Vector2 Position;
		public Color Color;
		public Vector2 TexCoord;

		public override string ToString() {
			return Position.ToString();
		}
	}

	[InlineArray(4)]
	private struct triangleVetrices {
		private CustomVertex element0;
	}

	private static short[] indices = new short[1];
	private static Triangle[] segments;
	private const short aSingleSegementCorners = 6;
	private static short amountOfVerticesContainednotIncludingHeadTail;

	private struct Triangle {
		public Vector2 setSeg {
			set {
				var center = value;
				vertices[0].Color = color;
				vertices[0].Position = new Vector2(center.X - size.X, center.Y - size.Y).RotatedBy(rotation, center);
				vertices[1].Color = color;
				vertices[1].Position = new Vector2(center.X - size.X, center.Y + size.Y).RotatedBy(rotation, center);
				vertices[3].Color = color;
				vertices[3].Position = new Vector2(center.X + size.X, center.Y - size.Y).RotatedBy(rotation, center);
				vertices[2].Color = color;
				vertices[2].Position = new Vector2(center.X + size.X, center.Y + size.Y).RotatedBy(rotation, center);
			}
		}
		public float rotation;
		public Vector2 size;
		public Color color;
		public bool flip;
		public bool includeHead;
		public triangleVetrices vertices;
	}

	private static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	private static void Init(int amountOfSegments, int positionRotationLegnth) 
	{
		Array.Resize(ref indices, 4 * positionRotationLegnth);
		Array.Resize(ref segments, amountOfSegments);

	}
	public static void NewQuad(Vector2 position, Color color, Vector2 size) 
	{
		Init(1, 4);
		segments[0].rotation = 0;
		segments[0].color = color;
		segments[0].setSeg = position;
		segments[0].size = size / 2;
		InitIndices();

		Draw();
	}

	public static void NewStrip(Vector2[] pos, float[] rot, Color col, Vector2[] size, Vector2 offset) 
	{
		
		Init(pos.Length, pos.Length);
		for (int i = 0; i < pos.Length - 2; i++) 
		{
			segments[i].rotation = MathHelper.WrapAngle(rot[i] - MathHelper.Pi);
			segments[i].size = size[i];
			segments[i].color = col;
			segments[i].flip = i % 2 == 0;
			segments[i].setSeg = pos[i] + offset;
		}
	
		InitIndices(); 
		
		Draw();

	}

	private static void InitIndices() {

		for(int i = 0; i < indices.Length - 1; i++) 
		{

			indices[4 * i] = (short)(i * 2);
			indices[4 * i + 1] = (short)(i * 2 + 1);
			indices[4* i + 2] = (short)(i * 2 + 2);
			indices[4 * i + 3] = (short)(i * 2 + 2);



			
		}

	}

	private static void Draw() 
	{
		CustomVertex[] vertices = new CustomVertex[segments.Length * 4];

		for (int i = 0; i < segments.Length; i++) {
			for (int j = 0; j < 4; j++) {

				var index = i * 4 + j;
				vertices[index] = segments[i].vertices[j];
				vertices[index].TexCoord = new Vector2(index * 2f / vertices.Length, index % 2 == 0 ? 1 : 0);
			}



		}

		//GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, segments.Length * 4);
		GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length, indices, 0, segments.Length * 4);


		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
}
