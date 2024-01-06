using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.WorldBuilding;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BossRush.Common.WorldGenOverhaul.XinimVer;

public partial class RogueLikeWorldGen : ModSystem {
	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		return;
		tasks.ForEach(g => g.Disable());
		tasks.AddRange(((ITaskCollection)this).Tasks);
	}
	
	public static int[][] ResigsterPosition = new int[24][]; 
	public static int ForestValue = 4;
	public static int JungleValue = 4;
	public static int CorruptionValue = 4;
	public static int CrimsonValue = 4;
	public static int DesertValue = 2;
	public static int SnowValue = 2;
}
public partial class RogueLikeWorldGen : ITaskCollection {
	/*
	I got bored so I made this reference table
	0  | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10| 11| 12| 13| 14| 15| 16| 17| 18| 19| 20| 21| 22| 23
	1  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	2  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	3  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	4  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	5  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	6  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	8  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	9  | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	10 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	11 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	12 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	13 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	14 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	15 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	16 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	17 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	18 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	19 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	20 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	21 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	22 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	23 | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x | x
	*/
	[Task]
	public void SetUp() {
		GridPart_X = Main.maxTilesX / 24;
		GridPart_Y = Main.maxTilesY / 24;
		Main.worldSurface = 0;
		Main.rockLayer = 0;
		Main.spawnTileX = Main.maxTilesX / 2;
		Main.spawnTileY = Main.maxTilesY / 2;

		static float NormalizedSquaredDistanceFromCenter(int i, int j) {
			Point center = new(Main.maxTilesX / 2, Main.maxTilesY / 2);
			return (MathF.Pow(i - center.X, 2) + MathF.Pow(j - center.Y, 2)) / MathF.Pow(Main.maxTilesX / 2f, 2);
		}

		GenerationHelper.PerlinNoise2D smallNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D bigNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D dirtNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D dirtNoiseThickness = new(WorldGen.genRand.Next());

		GenerationHelper.ForEachInRectangle(
			0,
			0,
			Main.maxTilesX,
			Main.maxTilesY,
			(i, j) => {
				// Generates basic caves and the dome in the middle.
				float domeSize = 0.005f;
				float distance = NormalizedSquaredDistanceFromCenter(i, j);
				float jFloor = (Main.maxTilesY / 2f + 3f * MathF.Sin(i * 0.025f) * MathF.Cos(i * 0.065f + 0.354135f));
				if (
					j > jFloor && j < jFloor + 100 * WorldGen.genRand.NextFloat(0.95f, 1f)
					&& distance < WorldGen.genRand.NextFloat(domeSize, domeSize + 0.0005f)
					) {
					GenerationHelper.FastPlaceTile(
						i,
						j,
						j <= (int)(jFloor + WorldGen.genRand.NextFloat(1f, 3f)) ? TileID.Grass : TileID.Dirt
					);
					return;
				}

				if (
					(
					smallNoise.GetValue(i * 0.15f + MathF.Sin(i * j) * 0.05f, j * 0.15f + MathF.Sin(i * j) * 0.05f) > 0f - 0.4f * MathF.Abs(bigNoise.GetValue(i * 0.008f, j * 0.008f))
					|| bigNoise.GetValue(i * 0.03f, j * 0.03f) > MathF.Sin((i + j) * 0.1f) * 0.1f + 0.2f
					)
					&& (
						j > jFloor
						|| distance > WorldGen.genRand.NextFloat(domeSize, domeSize + 0.0005f)
						)
					) {
					GenerationHelper.FastPlaceTile(
						i,
						j,
						dirtNoise.GetValue(i * 0.024f, j * 0.03f) > 0.3f + 0.4 * dirtNoiseThickness.GetValue(i * 0.005f, j * 0.005f)
							? TileID.Dirt : TileID.Stone
					);
				}
			});
		int bigCaveCount = (int)(Main.maxTilesX * 0.008f);
		for (int i = 0; i < bigCaveCount; i++) {
			Point randomPoint = new Point(
				Main.spawnTileX + WorldGen.genRand.Next((int)(Main.maxTilesX * 0.05f), Main.maxTilesX / 2) * (WorldGen.genRand.NextBool() ? 1 : -1),
				Main.spawnTileY + WorldGen.genRand.Next((int)(Main.maxTilesY * 0.05f), Main.maxTilesY / 2) * (WorldGen.genRand.NextBool() ? 1 : -1)
			);

			int spread = 25;
			for (int j = 0; j < 9; j++) {
				GenerationHelper.ForEachInCircle(
					randomPoint.X + Main.rand.Next(-spread, spread),
					randomPoint.Y + Main.rand.Next(-spread, spread),
					WorldGen.genRand.Next(5, 15),
					(i, j) => {
						if (Main.rand.NextFloat() < 0.55f) {
							GenerationHelper.FastRemoveTile(i, j);
						}
					}
				);
			}
		}

		//small world  : x = 4200 | y = 1200
		//medium world : x = 6400 | y = 1800
		//large world  : x = 8400 | y = 2400
	}
	[Task]
	public void Empty_AreaAroundPlayer() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(11, 11, 2),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
	[Task]
	public void Create_Jungle() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(17, 9, 4, 4),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Mud);
		});
	}
	[Task]
	public void Create_Tundra() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(3, 9, 4, 4),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
		});
	}
	[Task]
	public void Create_Crimson1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 4, 2, 11),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Crimstone);
		});
	}
	[Task]
	public void Create_Corruption1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(22, 4, 2, 11),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);

		});
	}

	[Task]
	public void Create_Desert() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(5, 0, 3, 5),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Sandstone);
		});
	}


	[Task]
	public void Create_Dungeon() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(14, 0, 3, 5),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.BlueDungeonBrick);
			GenerationHelper.FastPlaceWall(i, j, WallID.BlueDungeonUnsafe);
		});
	}

	[Task]//Test
	public void Create_CustomBiome1() {
		//Minor Biome or soon to be
		int[] oreIDarr = new int[] { TileID.Copper, TileID.Tin, TileID.Iron, TileID.Lead, TileID.Silver, TileID.Tungsten, TileID.Gold, TileID.Platinum, TileID.Palladium, TileID.Cobalt, TileID.Orichalcum, TileID.Mythril, TileID.Adamantite, TileID.Titanium };
		int random = WorldGen.genRand.Next(-1, 2);
		int x = 14 + random;
		int y = 18 + random;
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(x, y, 2, 2),
		(i, j) => {
			int oreID = WorldGen.genRand.Next(oreIDarr);
			GenerationHelper.FastPlaceTile(i, j, Main.rand.NextFloat() >= .45f ? TileID.Stone : oreID);
			GenerationHelper.FastPlaceWall(i, j, WallID.Stone);
		});
	}
	[Task]
	public void Create_CustomBiome2() {
		//Minor Biome or soon to be
		int random = WorldGen.genRand.Next(-1, 2);
		int x = 9 + random;
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(x, 0, 2, 2),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Cloud);
			GenerationHelper.FastPlaceWall(i, j, WallID.Cloud);
		});
	}

	[Task]
	public void Empty_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 22, 24, 2),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
}
