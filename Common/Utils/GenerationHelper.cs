﻿using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.WorldGenOverhaul;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Common.Systems.Achievement;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using BossRush.Common.RoguelikeChange.Prefixes;

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

	public static void FastPlaceTile(int i, int j, ushort tileType, byte color) {
		if (CoordinatesOutOfBounds(i, j) || tileType < 0 || tileType >= TileID.Count) {
			return;
		}

		Tile tile = Main.tile[i, j];
		tile.TileType = tileType;
		tile.TileColor = color;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}
	public static void FastPlaceTile(int i, int j, int tileType) {
		FastPlaceTile(i, j, (ushort)tileType);
	}

	public static void FastRemoveTile(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}
		Main.tile[i, j].ClearTile();
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

	public static void PlaceStructure(string FileName, Rectangle rect, GenerateStyle style = GenerateStyle.None) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		switch (style) {
			case GenerateStyle.None:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTile(holdX, holdY, gdata.tileData);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipHorizon:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTile(holdX, holdY, gdata.tileData);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipVertical:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTile(holdX, holdY, gdata.tileData);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipBoth:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTile(holdX, holdY, gdata.tileData);
						offsetY++;
					}
				}
				break;
		}
	}
	/// <summary>
	/// Use this to place the structure in world gen code
	/// </summary>
	/// <param name="method">the method that was uses to optimize the file</param>
	public static void PlaceStructure(string FileName, Rectangle rect, Action<int, int, Action> tileGen, GenerateStyle style = GenerateStyle.None) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		switch (style) {
			case GenerateStyle.None:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, () => Structure_PlaceTile(holdX, holdY, gdata.tileData));
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipHorizon:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, () => Structure_PlaceTile(holdX, holdY, gdata.tileData));
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipVertical:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, () => Structure_PlaceTile(holdX, holdY, gdata.tileData));
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipBoth:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, () => Structure_PlaceTile(holdX, holdY, gdata.tileData));
						offsetY++;
					}
				}
				break;
		}
	}
	/// <summary>
	/// Use this to place the structure in world gen code but attempt to override tile data before placing
	/// </summary>
	/// <param name="method">the method that was uses to optimize the file</param>
	public static void PlaceStructure(string FileName, Rectangle rect, Action<int, int, TileData> tileGen, GenerateStyle style = GenerateStyle.None) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		switch (style) {
			case GenerateStyle.None:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						TileData data = gdata.tileData;
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, data);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipHorizon:
				for (int i = 0; i < datalist.Count; i++) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						TileData data = gdata.tileData;
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, data);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipVertical:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = 0; l < gdata.Count; l++) {
						TileData data = gdata.tileData;
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, data);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipBoth:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					GenPassData gdata = datalist[i];
					for (int l = gdata.Count; l > 0; l--) {
						TileData data = gdata.tileData;
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						tileGen(holdX, holdY, data);
						offsetY++;
					}
				}
				break;
		}
	}
	public static void Structure_PlaceTile(int holdX, int holdY, TileData data) {
		Tile tile = Main.tile[holdX, holdY];
		if (!data.Tile_Air) {
			data.PlaceTile(tile);
		}
		else {
			FastRemoveTile(holdX, holdY);
			tile.WallType = data.Tile_WallData;
		}
	}
	/// <summary>
	/// Offer slightly slower structure placing, but ensure safety<br/>
	/// Use this to place the structure in world gen code
	/// </summary>
	/// <param name="method">the method that was uses to optimize the file</param>
	public static void Safe_PlaceStructure(string FileName, Rectangle rect) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		for (int i = 0; i < datalist.Count; i++) {
			GenPassData gdata = datalist[i];
			for (int l = 0; l < gdata.Count; l++) {
				if (offsetY >= rect.Height) {
					offsetY = 0;
					offsetX++;
				}
				holdX = X + offsetX; holdY = Y + offsetY;
				if (WorldGen.InWorld(holdX, holdY)) {
					Tile tile = Main.tile[holdX, holdY];
					TileData data = gdata.tileData;
					if (!data.Tile_Air) {
						data.PlaceTile(tile);
					}
					else {
						FastRemoveTile(holdX, holdY);
						tile.WallType = data.Tile_WallData;
					}
				}
				offsetY++;
			}
		}
	}
	/// <summary>
	/// Saves a given region of the world as a structure file
	/// </summary>
	/// <param name="target">The region of the world to save, in tile coordinates</param>
	/// <param name="targetPath">The name of the file to save. Automatically defaults to a file named after the date in the SavedStructures folder</param>
	public static void SaveToFile(Rectangle target, string name = "unnamed structure", SaverOptimizedMethod method = SaverOptimizedMethod.Default) {
		if (name == "")
			name = "unnamed structure";

		string path = Path.Join(Program.SavePathShared, "RogueLikeData");

		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
		if (method == SaverOptimizedMethod.Default) {
			SaveStructure(target, path, name);
		}
		else if (method == SaverOptimizedMethod.Template) {
			SaveTemplateStructure(target, path, name);
		}
		Main.NewText("Structure saved as " + Path.Combine(path, name), Color.Yellow);
	}
	/// <summary>
	/// Attempt to save a structure into a file
	/// </summary>
	/// <param name="target">The region to transform</param>
	/// <param name="path">Path to save</param>
	/// <param name="name">File's name</param>
	public static void SaveStructure(Rectangle target, string path, string name) {
		try {
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);

			Tile outSideLoop = new();
			outSideLoop.TileType = ushort.MaxValue;
			int distance = 0;
			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.TileType != outSideLoop.TileType || tile.TileFrameX != outSideLoop.TileFrameX && tile.TileType >= TileID.Count) {
						if (distance != 0) {
							m.Write(distance);
						}
						outSideLoop = tile;
						TileData td = new(tile);
						m.Write(td.ToString());
						distance = 1;
					}
					else {
						distance++;
					}
				}
			}
			if (distance != 0) {
				m.Write(distance);
			}
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	/// <summary>
	/// Attempt to save a structure into a file
	/// </summary>
	/// <param name="target">The region to transform</param>
	/// <param name="path">Path to save</param>
	/// <param name="name">File's name</param>
	public static void SaveTemplateStructure(Rectangle target, string path, string name) {
		try {
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);

			Tile outSideLoop = new();
			outSideLoop.TileType = ushort.MaxValue;
			int distance = 0;
			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.TileType != outSideLoop.TileType || tile.TileFrameX != outSideLoop.TileFrameX && tile.TileType >= TileID.Count) {
						if (distance != 0) {
							m.Write(distance);
						}
						outSideLoop = tile;
						TileData td = new(tile);
						if (td.Tile_Type != 0) {
							td.Tile_Type = TileID.Dirt;
						}
						if (td.Tile_WallData != 0) {
							td.Tile_WallData = WallID.Stone;
						}
						m.Write(td.ToString());
						distance = 1;
					}
					else {
						distance++;
					}
				}
			}
			if (distance != 0) {
				m.Write(distance);
			}
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	public static void Detailed_SaveStructure(Rectangle target, string path, string name) {
		try {
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);

			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					TileData td = new(tile);
					m.Write(td.ToString());
				}
			}
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	/// <summary>
	/// Attempt to save many structure into a file with rectangle format<br/>
	/// Be aware as this do not check for any error
	/// </summary>
	/// <param name="listtarget">The region to transform</param>
	/// <param name="path">Path to save</param>
	/// <param name="name">File's name</param>
	public static void SaveRectStructure(Point16 startingPoint, List<Rectangle> listtarget, string path, string name) {
		try {
			int StartX = startingPoint.X, StartY = startingPoint.Y;
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);
			foreach (Rectangle target in listtarget) {
				Tile tile = Framing.GetTileSafely(StartX, StartY);
				TileData td = new(tile);
				m.Write(td.ToString());
				m.Write(CustomRecSavingtFormat(target));
			}
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	public static string CustomRecSavingtFormat(Rectangle rect) {
		return $"A{rect.Width}B{rect.Height}";
	}
}
public enum GenerateStyle : byte {
	None,
	FlipHorizon,
	FlipVertical,
	FlipBoth
}
/// <summary>
/// TODO : Implement the below optimize saving method
/// All of these act differently so it is as expected
/// </summary>
public enum SaverOptimizedMethod : byte {
	/// <summary>
	/// The default optimization, it is highly recommend to use this method when you are saving a small building
	/// </summary>
	Default,
	/// <summary>
	/// The default optimization but instead horizontal
	/// </summary>
	HorizontalDefault,
	/// <summary>
	/// This optimization method will save the following structure in rectangle-like<br/>
	/// useful for wanting to save a massive arena structure that have a lot of open space<br/>
	/// Extremely not recommend for massive structure that is very complex
	/// </summary>
	MultiStructure,
	/// <summary>
	/// Default optimization but seperate wall and tile into 2 different field
	/// </summary>
	WallTileSeperate,
	/// <summary>
	/// This is for template kind, which will choose the most less consuming tile data and save it
	/// </summary>
	Template
}
public struct TileData : ICloneable {
	public ushort Tile_Type = 0;
	public short Tile_FrameX = 0;
	public short Tile_FrameY = 0;
	public ushort Tile_WallData = 0;
	public byte Tile_WireData = 0;
	public byte Tile_LiquidData = byte.MaxValue;
	public byte Tile_Liquid = 0;
	public bool Tile_HasActuator = false;
	public bool Tile_Air = false;
	public static TileData Default => new();
	public TileData() {
		Tile_Type = 0;
		Tile_FrameX = 0;
		Tile_FrameY = 0;
		Tile_WallData = 0;
		Tile_WireData = 0;
		Tile_LiquidData = byte.MaxValue;
		Tile_Liquid = 0;
		Tile_HasActuator = false;
		Tile_Air = false;
	}

	public TileData(ushort tileType, short tileFrameX, short tileFrameY, ushort tile_WallData, byte tile_WireData, bool tile_HasActuator) {
		Tile_Type = tileType;
		Tile_FrameX = tileFrameX;
		Tile_FrameY = tileFrameY;
		Tile_WallData = tile_WallData;
		Tile_WireData = tile_WireData;
		Tile_HasActuator = tile_HasActuator;
	}
	public TileData(Tile tile) {
		if (tile.Get<TileWallWireStateData>().HasTile) {
			Tile_Type = tile.TileType;
			if (tile.TileFrameX != -1) {
				Tile_FrameX = tile.TileFrameX;
			}
			if (tile.TileFrameY != -1) {
				Tile_FrameY = tile.TileFrameY;
			}
		}
		else {
			Tile_Air = true;
		}
		if (tile.WallType != 0) {
			Tile_WallData = tile.WallType;
		}
		if (tile.RedWire) {
			Tile_WireData = 1;
		}
		if (tile.BlueWire) {
			Tile_WireData = 2;
		}
		if (tile.YellowWire) {
			Tile_WireData = 3;
		}
		if (tile.GreenWire) {
			Tile_WireData = 4;
		}
		if (tile.LiquidAmount != 0) {
			Tile_Liquid = (byte)tile.LiquidType;
			Tile_LiquidData = tile.LiquidAmount;
		}
	}
	/// <summary>
	/// Reverse the ToString back into a tile format
	/// </summary>
	/// <param name="Tile"></param>
	public TileData(string Tile) {
		string NumberString = "";
		char c = ' ';
		for (int i = 0; i < Tile.Length; i++) {
			if (char.IsNumber(Tile[i])) {
				NumberString += Tile[i];
			}
			else {
				if (c != ' ') {

					ParseStringData(c, NumberString);
					NumberString = "";

				}
				c = Tile[i];
			}
		}
		if (c != ' ')
			ParseStringData(c, NumberString);
	}

	private void ParseStringData(char c, string str) {
		switch (c) {
			case 'T':
				Tile_Type = ushort.Parse(str); break;
			case 'X':
				Tile_FrameX = short.Parse(str); break;
			case 'Y':
				Tile_FrameY = short.Parse(str); break;
			case 'W':
				Tile_WallData = ushort.Parse(str); break;
			case 'L':
				Tile_Liquid = byte.Parse(str); break;
			case 'D':
				Tile_LiquidData = byte.Parse(str); break;
			case 'A':
				Tile_HasActuator = true; break;
			case 'N':
				Tile_Air = true; break;
		}
	}
	/// <summary>
	/// Use this method during world gen is advised<br/>
	/// But only if there are actually anything implemented into this method
	/// </summary>
	public void PlaceTile(Tile tile) {
		tile.TileType = Tile_Type;
		tile.TileFrameX = Tile_FrameX;
		tile.TileFrameY = Tile_FrameY;
		tile.WallType = Tile_WallData;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}
	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		if (Tile_Type < TileID.Count && !Tile_Air) {
			sb.Append("T").Append(Tile_Type);
			sb.Append("X").Append(Tile_FrameX);
			sb.Append("Y").Append(Tile_FrameY);
		}
		else {
			sb.Append("N");
		}
		if (Tile_WallData < WallID.Count) {
			sb.Append("W").Append(Tile_WallData);
		}
		if (Tile_LiquidData < LiquidID.Count && Tile_Liquid > 0) {
			sb.Append("L").Append(Tile_Liquid);
			sb.Append("D").Append(Tile_LiquidData);
		}
		if (Tile_HasActuator) {
			sb.Append("A");
		}
		return "{" + sb.ToString() + "}";
	}

	public object Clone() {
		return this.MemberwiseClone();
	}
	public override bool Equals([NotNullWhen(true)] object obj) {
		var TD = (TileData)obj;
		return TD.Tile_Type == this.Tile_Type
			&& TD.Tile_Liquid == this.Tile_Liquid
			&& TD.Tile_LiquidData == this.Tile_LiquidData
			&& TD.Tile_Air == this.Tile_Air
			&& TD.Tile_FrameX == this.Tile_FrameX
			&& TD.Tile_FrameY == this.Tile_FrameY
			&& TD.Tile_HasActuator == this.Tile_HasActuator
			&& TD.Tile_WallData == this.Tile_WallData
			&& TD.Tile_WireData == this.Tile_WireData;
	}
}
