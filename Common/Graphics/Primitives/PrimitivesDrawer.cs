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

	[InlineArray(6)]
	private struct segementVertices {
		private CustomVertex element0;
	}

	private static short[] indices = new short[1];
	private static SegementAttributes[] segments;
	private const short aSingleSegementCorners = 6;
	private static short amountOfVerticesContainednotIncludingHeadTail;

	private struct SegementAttributes {
		public Vector2 setSeg {
			set {
				var center = value;
				segementVertices segementVertices = new segementVertices();
				Vector2 vertex0 = new Vector2(center.X - size.X, center.Y - size.Y);
				Vector2 vertex1 = new Vector2(center.X - size.X, center.Y + size.Y);
				Vector2 vertex2 = new Vector2(center.X + size.X, center.Y - size.Y);
				Vector2 vertex3 = new Vector2(center.X + size.X, center.Y + size.Y);
				Vector2 vertex4 = new Vector2(center.X - size.X / 2, center.Y - size.Y);
				segementVertices[0].Color = color;
				segementVertices[0].Position = vertex0.RotatedBy(rotation,center);
				segementVertices[1].Color = color;
				segementVertices[1].Position = vertex1.RotatedBy(rotation, center);
				segementVertices[2].Color = color;
				segementVertices[2].Position = vertex2.RotatedBy(rotation, center); ;
				segementVertices[3].Color = color;
				segementVertices[3].Position = vertex3.RotatedBy(rotation, center); ;

				if (head) 
				{
					segementVertices[4].Color = color;
					segementVertices[4].Position = vertex4.RotatedBy(rotation, center);
				}

				vertices = segementVertices;

			}
		}
		public float rotation;
		public Vector2 size;
		public Color color;
		public bool head;
		public segementVertices vertices;
	}

	private static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	private static void init(int amountOfSegments, int positionRotationLegnth) 
	{
		Array.Resize<short>(ref indices, 6 * positionRotationLegnth);
		Array.Resize<SegementAttributes>(ref segments, amountOfSegments);
		amountOfVerticesContainednotIncludingHeadTail = (short)((amountOfSegments * 6) * 2);

	}
	public static void newQuad(Vector2 position, Color color, Vector2 size) 
	{
		init(1, 4);
		segments[0].rotation = 0;
		segments[0].color = color;
		segments[0].head = true;
		segments[0].setSeg = position;
		segments[0].size = size;

		initIndices();

		draw();
	}

	public static void newStrip(Vector2[] pos, float[] rot, Color col, Vector2[] size, Vector2 offset) 
	{
		
		init(pos.Length, pos.Length);
		for (int i = 0; i < pos.Length - 2; i++) 
		{
		
			segments[i].rotation = MathHelper.WrapAngle(rot[i] - MathHelper.Pi);
			segments[i].size = size[i];
			segments[i].color = col;

			if(i == 0)
				segments[i].head = true;
			else
				segments[i].head = false;


			segments[i].setSeg = pos[i] + offset;
		}
	
		initIndices(); 
		
		draw();

	}

	private static void initIndices() 
	{
		
		for(short i = 0; i < indices.Length - 1; i = (short)(i + 1)) 
		{

			indices[i] = i;

		}


	}

	private static void draw() 
	{
		// + 1 means that it must have a tail and a head
		CustomVertex[] vertices = new CustomVertex[segments.Length * 4 + 1];

		for (int i = 0; i < segments.Length; i++) 
		{
			for (int j = 0; j < 4; j++)
			{
			
				var index = i * 4 + j + 1;
				vertices[index] = segments[i].vertices[j];
				vertices[index].TexCoord = new Vector2(index*2f/vertices.Length, index % 2 == 0 ? 1 : 0);
			}

			if (segments[i].head) 
			{
				vertices[0] = segments[i].vertices[4];
				vertices[0].TexCoord = new Vector2(1, 1);
			}
		
		}


		GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, amountOfVerticesContainednotIncludingHeadTail / 4);
		//GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length, indices, 0, primtiveAmount);


		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
}
