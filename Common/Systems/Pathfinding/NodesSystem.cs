using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Common.Systems.Pathfinding;
public class NodesSystem : ModSystem {

	public static Node[,] nodes;
	private static int activeRangeX = (int)((double)1920 * 2.1);
	private static int activeRangeY = (int)((double)1080 * 2.1);
	public override void OnWorldLoad() {

		nodes = new Node[Main.maxTilesX, Main.maxTilesY];

		for (int i = 0; i < Main.maxTilesX; i++) {
			for (int j = 0; j < Main.maxTilesY; j++) {

				nodes[i,j] = new Node();
				nodes[i, j].position = Terraria.Utils.ToWorldCoordinates(new Point(i, j));
				nodes[i, j].arraySlot = new Point(i, j);
			}

		}
	}

	public static readonly List<Point> Dirs = new List<Point>()
		{
			new Point(0, 1), new Point(-1, 0), new Point(0, -1), new Point(1, 0),
			new Point(1, 1), new Point(1, -1), new Point(-1, -1), new Point(-1, 1)
		};


	public static HashSet<Node> GetNeighbors(Point nodeAt) {
		var neighbors = new HashSet<Node>();


		foreach (var dir in Dirs) {
				Point clampedPoint = new Point((int)MathHelper.Clamp(nodeAt.X + dir.X,0,Main.maxTilesX - 1),(int)MathHelper.Clamp(nodeAt.Y + dir.Y,0,Main.maxTilesY - 1));

		
				var node = nodes[clampedPoint.X, clampedPoint.Y];

				if (node != null)
					neighbors.Add(node);
			


		}


		return neighbors;

	}


	public static float GetDistance(Point firstNode, Point otherNode) {

		var dist = new Vector2(Math.Abs(firstNode.X - otherNode.X), Math.Abs(firstNode.Y - otherNode.Y));

		var lowest = MathF.Min(dist.X, dist.Y);
		var highest = MathF.Max(dist.X, dist.Y);

		var horizontalMovesRequired = highest - lowest;

		return lowest * 1.4f + horizontalMovesRequired;
	}



	public override void PreUpdateEntities() {

		base.PreUpdateEntities();


		foreach(Player p in Main.ActivePlayers) {

			ActivateNodesAroundPosition(p.position);

		}

		if(debugNodes)
		{
			for(int i = 0; i<25; i++)
			debugList = Astar.getPath(Main.LocalPlayer.Center,Main.MouseWorld);

		}

	}
	HashSet<Vector2> debugList = new();
	public static void ActivateNodesAroundPosition(Vector2 position) {
		Node centerNode = nodes[position.ToTileCoordinates().X, position.ToTileCoordinates().Y];

		Shapes.Rectangle rectangle = new Shapes.Rectangle(activeRangeX / 16,activeRangeY / 16);

		WorldUtils.Gen(new Point(centerNode.arraySlot.X - activeRangeX / 16 / 2, centerNode.arraySlot.Y - activeRangeY / 16 / 2),rectangle,new Actions.Custom((x,y,args) => 
		{
			Point clampedPoint = new Point((int)MathHelper.Clamp(x,0,Main.maxTilesX - 1),(int)MathHelper.Clamp(y,0,Main.maxTilesY - 1));
			if(!WorldGen.SolidTile(clampedPoint.X,clampedPoint.Y))
			{
				nodes[clampedPoint.X,clampedPoint.Y].walkable = true;

			}
			else
			{
				nodes[clampedPoint.X,clampedPoint.Y].walkable = false;

			}

			return true;
		}));

	}
	public static bool debugNodes = false;
	public override void PostDrawTiles() {
		
		Main.spriteBatch.Begin();

		if(debugNodes && Main.LocalPlayer.active)
		{


			foreach(Vector2 n in debugList)
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(BossRushTexture.MissingTexture_Default).Value,n - Main.screenPosition,Color.White);
			
			
		} 

		Main.spriteBatch.End();

	}

}
