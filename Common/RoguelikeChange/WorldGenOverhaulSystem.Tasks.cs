//using BossRush.Common.Utils;
//using Microsoft.Xna.Framework;
//using MonoMod.Logs;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using Terraria;
//using Terraria.ID;
//using Terraria.IO;
//using Terraria.ModLoader;
//using Terraria.WorldBuilding;

//namespace BossRush.Common.WorldGenOverhaul {
//	internal partial class WorldGenOverhaulSystem : ITaskCollection {
//		[Task]
//		public void Setup() {
//			Main.worldSurface = 0;
//			Main.rockLayer = 0;
//			Main.spawnTileX = Main.maxTilesX / 2;
//			Main.spawnTileY = Main.maxTilesY / 2;
//		}

//		[Task]
//		public void Terrain() {
//			static float NormalizedSquaredDistanceFromCenter(int i, int j) {
//				Point center = new(Main.maxTilesX / 2, Main.maxTilesY / 2);
//				return (MathF.Pow(i - center.X, 2) + MathF.Pow(j - center.Y, 2)) / MathF.Pow(Main.maxTilesX / 2f, 2);
//			}

//			GenerationHelper.PerlinNoise2D smallNoise = new(WorldGen.genRand.Next());
//			GenerationHelper.PerlinNoise2D bigNoise = new(WorldGen.genRand.Next());
//			GenerationHelper.PerlinNoise2D dirtNoise = new(WorldGen.genRand.Next());
//			GenerationHelper.PerlinNoise2D dirtNoiseThickness = new(WorldGen.genRand.Next());

//			DomeRadius = (int)(Main.maxTilesX * 0.005f);

//			GenerationHelper.ForEachInRectangle(
//				0,
//				0,
//				Main.maxTilesX,
//				Main.maxTilesY,
//				(i, j) => {
//					// Generates basic caves and the dome in the middle.
//					float domeSize = 0.005f;
//					float distance = NormalizedSquaredDistanceFromCenter(i, j);
//					float jFloor = (Main.maxTilesY / 2f + 3f * MathF.Sin(i * 0.025f) * MathF.Cos(i * 0.065f + 0.354135f));
//					if (
//						j > jFloor && j < jFloor + 100 * WorldGen.genRand.NextFloat(0.95f, 1f)
//						&& distance < WorldGen.genRand.NextFloat(domeSize, domeSize + 0.0005f)
//						) {
//						GenerationHelper.FastPlaceTile(
//							i,
//							j,
//							j <= (int)(jFloor + WorldGen.genRand.NextFloat(1f, 3f)) ? TileID.Grass : TileID.Dirt
//						);
//						return;
//					}

//					if (
//						(
//						smallNoise.GetValue(i * 0.15f + MathF.Sin(i * j) * 0.05f, j * 0.15f + MathF.Sin(i * j) * 0.05f) > 0f - 0.4f * MathF.Abs(bigNoise.GetValue(i * 0.008f, j * 0.008f))
//						|| bigNoise.GetValue(i * 0.03f, j * 0.03f) > MathF.Sin((i + j) * 0.1f) * 0.1f + 0.2f
//						)
//						&& (
//							j > jFloor
//							|| distance > WorldGen.genRand.NextFloat(domeSize, domeSize + 0.0005f)
//							)
//						) {
//						GenerationHelper.FastPlaceTile(
//							i,
//							j,
//							dirtNoise.GetValue(i * 0.024f, j * 0.03f) > 0.3f + 0.4 * dirtNoiseThickness.GetValue(i * 0.005f, j * 0.005f)
//								? TileID.Dirt : TileID.Stone
//						);
//					}
//				}
//			);

//			// PLACE BIG CAVES ---------------------------------

//			int bigCaveCount = (int)(Main.maxTilesX * 0.008f);
//			for (int i = 0; i < bigCaveCount; i++) {
//				Point randomPoint = new Point(
//					Main.spawnTileX + WorldGen.genRand.Next((int)(Main.maxTilesX * 0.05f), Main.maxTilesX / 2) * (WorldGen.genRand.NextBool() ? 1 : -1),
//					Main.spawnTileY + WorldGen.genRand.Next((int)(Main.maxTilesY * 0.05f), Main.maxTilesY / 2) * (WorldGen.genRand.NextBool() ? 1 : -1)
//				);

//				int spread = 25;
//				for (int j = 0; j < 9; j++) {
//					GenerationHelper.ForEachInCircle(
//						randomPoint.X + Main.rand.Next(-spread, spread),
//						randomPoint.Y + Main.rand.Next(-spread, spread),
//						WorldGen.genRand.Next(5, 15),
//						(i, j) => {
//							if (Main.rand.NextFloat() < 0.55f) {
//								GenerationHelper.FastRemoveTile(i, j);
//							}
//						}
//					);
//				}
//			}
//		}

//		[Task]
//		public void PlaceWaterCaves() {
//			int waterCaveCount = (int)(Main.maxTilesX * 0.015f);
//			for (int i = 0; i < waterCaveCount; i++) {
//				Point randomPoint = new Point(
//					Main.spawnTileX + WorldGen.genRand.Next((int)(Main.maxTilesX * 0.05f), Main.maxTilesX / 2) * (WorldGen.genRand.NextBool() ? 1 : -1),
//					Main.spawnTileY + WorldGen.genRand.Next((int)(Main.maxTilesY * 0.05f), Main.maxTilesY / 2) * (WorldGen.genRand.NextBool() ? 1 : -1)
//				);

//				int spread = 10;
//				for (int j = 0; j < 8; j++) {
//					GenerationHelper.ForEachInCircle(
//						randomPoint.X + Main.rand.Next(-spread, spread),
//						randomPoint.Y + Main.rand.Next(-spread, spread),
//						WorldGen.genRand.Next(8, 10),
//						(i, j) => {
//							if (Main.rand.NextFloat() < 0.7f) {
//								GenerationHelper.FastRemoveTile(i, j);
//								WorldGen.PlaceLiquid(i, j, (byte)LiquidID.Water, byte.MaxValue);
//							}
//						}
//					);
//				}
//			}
//		}

//		[Task]
//		public void PlaceWalls() {

//		}

//		[Task]
//		public void PlaceJungleTerrain() {
//			JungleWidth = (int)(Main.maxTilesX * 0.1f);
//			JungleHeight = (int)(Main.maxTilesX * 0.1f);
//			JungleTopLeft = new Point(
//				(int)(Main.maxTilesX * (WorldGen.genRand.NextBool() ? WorldGen.genRand.NextFloat(0.7f, 0.8f) : WorldGen.genRand.NextFloat(0.2f, 0.3f)))
//					- JungleWidth / 2,
//				(int)(Main.maxTilesY * WorldGen.genRand.NextFloat(0.45f, 0.65f)) - JungleHeight / 2
//			);

//			GenerationHelper.PerlinNoise2D jungleNoise = new(WorldGen.genRand.Next());

//			GenerationHelper.ForEachInRectangle(
//				JungleTopLeft.X,
//				JungleTopLeft.Y,
//				JungleWidth,
//				JungleHeight,
//				(i, j) => {
//					float iDistance = Math.Abs(JungleTopLeft.X + JungleWidth / 2 - i) / (JungleWidth * 0.5f);
//					float jDistance = Math.Abs(JungleTopLeft.Y + JungleHeight / 2 - j) / (JungleHeight * 0.5f);
//					if (WorldGen.genRand.NextFloat(0.6f, 1f) < iDistance || WorldGen.genRand.NextFloat(0.6f, 1f) < jDistance) {
//						return;
//					}

//					if (!GenerationHelper.CoordinatesOutOfBounds(i, j) && Main.tile[i, j].HasTile) {
//						if (jungleNoise.GetValue(i * 0.06f, j * 0.06f) > 0.4f) {
//							GenerationHelper.FastPlaceTile(i, j, TileID.Stone);
//							return;
//						}

//						GenerationHelper.FastPlaceTile(i, j, TileID.Mud);
//					}
//				}
//			);

//			int bigCaveCount = 8;
//			for (int i = 0; i < bigCaveCount; i++) {
//				Point randomPosition = JungleTopLeft + new Point(
//					WorldGen.genRand.Next(-JungleWidth, JungleWidth) / 3,
//					WorldGen.genRand.Next(-JungleHeight, JungleHeight) / 3
//				);

//				int spread = 30;
//				for (int x = 0; x < 5; x++) {
//					GenerationHelper.ForEachInCircle(
//						randomPosition.X + Main.rand.Next(-spread, spread),
//						randomPosition.Y + Main.rand.Next(-spread, spread),
//						WorldGen.genRand.Next(12, 24),
//						(i, j) => {
//							if (WorldGen.genRand.NextFloat() < 0.40f) {
//								GenerationHelper.FastRemoveTile(i, j);
//							}
//						}
//					);
//				}
//			}
//		}

//		[Task]
//		public void TerrainSmoothie() {
//			for (int i = 0; i < 2; i++) {
//				GenerationHelper.ForEachInRectangle(
//					0,
//					0,
//					Main.maxTilesX,
//					Main.maxTilesY,
//					(i, j) => {
//						if (Main.tile[i, j].TileType == TileID.Grass) {
//							return;
//						}

//						var info = new GenerationHelper.TileNeighbourInfo(i, j).TypeCount(tile => tile.HasTile);
//						if (info.Count > 4) {
//							GenerationHelper.FastPlaceTile(i, j, info.MaxCountType);
//						}
//						else if (info.Count < 4) {
//							GenerationHelper.FastRemoveTile(i, j);
//						}
//					}
//				);
//			}
//		}

//		[Task]
//		public void PlaceGrass() {
//			GenerationHelper.ForEachInRectangle(
//				0,
//				0,
//				Main.maxTilesX,
//				Main.maxTilesY,
//				(i, j) => {
//					if (!Main.tile[i, j].HasTile) {
//						return;
//					}

//					var info = new GenerationHelper.TileNeighbourInfo(i, j).Solid;
//					if (info.Count == 8) {
//						return;
//					}

//					switch (Main.tile[i, j].TileType) {
//						case TileID.Mud:
//							GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
//							break;
//					}
//				}
//			);
//		}

//		[Task]
//		public void Ambient() {
//			// Jungle
//			GenerationHelper.ForEachInRectangle(
//				JungleTopLeft.X,
//				JungleTopLeft.Y,
//				JungleWidth,
//				JungleHeight,
//				(i, j) => {
//					if (
//						Main.tile[i, j - 1].HasTile
//						&& Main.tile[i, j - 1].TileType == TileID.JungleGrass
//						&& Main.rand.NextBool(2)
//						) {
//						int vineLength = WorldGen.genRand.Next(3, 10);
//						for (int x = 0; x < vineLength; x++) {
//							if (Main.tile[i, j + x].HasTile) {
//								break;
//							}

//							WorldGen.PlaceTile(i, j + x, TileID.JungleVines, true);
//						}
//					}
//				}
//			);
//		}

//		[Task]
//		public void Finish() {

//		}
//	}
//}
