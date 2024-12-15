using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Common.Graphics.Primitives;
public sealed class PrimitivesDrawer : ModSystem {



	[InlineArray(6)]
	private struct segementVertices {
		private VertexPositionColor element0;
	}

	private static short[] indices = new short[1];
	private static SegementAttributes[] segments;
	private static float[] rotationsCache;
	private static Vector2[] positionsCache;
	private const short aSingleSegementCorners = 6;
	private static short amountOfVerticesContainednotIncludingHeadTail;
	private static VertexPositionColor[] verticesCache;
	public static BasicEffect basicEffect;



	/// <summary>
	/// a segment, contains 4/5/6 (depends in head/tail) vertices, creating a Parallelogram shape
	/// </summary>
	private struct SegementAttributes {
		public Vector2 setSeg {
			set {
				var center = value;
				segementVertices segementVertices = new segementVertices();
				segementVertices[0].Color = color;
				segementVertices[0].Position = new Vector3(center.X - width, center.Y - height,0);
				segementVertices[1].Color = color;
				segementVertices[1].Position = new Vector3(center.X - width , center.Y + height, 0);
				segementVertices[2].Color = color;
				segementVertices[2].Position = new Vector3(center.X + width, center.Y - height, 0);
				segementVertices[3].Color = color;
				segementVertices[3].Position = new Vector3(center.X + width, center.Y + height, 0);

				if (head) 
				{
					segementVertices[4].Color = color;
					segementVertices[4].Position = new Vector3(center.X - width / 2, center.Y - height, 0);
				}
				if (tail) 
				{
					segementVertices[5].Color = color;
					segementVertices[5].Position = new Vector3(center.X + width / 2, center.Y + height, 0);

				}

				vertices = segementVertices;

			}
		}
		public float rotation;
		public float width;
		public float height;
		public Color color;
		public bool tail;
		public bool head;
		public segementVertices vertices;


	}

	private static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	private static void init(int amountOfSegments, int positionRotationLegnth) 
	{
		Array.Resize<short>(ref indices, 6 * positionRotationLegnth);
		Array.Resize<SegementAttributes>(ref segments, amountOfSegments);
		Array.Resize<float>(ref rotationsCache, positionRotationLegnth);
		Array.Resize<Vector2>(ref positionsCache, positionRotationLegnth);
		amountOfVerticesContainednotIncludingHeadTail = (short)((amountOfSegments * 6) * 2);

	}

	public static void newQuad(Vector2 position, Color color, float size) 
	{
		init(1, 4);
		segments[0].rotation = 0;
		segments[0].width = size / 2;
		segments[0].height = size / 2;
		segments[0].color = color;
		segments[0].tail = true;
		segments[0].head = true;
		segments[0].setSeg = position;


		initIndices();

		draw();
	}

	public static void newStrip(Vector2[] pos, float[] rot, Color col, float[] width, Vector2 offset) 
	{

		init(pos.Length, pos.Length);
		for (int i = 0; i < pos.Length - 2; i++) 
		{
			segments[i].rotation = 0;
			segments[i].width = Vector2.Distance(pos[i], pos[i+1]);
			segments[i].height = width[i];
			segments[i].color = col;
			segments[i].tail = false;

			if(i == 0)
				segments[i].head = true;
			else
				segments[i].head = false;

			if (i == pos.Length - 3)
				segments[i].tail = true;
			else
				segments[i].tail = false;

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
		// + 6 means that it must have a tail and a head
		VertexPositionColor[] vertices = new VertexPositionColor[segments.Length * 4 + 2];
		for (int i = 0; i < segments.Length; i++) 
		{
			for (int j = 0; j < 4; j++)
			{
				vertices[i * 4 + j + 1] = segments[i].vertices[j];
			}

			if (segments[i].head) 
			{
				vertices[0] = segments[i].vertices[4];
			}

			if (segments[i].tail) {
				vertices[segments.Length * 4 + 2 - 1] = segments[i].vertices[5];
			}
		}


		int primtiveAmount = (int)(amountOfVerticesContainednotIncludingHeadTail / 4 + 2);



		GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
		basicEffect.CurrentTechnique.Passes[0].Apply();
		var viewport = GraphicsDevice.Viewport;
		basicEffect.World = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0));
		basicEffect.View = Main.GameViewMatrix.TransformationMatrix;
		basicEffect.Projection = Matrix.CreateOrthographicOffCenter(left: 0, right: viewport.Width, bottom: viewport.Height, top: 0, zNearPlane: -1, zFarPlane: 10);
		//GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, primtiveAmount);
		GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length, indices, 0, primtiveAmount);


		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}

	public override void Load() {
		Main.RunOnMainThread(() => {


			basicEffect = new BasicEffect(GraphicsDevice);
			basicEffect.VertexColorEnabled = true;


		}).Wait();
	}

	public override void Unload() {
		Main.RunOnMainThread(() => {


			basicEffect?.Dispose();
		}).Wait();
		basicEffect = null;

	}
}
