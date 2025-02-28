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
using System.Reflection;
using BossRush.Common.General;
using System;
using Terraria.Utilities;
using System.Diagnostics;

namespace BossRush.Common.WorldGenOverhaul;
public class PlayerBiome : ModPlayer {
	HashSet<short> CurrentBiome = new HashSet<short>();
}
public class BiomeAreaID {
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
	public const short Slime = 11;
	public const short FleshRealm = 12;
	public const short Beaches = 13;
	public const short Underground = 14;
	public const short BeeNest = 15;
	public const short Hallow = 16;
	public const short Ocean = 17;
}

public partial class RogueLikeWorldGen : ModSystem {
	public static Dictionary<short, string> BiomeID;
	public override void OnModLoad() {
		BiomeID = new();
		FieldInfo[] field = typeof(BiomeAreaID).GetFields();
		for (int i = 0; i < field.Length; i++) {
			object? obj = field[i].GetValue(null);
			if (obj == null) {
				continue;
			}
			if (obj.GetType() != typeof(short)) {
				continue;
			}
			short objvalue = (short)obj;
			BiomeID.Add(objvalue, field[i].Name);
		}

	}
	public override void OnModUnload() {
		BiomeID = null;
	}

	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().WorldGenTest) {
			//tasks.ForEach(g => g.Disable());
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Spider Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Living Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wood Tree Walls")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Island Houses")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Life Crystals")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pyramids")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Altars")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hives")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Temple")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Marble")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Granite")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushrooms")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Ore and Stone")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Planting Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Larva")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cactus, Palm Trees, & Coral")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gems In Ice Biome")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Random Gems")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Vines")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Piles")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Traps")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Statues")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shell Piles")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Oasis")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Flowers")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wavy Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Rock Layer Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Weeds")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Webs And Honey")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Clay")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Herbs")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dye Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt Layer Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss Grass")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hellforge")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pots")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Place Fallen Log")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushroom Patches")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Glowing Mushrooms and Jungle Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Small Holes")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Broken Traps")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Full Desert")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wall Variety")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gem Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cave Walls")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Temple")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Generate Ice Biome")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Corruption")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Waterfalls")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Water From Sand")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Create Ocean Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Slush")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mud Caves To Grass")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt To Mud")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Lakes")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Muds Walls In Jungle")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Silt")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Grass")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Sunflowers")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Guide")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dungeon")));
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
	public UnifiedRandom Rand => WorldGen.genRand;
	public static readonly Point OffSetPoint = new Point(-64, -64);
	Rectangle rect = new();
	Point counter = OffSetPoint;
	HashSet<Point> EmptySpaceRecorder = new();
	int count = -1;
	bool IsUsingHorizontal = false;
	int offsetcount = 0;
	int additionaloffset = -1;
	bool SpawnedShrine = false;
	[Task]
	public void SetUp() {
		Biome = new();
		GridPart_X = Main.maxTilesX / 24;//small world : 175
		GridPart_Y = Main.maxTilesY / 24;//small world : 50
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	}
	[Task]
	public void AddAltar() {
		ResetTemplate_GenerationValue();
		bool RNG = false;
		for (int i = 1; i < Main.maxTilesX - 1; i++) {
			for (int j = 1; j < Main.maxTilesY - 1; j++) {
				if (!RNG) {
					if (Rand.NextBool(1500)) {
						RNG = true;
					}
				}
				else {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (offsetY == 1 && offsetX == 0) continue;
							if (!WorldGen.InWorld(i + offsetX, j + offsetY)) continue;
							if (!WorldGen.TileEmpty(i + offsetX, j + offsetY)) {
								j = Math.Clamp(j + 1, 0, Main.maxTilesY);
								break;
							}
							else {
								pass++;
							}
						}
					}
					if (pass >= 7) {
						if (WorldGen.genRand.NextBool(100)) {
							Generate_Trial(i, j);
						}
						else {
							WorldGen.PlaceTile(i, j, Main.rand.Next(TerrariaArrayID.Altar));
						}
						RNG = false;
					}
				}
			}
		}
	}
	[Task]
	public void Generate_TrialTest() {
		Generate_TrialTest(Main.maxTilesX / 3, Main.maxTilesY / 2);
	}
	[Task]
	public void GenerateSlimeZone() {
		rect = GenerationHelper.GridPositionInTheWorld24x24(16, 10, 3, 3);
		File_GenerateBiomeTemplate("Template/WG_Template", TileID.SlimeBlock, WallID.Slime, BiomeAreaID.Slime, "SlimeShrine");
		ResetTemplate_GenerationValue();
	}
	[Task]
	public void GenerateFleshZone() {
		rect = GenerationHelper.GridPositionInTheWorld24x24(4, 12, 3, 3);
		File_GenerateBiomeTemplate("Template/WG_Template", TileID.FleshBlock, WallID.Flesh, BiomeAreaID.FleshRealm, "FleshShrine");
		ResetTemplate_GenerationValue();
	}
	[Task]
	public void Re_GenerateFrost() {
		rect = GenerationHelper.GridPositionInTheWorld24x24(5, 5, 4, 4);
		File_GenerateBiomeTemplate("Template/WG_Template", TileID.Dirt, WallID.Dirt, BiomeAreaID.Forest, "");
		ResetTemplate_GenerationValue();
	}
	[Task]
	public void re_GenerateDungeon() {
		rect = GenerationHelper.GridPositionInTheWorld24x24(5, 3, 4, 4);
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			ImageData template;
			IsUsingHorizontal = ++count % 2 == 0;
			if (IsUsingHorizontal) {
				template = ImageStructureLoader.Get_Tempate("WG_Dungeon_TemplateHorizontal" + WorldGen.genRand.Next(1, 10));
			}
			else {
				template = ImageStructureLoader.Get_Tempate("WG_Dungeon_TemplateVertical" + WorldGen.genRand.Next(1, 10));
			}
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			bool ChanceOfSpawningShrine = Rand.NextBool(300) && !SpawnedShrine;
			template.EnumeratePixels((a, b, color) => {
				a += rect.X + counter.X;
				b += rect.Y + counter.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (a < rect.Left || b < rect.Top) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.B == 0 && color.G == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BlueDungeonBrick);
				}
				else if (ChanceOfSpawningShrine) {
					EmptySpaceRecorder.Add(new(a, b));
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.BlueDungeon);
			});
			if (ChanceOfSpawningShrine) {
				if (EmptySpaceRecorder.Count > 1) {
					ChanceOfSpawningShrine = false;
					SpawnedShrine = true;
				}
			}
			if (counter.X < rect.Width) {
				counter.X += template.Width;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
		Biome.Add(BiomeAreaID.Dungeon, new List<Rectangle> { rect });
		ResetTemplate_GenerationValue();
	}
	public void Generate_Trial(int X, int Y) {
		ImageData template = ImageStructureLoader.Get_Trials("TrialRoomTemplate1");
		template.EnumeratePixels((a, b, color) => {
			if (a == 25 && b == 25) {
				a += X;
				b += Y;
				GenerationHelper.FastRemoveTile(a, b);
				WorldGen.PlaceTile(a, b, ModContent.TileType<StartTrialAltar_Template_1>());
				GenerationHelper.FastPlaceWall(a, b, WallID.StoneSlab);
				return;
			}
			a += X;
			b += Y;
			GenerationHelper.FastRemoveTile(a, b);
			if (color.R == 255 && color.B == 0 && color.G == 0) {
				GenerationHelper.FastPlaceTile(a, b, TileID.StoneSlab);
			}
			else if (color.R == 0 && color.B == 255 && color.G == 0) {
				GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
			}
			GenerationHelper.FastPlaceWall(a, b, WallID.StoneSlab);
		});
	}
	public void Generate_TrialTest(int X, int Y) {
		GenerationHelper.Safe_PlaceStructure($"Trial1", new Rectangle(X, Y, 50, 50));
		WorldGen.PlaceTile(X + 50, Y + 50, ModContent.TileType<StartTrialAltar_Template_1>());
	}
	public void Generate_Shrine(string shrineType, int X, int Y, int width = 11, int height = 12) {
		if (shrineType == string.Empty) {
			return;
		}
		GenerationHelper.Safe_PlaceStructure($"Shrine/{shrineType}", new Rectangle(X, Y, width, height));
		WorldGen.PlaceTile(X + width / 2, Y + height / 2, ModContent.TileType<SlimeBossAltar>());
	}
	public void ResetTemplate_GenerationValue() {
		rect = new();
		counter = OffSetPoint;
		EmptySpaceRecorder.Clear();
		count = -1;
		IsUsingHorizontal = false;
		offsetcount = 0;
		additionaloffset = -1;
		SpawnedShrine = false;
	}
	/// <summary>
	/// Use <see cref="BiomeAreaID"/> for BiomeID<br/>
	/// Will automatically handle template placing and auto handle shrine
	/// </summary>
	/// <param name="TileID"></param>
	/// <param name="WallID"></param>
	/// <param name="BiomeID">Use <see cref="BiomeAreaID"/> for BiomeID</param>
	public void Generate_Biome(ushort TileID, ushort WallID, short BiomeID, string ShrineType) {
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			ImageData template;
			IsUsingHorizontal = ++count % 2 == 0;
			if (IsUsingHorizontal) {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateHorizontal" + WorldGen.genRand.Next(1, 9));
			}
			else {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateVertical" + WorldGen.genRand.Next(1, 9));
			}
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			bool ChanceOfSpawningShrine = Rand.NextBool(300) && !SpawnedShrine;
			template.EnumeratePixels((a, b, color) => {
				a += rect.X + counter.X;
				b += rect.Y + counter.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (a < rect.Left || b < rect.Top) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.B == 0 && color.G == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID);
				}
				else if (ChanceOfSpawningShrine) {
					EmptySpaceRecorder.Add(new(a, b));
				}
				GenerationHelper.FastPlaceWall(a, b, WallID);
			});
			if (ChanceOfSpawningShrine) {
				if (EmptySpaceRecorder.Count > 1) {
					ChanceOfSpawningShrine = false;
					SpawnedShrine = true;
				}
			}
			if (counter.X < rect.Width) {
				counter.X += template.Width;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
		if (EmptySpaceRecorder.Count > 0) {
			Point randPoint = Rand.NextFromHashSet(EmptySpaceRecorder);
			Generate_Shrine(ShrineType, randPoint.X, randPoint.Y);
		}
		if (Biome.ContainsKey(BiomeID)) {
			Biome[BiomeID].Add(rect);
		}
		else {
			Biome.Add(BiomeID, new List<Rectangle> { rect });
		}
	}
	GenerateStyle[] styles => new[] { GenerateStyle.None, GenerateStyle.FlipHorizon, GenerateStyle.FlipVertical, GenerateStyle.FlipBoth };
	/// <summary>
	/// Use this when it is assumed that you already have the template pre saved in a file format
	/// </summary>
	/// <param name="TemplatePath"></param>
	/// <param name="BiomeID"></param>
	/// <param name="ShrineType"></param>
	public void File_GenerateBiomeTemplate(string TemplatePath, ushort TileID, ushort WallID, short BiomeID, string ShrineType = "") {
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			IsUsingHorizontal = ++count % 2 == 0;
			Rectangle re = new Rectangle(rect.X + counter.X, rect.Y + counter.Y, 0, 0);
			if (IsUsingHorizontal) {
				re.Width = 64;
				re.Height = 32;
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 9), re, (x, y, t) => {
					if (rect.Contains(x, y)) {
						t.Tile_Type = TileID;
						t.Tile_WallData = WallID;
						GenerationHelper.Structure_PlaceTile(x, y, t);
						EmptySpaceRecorder.Add(new(x, y));
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 9), re, (x, y, t) => {
					if (rect.Contains(x, y)) {
						t.Tile_Type = TileID;
						t.Tile_WallData = WallID;
						GenerationHelper.Structure_PlaceTile(x, y, t);
						EmptySpaceRecorder.Add(new(x, y));
					}
				}, Main.rand.Next(styles));
			}
			if (counter.X < rect.Width) {
				counter.X += re.Width;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
		if (EmptySpaceRecorder.Count > 0 && !string.IsNullOrEmpty(ShrineType)) {
			Point randPoint = Rand.NextFromHashSet(EmptySpaceRecorder);
			Generate_Shrine(ShrineType, randPoint.X, randPoint.Y);
		}
		if (Biome.ContainsKey(BiomeID)) {
			Biome[BiomeID].Add(rect);
		}
		else {
			Biome.Add(BiomeID, new List<Rectangle> { rect });
		}
	}
	//[Task]
	//public void SetUp() {
	//	Biome = new Dictionary<short, List<Rectangle>>();
	//	GridPart_X = Main.maxTilesX / 24;
	//	GridPart_Y = Main.maxTilesY / 24;
	//	Main.worldSurface = 0;
	//	Main.rockLayer = 0;
	//	Main.spawnTileX = (int)(Main.maxTilesX / 2);
	//	Main.spawnTileY = (int)(Main.maxTilesY * .22f);
	//	WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	//	WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 22),
	//		(i, j) => {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
	//		}
	//	);
	//	//GenerationHelper.ForEachInRectangle(
	//	//	0,
	//	//	0,
	//	//	Main.maxTilesX,
	//	//	Main.maxTilesY,
	//	//	(i, j) => {
	//	//		for (int a = 1; a < 24; a++) {
	//	//			if (i == GridPart_X * a || j == GridPart_Y * a) {
	//	//				GenerationHelper.FastPlaceTile(i, j, TileID.LihzahrdBrick);
	//	//				break;
	//	//			}
	//	//		}
	//	//	}
	//	//);
	//	//small world  : x = 4200 | y = 1200
	//	//medium world : x = 6400 | y = 1800
	//	//large world  : x = 8400 | y = 2400
	//}

	//[Task]//Test
	//public void Create_MinerParadise() {
	//	//Minor Biome or soon to be
	//	int[] oreIDarr = { TileID.Copper, TileID.Tin, TileID.Iron, TileID.Lead, TileID.Silver, TileID.Tungsten, TileID.Gold, TileID.Platinum, TileID.Palladium, TileID.Cobalt, TileID.Orichalcum, TileID.Mythril, TileID.Adamantite, TileID.Titanium };
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(11, 11, 2, 5),
	//	(i, j) => {
	//		int oreID = WorldGen.genRand.Next(oreIDarr);
	//		GenerationHelper.FastPlaceTile(i, j, Main.rand.NextFloat() >= .45f ? TileID.Stone : oreID);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Stone);
	//	});
	//}
	//[Task]
	//public void Create_CloudBiome() {
	//	//Minor Biome or soon to be
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(10, 0, 4, 3),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Cloud);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Cloud);
	//	});
	//}

	//[Task]
	//public void Create_TheBoneZone() {
	//	//Minor Biome or soon to be
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 22, 3, 2),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.BoneBlock);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Bone);
	//	});
	//}

	//[Task]
	//public void Final_CleanUp() {
	//}

}
