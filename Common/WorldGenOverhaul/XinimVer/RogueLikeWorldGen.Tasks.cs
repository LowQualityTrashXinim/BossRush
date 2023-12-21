using Terraria;
using Terraria.ID;
using BossRush.Common.Utils;
using System.Threading.Tasks;

namespace BossRush.Common.WorldGenOverhaul.XinimVer;
public partial class RogueLikeWorldGen : ITaskCollection {
	/*
	I got bored so I made this
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

		//small world  : x = 4200 | y = 1200
		//medium world : x = 6400 | y = 1800
		//large world  : x = 8400 | y = 2400
	}
	[Task]
	public void Fill_Dirt() {
		GenerationHelper.ForEachInRectangle(
	0,
	0,
	Main.maxTilesX,
	Main.maxTilesY,
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
		});
	}
	[Task]
	public void Empty_AreaAroundPlayer() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(11, 11, 2),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
	[Task]
	public void Create_Jungle() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(17, 9, 4, 4),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Mud);
		});
	}
	[Task]
	public void Create_Tundra() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(3, 9, 4, 4),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
		});
	}
	[Task]
	public void Create_Crimson1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(0, 4, 2, 11),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Crimstone);
		});
	}
	[Task]
	public void Create_Corruption1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(22, 4, 2, 11),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);
		});
	}
	[Task]
	public void Empty_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(0, 21, 24, 3),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
}
