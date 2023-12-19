using Terraria;
using Terraria.ID;
using BossRush.Common.Utils;
using System.Threading.Tasks;

namespace BossRush.Common.WorldGenOverhaul.XinimVer;
public partial class RogueLikeWorldGen : ITaskCollection {
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
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(11, 11, 2, 2),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
	[Task]
	public void Create_Crimson1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(0, 4, 1, 10),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Crimstone);
		});
	}
	[Task]
	public void Create_Corruption1() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(23, 4, 1, 10),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);
		});
	}
	[Task]
	public void Empty_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld(0, 21, 23, 3),
		(i, j) => {
			GenerationHelper.FastRemoveTile(i, j);
		});
	}
}
