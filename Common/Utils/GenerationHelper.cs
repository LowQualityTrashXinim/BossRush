using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.WorldGenOverhaul;

namespace BossRush.Common.Utils;

internal static partial class GenerationHelper {
	public static bool CoordinatesOutOfBounds(int i, int j) => i >= Main.maxTilesX || j >= Main.maxTilesY || i < 0 || j < 0;

	public static void FastPlaceTile(int i, int j, ushort tileType) {
		if (CoordinatesOutOfBounds(i, j) || tileType < 0 || tileType >= TileID.Count) {
			return;
		}

		Tile tile = Main.tile[i, j];
		tile.TileType = tileType;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}

	public static void FastPlaceTile(int i, int j, int tileType) {
		FastPlaceTile(i, j, (ushort)tileType);
	}

	public static void FastRemoveTile(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].Get<TileWallWireStateData>().HasTile = false;
	}

	public static void FastPlaceWall(int i, int j, int wallType) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = (ushort)wallType;
	}

	public static void FastRemoveWall(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = WallID.None;
	}

	public static void ForEachInRectangle(Rectangle rectangle, Action<int, int> action) {
		for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++) {
			for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++) {
				action(i, j);
			}
		}
	}

	public static int PointOnRectX(this Rectangle rect, float scale) => (int)(rect.X + rect.Width * scale);
	public static int PointOnRectY(this Rectangle rect, float scale) => (int)(rect.Y + rect.Height * scale);
	public static void ForEachInRectangle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(new Rectangle(i, j, width, height), action);
	}

	public static void ForEachInCircle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(
			i - width / 2,
			j - height / 2,
			width,
			height,
			(iLocal, jLocal) => {
				if (MathF.Pow((iLocal - i) / (width * 0.5f), 2) + MathF.Pow((jLocal - j) / (height * 0.5f), 2) - 1 >= 0) {
					return;
				}

				action(iLocal, jLocal);
			}
		);
	}
	public static Point RandomizePossibleGridSelection(this UnifiedRandom r, HashSet<Point> selectionList, int limitX = 24, int limitY = 24) {
		if (selectionList.Count < 1) {
			return new Point(-1, -1);
		}
		Point currentPos = selectionList.ElementAt(selectionList.Count - 1);
		if (selectionList.Count == 1) {
			currentPos.X += r.NextBool().ToDirectionInt();
			currentPos.Y += r.NextBool().ToDirectionInt();
			return currentPos;
		}
		HashSet<Point> getallPosition = new HashSet<Point>();
		if (!selectionList.Contains(currentPos + new Point(1, 0)) && currentPos.X > 0 && currentPos.X < limitX) {
			getallPosition.Add(currentPos + new Point(1, 0));
		}
		if (!selectionList.Contains(currentPos - new Point(1, 0)) && currentPos.X > 0 && currentPos.X < limitX) {
			getallPosition.Add(currentPos - new Point(1, 0));
		}
		if (!selectionList.Contains(currentPos + new Point(1, 0)) && currentPos.Y > 0 && currentPos.Y < limitY) {
			getallPosition.Add(currentPos + new Point(0, -1));
		}
		if (!selectionList.Contains(currentPos - new Point(1, 0)) && currentPos.Y > 0 && currentPos.Y < limitY) {
			getallPosition.Add(currentPos - new Point(1, -1));
		}
		if (getallPosition.Count < 1) {
			return new Point(-1, -1);
		}
		else if (getallPosition.Count == 1) {
			return getallPosition.ElementAt(0);
		}
		return r.NextFromHashSet(getallPosition);
	}
	/// <summary>
	/// Use this for easy place tile in the world in 24x24 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Select multiple grid on X axis</param>
	/// <param name="dragY">Select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld24x24(int x, int y, int dragX = 1, int dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X * x, RogueLikeWorldGen.GridPart_Y * y, RogueLikeWorldGen.GridPart_X * dragX, RogueLikeWorldGen.GridPart_Y * dragY);
	/// <summary>
	/// Use this for easy place tile in the world in 48x48 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Select multiple grid on X axis</param>
	/// <param name="dragY">Select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld48x48(int x, int y, int dragX = 1, int dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X / 2 * x, RogueLikeWorldGen.GridPart_Y / 2 * y, RogueLikeWorldGen.GridPart_X / 2 * dragX, RogueLikeWorldGen.GridPart_Y / 2 * dragY);
	public static float ProgressOnAStrip(int minY, int maxY, int currentY) {
		return MathHelper.Lerp(minY, maxY, currentY);
	}
	public static void ForEachInCircle(int i, int j, int radius, Action<int, int> action) {
		ForEachInCircle(i, j, radius * 2, radius * 2, action);
	}
	public static void PlaceStructure(int startingX, int startingY, int currentX, int currentY, StructureData structureData) {
		if(startingX > currentX || startingY > currentY) {
			return;
		}
		if (currentX - startingX >= structureData.data[0].Length) {
			return;
		}
		if (currentY - startingY >= structureData.data.Length) {
			return;
		}
		if(structureData.data[currentY - startingY][currentX - startingX] >= TileID.Count) {
			FastRemoveTile(currentX, currentY);
		}
		FastPlaceTile(currentX, currentY, structureData.data[currentY - startingY][currentX - startingX]);
	}
}
/// <summary>
/// It is extremely not recommended to uses this if you gonna make the same kind of world gen
/// This is very much hardcoded
/// </summary>
public class StructureData {
	public int Type;
	/// <summary>
	/// Beaware that when making this, the tile placement will be rotated by 90 degree clockwise
	/// </summary>
	public ushort[][] data;
	public StructureData() {
	}
	public StructureData(int type, ushort[][] Data) {
		Type = type;
		data = Data;
	}
}
