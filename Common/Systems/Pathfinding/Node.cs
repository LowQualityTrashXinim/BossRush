using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Terraria;

namespace BossRush.Common.Systems.Pathfinding;
public class Node {

		public Point arraySlot = new Point();
		public Vector2 position = new Vector2();
		public Node parent = null;
		public bool isPath = false;
		public bool walkable = false;
		public float g = 0;
		public float h = 0;
		public float f => g + h;
		

	}
