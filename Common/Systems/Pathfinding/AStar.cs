using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Terraria;


namespace BossRush.Common.Systems.Pathfinding {
	public static class Astar {
		public static HashSet<Vector2> getPath(Vector2 startingPosition, Vector2 targetPostion) {
			var startingNode = NodesSystem.nodes[startingPosition.ToTileCoordinates().X,startingPosition.ToTileCoordinates().Y];
			var targetNode = NodesSystem.nodes[targetPostion.ToTileCoordinates().X,targetPostion.ToTileCoordinates().Y];
			var openNodes = new HashSet<Node>() { startingNode };
			var closedNodes = new HashSet<Node>();
			var frontier = new PriorityQueue<Node, float>();
			frontier.Enqueue(startingNode, 0);
			var path = new HashSet<Vector2>();
			int counter = 0;
			while (openNodes.Count != 0) {
				counter++;

				if (counter == 2040)
					break;
				var current = frontier.Dequeue();

				openNodes.Remove(current);
				closedNodes.Add(current);


				if (current == targetNode) {
					var currentPathNode = targetNode;

					while (currentPathNode != startingNode) {
						currentPathNode.isPath = true;
						path.Add(currentPathNode.position);
						currentPathNode = currentPathNode.parent;

					}

					
					return path;

				}

				foreach (var neighbor in NodesSystem.GetNeighbors(current.arraySlot).Where(n => !closedNodes.Contains(n) && n.walkable)) {
					var isOpened = openNodes.Contains(neighbor);

					var costToNeighbor = current.g + NodesSystem.GetDistance(current.arraySlot,neighbor.arraySlot);

					if (!isOpened || costToNeighbor < neighbor.g) {
						neighbor.g = costToNeighbor;
						neighbor.parent = current;

						if (!isOpened) {

							neighbor.h = NodesSystem.GetDistance(neighbor.arraySlot,targetNode.arraySlot);
							openNodes.Add(neighbor);
							frontier.Enqueue(neighbor, costToNeighbor + NodesSystem.GetDistance(neighbor.arraySlot,targetNode.arraySlot));
						}
					}
				}

			}
			return path;
		}


	}
}
