using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.WorldBuilding;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BossRush.Common.WorldGenOverhaul.XinimVer;

public partial class RogueLikeWorldGen : ModSystem {
	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		tasks.ForEach(g => g.Disable());
		tasks.AddRange(((ITaskCollection)this).Tasks);
	}
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
		Main.worldSurface = 0;
		Main.rockLayer = 0;
		Main.spawnTileX = Main.maxTilesX / 2;
		Main.spawnTileY = Main.maxTilesY / 2;

		GenerationHelper.PerlinNoise2D smallNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D bigNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D dirtNoise = new(WorldGen.genRand.Next());
		GenerationHelper.PerlinNoise2D dirtNoiseThickness = new(WorldGen.genRand.Next());

		GenerationHelper.ForEachInRectangle(0, 0, Main.maxTilesX, Main.maxTilesY,
		(i, j) => {
			float jFloor = (3f * MathF.Sin(i * 0.025f) * MathF.Cos(i * 0.065f + 0.354135f));
			float sinVal = MathF.Sin(i * j) * .005f;
			var bignoise = bigNoise.GetValue(i, j) * 10;
			if ((smallNoise.GetValue(i + sinVal, j + sinVal) > 0f - 0.4f * MathF.Abs(bignoise * .08f)
				|| bignoise * 3f > MathF.Sin((i + j) * 0.1f) * 0.1f + 0.2f) &&
				 j > jFloor) {
				GenerationHelper.FastPlaceTile(
					i,
					j,
					dirtNoise.GetValue(i * 0.024f, j * 0.03f) > 0.3f + 0.4 * dirtNoiseThickness.GetValue(i, j) * .5f
						? TileID.Dirt : TileID.Stone
				);
			}
		});

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
	public void Create_FakeDungeon() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(14, 0, 3, 5),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.BlueDungeonBrick);
			GenerationHelper.FastPlaceWall(i, j, WallID.BlueDungeonUnsafe);
		});
	}

	[Task]
	public void Empty_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 21, 24, 3),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
}
