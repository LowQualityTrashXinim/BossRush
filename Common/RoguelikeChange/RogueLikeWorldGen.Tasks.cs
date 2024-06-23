using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Utils;
using Terraria.WorldBuilding;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Common.WorldGenOverhaul;

public class RogueLike_BiomeAreaID {
	public const short None = 0;
	public const short Forest = 1;
	public const short Jungle = 2;
	public const short Tundra = 3;
	public const short Desert = 4;
	public const short Crimson = 5;
	public const short Corruption = 6;
	public const short Dungeon = 7;
	public const short BlueShroom = 8;
	public const short Granite = 9;
	public const short Marble = 10;
	public const short BlueSlime = 11;
	public const short FleshRealm = 12;
}

public partial class RogueLikeWorldGen : ModSystem {
	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<BossRushModConfig>().WorldGenTest) {
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
	public static Dictionary<short, List<Rectangle>> Biome;
	public override void SaveWorldData(TagCompound tag) {
		if (Biome == null) {
			return;
		}
		tag["BiomeType"] = Biome.Keys.ToList();
		tag["BiomeArea"] = Biome.Values.ToList();
	}
	public override void LoadWorldData(TagCompound tag) {
		var Type = tag.Get<List<short>>("BiomeType");
		var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
		if (Type == null || Area == null) {
			return;
		}
		Biome = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
	}
}
public partial class RogueLikeWorldGen : ITaskCollection {
	[Task]
	public void SetUp() {
		Biome = new Dictionary<short, List<Rectangle>>();
		GridPart_X = Main.maxTilesX / 24;
		GridPart_Y = Main.maxTilesY / 24;
		Main.worldSurface = 0;
		Main.rockLayer = 0;
		Main.spawnTileX = Main.maxTilesX / 2;
		Main.spawnTileY = Main.maxTilesY / 2;
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
		GenerationHelper.ForEachInRectangle(
			0,
			0,
			Main.maxTilesX,
			Main.maxTilesY,
			(i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
			}
			);
		//GenerationHelper.ForEachInRectangle(
		//	0,
		//	0,
		//	Main.maxTilesX,
		//	Main.maxTilesY,
		//	(i, j) => {
		//		for (int a = 1; a < 24; a++) {
		//			if (i == GridPart_X * a || j == GridPart_Y * a) {
		//				GenerationHelper.FastPlaceTile(i, j, TileID.LihzahrdBrick);
		//				break;
		//			}
		//		}
		//	}
		//);
		//small world  : x = 4200 | y = 1200
		//medium world : x = 6400 | y = 1800
		//large world  : x = 8400 | y = 2400
	}
	[Task]
	public void Create_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 22, 24, 1), GenerationHelper.FastRemoveTile);
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 23, 24, 1),
		(i, j) => {
			if (j == GridPart_Y * 23) {
				GenerationHelper.FastPlaceTile(i, j, TileID.AshGrass);
			}
			else {
				GenerationHelper.FastPlaceTile(i, j, TileID.Ash);
			}
		});
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
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(17, 8, 4, 4));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			if (i <= rectList[0].PointOnRectX(.2f / WorldWidthHeight_Ratio) || i >= rectList[0].PointOnRectX(.8f + WorldHeightWidth_Ratio)) {
				//only place block if I is between 20% and 80% of rect width
				GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
			}
			else if (j <= rectList[0].PointOnRectY(.2f) || j >= rectList[0].PointOnRectY(.8f)) {
				//only place block if J is between 20% and 80% of rect height
				{
					GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
				}
			}
			else {
				GenerationHelper.FastRemoveTile(i, j);
			}
		});
		Biome.Add(RogueLike_BiomeAreaID.Jungle, rectList);
	}
	[Task]
	public void Create_Tundra() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(3, 10, 4, 4));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			if (i <= rectList[0].PointOnRectX(.2f) || i >= rectList[0].PointOnRectX(.8f)) {
				GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
			}
			else if (j <= rectList[0].PointOnRectY(.2f) || j >= rectList[0].PointOnRectY(.8f)) {
				GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
			}
			else {
				GenerationHelper.FastRemoveTile(i, j);
			}
		});
		Biome.Add(RogueLike_BiomeAreaID.Tundra, rectList);
	}
	[Task]
	public void Create_Crimson() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(0, 2, 3, 5));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Crimstone);
		});
		Biome.Add(RogueLike_BiomeAreaID.Crimson, rectList);
	}
	[Task]
	public void Create_Corruption() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(21, 2, 3, 5));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);

		});
		Biome.Add(RogueLike_BiomeAreaID.Corruption, rectList);
	}
	[Task]
	public void Create_Desert() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(5, 0, 3, 5));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Sandstone);
		});
		Biome.Add(RogueLike_BiomeAreaID.Desert, rectList);
	}
	[Task]
	public void Create_BlueShroom() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(16, 0, 3, 5));
		GenerationHelper.ForEachInRectangle(rectList[0],
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.MushroomGrass);
		});
		Biome.Add(RogueLike_BiomeAreaID.BlueShroom, rectList);
	}
	[Task]
	public void Create_BigGraniteOrMarble() {
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(22, 9, 2, 3));
		if (WorldGen._genRand.NextBool()) {
			GenerationHelper.ForEachInRectangle(rectList[0],
			(i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.Granite);
				GenerationHelper.FastPlaceWall(i, j, WallID.GraniteUnsafe);
			});
			Biome.Add(RogueLike_BiomeAreaID.Granite, rectList);
		}
		else {
			GenerationHelper.ForEachInRectangle(rectList[0],
			(i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.Marble);
				GenerationHelper.FastPlaceWall(i, j, WallID.MarbleUnsafe);
			});
			Biome.Add(RogueLike_BiomeAreaID.Marble, rectList);
		}
	}
	[Task]
	public void Create_Dungeon() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(20, 16, 4, 5),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.BlueDungeonBrick);
			GenerationHelper.FastPlaceWall(i, j, WallID.BlueDungeonUnsafe);
		});
	}
	[Task]//Test
	public void Create_MinerParadise() {
		//Minor Biome or soon to be
		int[] oreIDarr = { TileID.Copper, TileID.Tin, TileID.Iron, TileID.Lead, TileID.Silver, TileID.Tungsten, TileID.Gold, TileID.Platinum, TileID.Palladium, TileID.Cobalt, TileID.Orichalcum, TileID.Mythril, TileID.Adamantite, TileID.Titanium };
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(5, 17, 4, 4),
		(i, j) => {
			int oreID = WorldGen.genRand.Next(oreIDarr);
			GenerationHelper.FastPlaceTile(i, j, Main.rand.NextFloat() >= .45f ? TileID.Stone : oreID);
			GenerationHelper.FastPlaceWall(i, j, WallID.Stone);
		});
	}
	[Task]
	public void Create_CloudBiome() {
		//Minor Biome or soon to be
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(10, 0, 4, 3),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.Cloud);
			GenerationHelper.FastPlaceWall(i, j, WallID.Cloud);
		});
	}
	[Task]
	public void Create_SlimeWorldOrFleshRealm() {
		//Minor Biome or soon to be
		List<Rectangle> rectList = new List<Rectangle>();
		rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(10, 14, 2, 3));
		if (WorldGen._genRand.NextBool()) {
			GenerationHelper.ForEachInRectangle(rectList[0],
			(i, j) => {
				if (j <= rectList[0].Y + rectList[0].Height * .75f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.PinkSlimeBlock);
				}
				else {
					GenerationHelper.FastPlaceWall(i, j, WallID.Slime);
				}
				GenerationHelper.FastPlaceTile(i, j, TileID.SlimeBlock);
			});
			Biome.Add(RogueLike_BiomeAreaID.BlueSlime, rectList);
		}
		else {
			GenerationHelper.ForEachInRectangle(rectList[0], (i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.FleshBlock);
				GenerationHelper.FastPlaceWall(i, j, WallID.Flesh);
			});
			Biome.Add(RogueLike_BiomeAreaID.FleshRealm, rectList);
		}
	}
	[Task]
	public void Create_TheBoneZone() {
		//Minor Biome or soon to be
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 22, 3, 2),
		(i, j) => {
			GenerationHelper.FastPlaceTile(i, j, TileID.BoneBlock);
			GenerationHelper.FastPlaceWall(i, j, WallID.Bone);
		});
	}
	[Task]
	public void Create_Path() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(0, 23, 23, 1), GenerationHelper.FastRemoveTile);
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(24, 22, 24, 1), GenerationHelper.FastRemoveTile);
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(22, 0, 1, 24), GenerationHelper.FastRemoveTile);
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(26, 23, 1, 22), GenerationHelper.FastRemoveTile);
		//Create path from hel-la-va-tor through slime biome to miner paradise
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(0, 36, 27, 1), GenerationHelper.FastRemoveTile);
	}
	[Task]
	public void Final_CleanUp() {
	}

}
