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
using System.Text;

namespace BossRush.Common.WorldGenOverhaul;
public class PlayerBiome : ModPlayer {
	HashSet<short> CurrentBiome = new HashSet<short>();
}
/// <summary>
/// short for Biome Arena ID 
/// </summary>
public class Bid {
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
	public const short Underworld = 14;
	public const short BeeNest = 15;
	public const short Hallow = 16;
	public const short Ocean = 17;
	public const short JungleTemple = 18;
	public const short Space = 19;
	public const short Caven = 20;
	public const short Advanced = 999;
}
public struct BiomeDataBundle {
	public ushort tile = 0;
	public ushort wall = 0;
	public string FormatFile = "";
	public BiomeDataBundle() {
	}
	public BiomeDataBundle(ushort t, ushort w, string file) {
		tile = t;
		wall = w;
		FormatFile = file;
	}
}
public partial class RogueLikeWorldGen : ModSystem {
	public static Dictionary<short, string> BiomeID;
	public static TimeSpan WatchTracker = TimeSpan.Zero;
	public static Dictionary<short, BiomeDataBundle> dict_BiomeBundle = new();
	public override void OnModLoad() {
		BiomeID = new();
		FieldInfo[] field = typeof(Bid).GetFields();
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
		Biome = new();
		TrialArea = new();

		dict_BiomeBundle = new() {
			{ Bid.Forest, new(TileID.Dirt, WallID.Dirt, "") },
			{ Bid.Jungle, new(TileID.Mud, WallID.Jungle, "") },
			{ Bid.Tundra, new(TileID.SnowBlock, WallID.SnowWallUnsafe, "") },
			{ Bid.Desert, new(TileID.Sandstone, WallID.Sandstone, "") },
			{ Bid.Corruption, new(TileID.Ebonstone, WallID.EbonstoneUnsafe, "") },
			{ Bid.Crimson, new(TileID.Crimstone, WallID.CrimstoneUnsafe, "") },
			{ Bid.Dungeon, new(TileID.BlueDungeonBrick, WallID.BlueDungeon, "") },
			{ Bid.Hallow, new(TileID.HallowedGrass, WallID.HallowedGrassUnsafe, "") },
			{ Bid.Ocean, new(TileID.Coralstone, WallID.Sandstone, "") },
			{ Bid.Space, new(TileID.Stone, WallID.ConfettiBlack, "") },
			{ Bid.Caven, new(TileID.Stone, WallID.Stone, "") },
			{ Bid.Underworld, new(TileID.Ash, WallID.None, "") },
		};
	}
	public override void OnModUnload() {
		BiomeID = null;
		TrialArea = null;
		dict_BiomeBundle = null;
	}

	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().WorldGenTest) {
			tasks.ForEach(g => g.Disable());
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Spider Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Living Trees")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wood Tree Walls")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Island Houses")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Life Crystals")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pyramids")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Altars")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hives")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Temple")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Marble")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Granite")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushrooms")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Ore and Stone")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Planting Trees")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Larva")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cactus, Palm Trees, & Coral")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gems In Ice Biome")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Random Gems")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Vines")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Piles")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Traps")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Statues")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shell Piles")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Oasis")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Plants")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Flowers")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Plants")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wavy Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Rock Layer Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Weeds")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Webs And Honey")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Clay")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Herbs")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dye Plants")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt Layer Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss Grass")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hellforge")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pots")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Place Fallen Log")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushroom Patches")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Glowing Mushrooms and Jungle Plants")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Small Holes")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Broken Traps")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Full Desert")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wall Variety")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gem Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cave Walls")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Temple")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Generate Ice Biome")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Corruption")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Waterfalls")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Water From Sand")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Create Ocean Caves")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Slush")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mud Caves To Grass")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt To Mud")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Lakes")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Muds Walls In Jungle")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Silt")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Grass")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Sunflowers")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Guide")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dungeon")));
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
	public static Dictionary<short, List<Rectangle>> Biome;
	public static List<Rectangle> TrialArea = new();
	public override void SaveWorldData(TagCompound tag) {
		if (Biome == null) {
			return;
		}
		tag["BiomeType"] = Biome.Keys.ToList();
		tag["BiomeArea"] = Biome.Values.ToList();
		tag["TrialArea"] = TrialArea;
	}
	public override void LoadWorldData(TagCompound tag) {
		var Type = tag.Get<List<short>>("BiomeType");
		var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
		if (Type == null || Area == null) {
			return;
		}
		Biome = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		TrialArea = tag.Get<List<Rectangle>>("TrialArea");
	}
}
public partial class RogueLikeWorldGen : ITaskCollection {
	public static string ToC(int num) {
		if (num < 0 || num >= char.MaxValue) {
			return "" + char.MinValue;
		}
		return "" + (char)num;
	}
	public static string ToC(int[] num) {
		if (num.Length < 1) {
			return "" + char.MinValue;
		}
		StringBuilder builder = new();
		for (int i = 0; i < num.Length; i++) {
			if (num[i] < 0 || num[i] >= char.MaxValue) {
				return "" + char.MinValue;
			}
			builder.Append(num[i]);
		}
		return builder.ToString();
	}
	public static GenerateStyle[] styles => new[] { GenerateStyle.None, GenerateStyle.FlipHorizon, GenerateStyle.FlipVertical, GenerateStyle.FlipBoth };
	public UnifiedRandom Rand => WorldGen.genRand;
	public static readonly Point OffSetPoint = new Point(-64, -64);
	Rectangle rect = new();
	Point counter = OffSetPoint;
	HashSet<Point> EmptySpaceRecorder = new();
	int count = -1;
	bool IsUsingHorizontal = false;
	int offsetcount = 0;
	int additionaloffset = -1;
	public string[] BiomeMapping = new string[24 * 24];
	public string GetStringDataBiomeMapping(int x, int y) {
		x = Math.Clamp(x, 0, 23);
		y = Math.Clamp(y, 0, 23);
		string assign = BiomeMapping[x + y * 24];
		if (assign == null) {
			return " ";
		}
		return assign;
	}
	public int MapIndex(int x, int y) => x + y * 24;
	[Task]
	public void SetUp() {
		WatchTracker = TimeSpan.Zero;
		Biome = new();
		GridPart_X = Main.maxTilesX / 24;//small world : 175
		GridPart_Y = Main.maxTilesY / 24;//small world : 50
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
		Main.spawnTileX = GridPart_X * 11;
		Main.spawnTileY = GridPart_Y * 11;

		Stopwatch watch = new();
		watch.Start();
		//Initialize Space biome
		Array.Fill(BiomeMapping, ToC(Bid.Space), 0, 14);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 1), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 2), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 3), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 4), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 5), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 6), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(0, 7), 1);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(11, 1), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(12, 2), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Space), MapIndex(13, 3), 3);

		//Initialize jungle temple
		Array.Fill(BiomeMapping, ToC(Bid.JungleTemple), MapIndex(7, 1), 4);
		Array.Fill(BiomeMapping, ToC(Bid.JungleTemple), MapIndex(6, 2), 6);
		Array.Fill(BiomeMapping, ToC(Bid.JungleTemple), MapIndex(6, 3), 6);
		Array.Fill(BiomeMapping, ToC(Bid.JungleTemple), MapIndex(6, 4), 4);

		//Initialize Hallow biome
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(15, 0), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(16, 1), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(17, 2), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(15, 3), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(20, 4), 4);

		//Initialize Jungle biome
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(10, 4), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(6, 5), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(7, 6), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(7, 7), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(8, 8), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(8, 9), 7);

		//Initialize Desert biome
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(10, 4), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(14, 5), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(16, 6), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(15, 7), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(14, 8), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(15, 9), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(20, 9), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(22, 10), 2);

		//Initialize Tundra biome
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(5, 6), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(4, 7), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(3, 8), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(3, 9), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 10), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 11), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 12), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(3, 13), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(4, 14), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(5, 15), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(7, 16), 2);

		//Initialize Forest biome
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(8, 10), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(7, 11), 12);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(6, 12), 13);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(7, 11), 12);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(15, 14), 3);

		//Initialize Ocean biome
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(4, 5), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(2, 6), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(1, 7), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 8), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 9), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 10), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 11), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 12), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 13), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 14), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 15), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 16), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 17), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 18), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 19), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(1, 20), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(4, 21), 2);

		//Initialize Corruption biome
		BiomeMapping[MapIndex(2, 13)] = ToC(Bid.Corruption);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 14), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 15), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 16), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(3, 17), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(4, 18), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(4, 19), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(5, 20), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(6, 21), 3);

		//Initialize Underworl biome
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(8, 14), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(18, 14), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(9, 15), 13);
		BiomeMapping[MapIndex(9, 16)] = ToC(Bid.Caven);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(13, 16), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(14, 17), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(16, 18), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(16, 19), 2);
		BiomeMapping[MapIndex(16, 20)] = ToC(Bid.Caven);

		//Initialize dungeon biome
		Array.Fill(BiomeMapping, ToC(Bid.Dungeon), MapIndex(10, 16), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Dungeon), MapIndex(9, 17), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Dungeon), MapIndex(10, 18), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Dungeon), MapIndex(11, 19), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Dungeon), MapIndex(14, 20), 2);

		//Initialize crimson biome
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 9), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 10), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 13), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 14), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 15), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(21, 16), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(22, 17), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 18), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 19), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 20), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 21), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(17, 22), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(15, 23), 5);

		watch.Stop();
		Mod.Logger.Info(watch.ToString());
	}
	[Task]
	public void AddAltar() {
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 24);
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Dirt;
		ushort wallID = WallID.Dirt;
		string horizontal = TemplatePath + "Horizontal";
		string vertical = TemplatePath + "Vertical";
		BiomeDataBundle bundle = new();
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
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				if (!modsystem.dict_Struture.TryGetValue(horizontal + Rand.Next(1, 10), out List<GenPassData> datalist)) {
					Console.WriteLine("Structure not found !");
					continue;
				}
				int X = re.X, Y = re.Y, offsetY = 0, offsetX = 0, holdX, holdY;
				switch (Main.rand.Next(styles)) {
					case GenerateStyle.None:
						for (int i = 0; i < datalist.Count; i++) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = 0; l < gdata.Count; l++) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipHorizon:
						for (int i = 0; i < datalist.Count; i++) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = gdata.Count; l > 0; l--) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipVertical:
						for (int i = datalist.Count - 1; i >= 0; i--) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = 0; l < gdata.Count; l++) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipBoth:
						for (int i = datalist.Count - 1; i >= 0; i--) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = gdata.Count; l > 0; l--) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
				}
			}
			else {
				re.Width = 32;
				re.Height = 64;
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				if (!modsystem.dict_Struture.TryGetValue(vertical + Rand.Next(1, 10), out List<GenPassData> datalist)) {
					Console.WriteLine("Structure not found !");
					continue;
				}
				int X = re.X, Y = re.Y, offsetY = 0, offsetX = 0, holdX, holdY;
				switch (Main.rand.Next(styles)) {
					case GenerateStyle.None:
						for (int i = 0; i < datalist.Count; i++) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = 0; l < gdata.Count; l++) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipHorizon:
						for (int i = 0; i < datalist.Count; i++) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = gdata.Count; l > 0; l--) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipVertical:
						for (int i = datalist.Count - 1; i >= 0; i--) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = 0; l < gdata.Count; l++) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
					case GenerateStyle.FlipBoth:
						for (int i = datalist.Count - 1; i >= 0; i--) {
							GenPassData gdata = datalist[i];
							TileData data = gdata.tileData;
							data.Tile_Type = tileID;
							data.Tile_WallData = wallID;
							for (int l = gdata.Count; l > 0; l--) {
								if (offsetY >= re.Height) {
									offsetY = 0;
									offsetX++;
								}
								holdX = X + offsetX; holdY = Y + offsetY;
								if (WorldGen.InWorld(holdX, holdY)) {
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
								offsetY++;
							}
						}
						break;
				}
			}
			if (counter.X < rect.Width) {
				counter.X += re.Width;
				if (dict_BiomeBundle.TryGetValue(Convert.ToInt16(GetStringDataBiomeMapping(counter.X / GridPart_X, counter.Y / GridPart_Y)[0]), out BiomeDataBundle value)) {
					bundle = value;
				}
				tileID = bundle.tile;
				wallID = bundle.wall;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
		watch.Stop();
		WatchTracker += watch.Elapsed;
		//Biome.Add(Bid.Forest);
		ResetTemplate_GenerationValue();
		Mod.Logger.Info("Time it took to generate whole world with template :" + WatchTracker.ToString());
	}
	public void ResetTemplate_GenerationValue() {
		rect = new();
		counter = OffSetPoint;
		EmptySpaceRecorder.Clear();
		count = -1;
		IsUsingHorizontal = false;
		offsetcount = 0;
		additionaloffset = -1;
	}
	[Task]
	public void Generate_TrialTest() {
		//Generate_TrialTest(Main.maxTilesX / 3, Main.maxTilesY / 2);
	}
	//[Task]
	//public void GenerateSlimeZone() {
	//	//rect = GenerationHelper.GridPositionInTheWorld24x24(16, 10, 3, 3);
	//	//File_GenerateBiomeTemplate("Template/WG_Template", TileID.SlimeBlock, WallID.Slime, Bid.Slime);
	//	//ResetTemplate_GenerationValue();
	//}
	//[Task]
	//public void GenerateFleshZone() {
	//	//rect = GenerationHelper.GridPositionInTheWorld24x24(4, 12, 3, 3);
	//	//File_GenerateBiomeTemplate("Template/WG_Template", TileID.FleshBlock, WallID.Flesh, Bid.FleshRealm);
	//	//ResetTemplate_GenerationValue();
	//}
	/*
	[Task]
	public void Re_GenerateForest() {
		Stopwatch watch = new();
		watch.Start();
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(8, 10, 10, 1),
			GenerationHelper.GridPositionInTheWorld24x24(7, 11, 12, 1),
			GenerationHelper.GridPositionInTheWorld24x24(6, 12, 13, 1),
			GenerationHelper.GridPositionInTheWorld24x24(7, 13, 13, 1),
			GenerationHelper.GridPositionInTheWorld24x24(15, 14, 3, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(6, 10, 14, 5);
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Dirt;
		ushort wallID = WallID.Dirt;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		watch.Stop();
		WatchTracker += watch.Elapsed;
		Biome.Add(Bid.Forest, listrect);
		ResetTemplate_GenerationValue();
	}
	[Task]
	public void Re_GenerateDungeon() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(10, 16, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(9, 17, 5, 1),
			GenerationHelper.GridPositionInTheWorld24x24(10, 18, 6, 1),
			GenerationHelper.GridPositionInTheWorld24x24(11, 19, 5, 2),
			GenerationHelper.GridPositionInTheWorld24x24(14, 20, 2, 1),
		};
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(9, 17, 7, 4);
		string TemplatePath = "Template/WG_Dungeon_Template";
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Dungeon, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	//[Task]
	//public void Re_GenerateBeeHive() {
	//	rect = GenerationHelper.GridPositionInTheWorld24x24(5, 8, 2, 2);
	//	File_GenerateBiomeTemplate("Template/WG_Template", TileID.BeeHive, WallID.Hive, Bid.BeeNest);
	//	ResetTemplate_GenerationValue();
	//}
	[Task]
	public void Re_GenerateJungle() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(6, 5, 1, 1),
			GenerationHelper.GridPositionInTheWorld24x24(7, 5, 1, 3),
			GenerationHelper.GridPositionInTheWorld24x24(8, 4, 5, 6),
			GenerationHelper.GridPositionInTheWorld24x24(13, 5, 1, 5),
			GenerationHelper.GridPositionInTheWorld24x24(14, 6, 2, 1),
			GenerationHelper.GridPositionInTheWorld24x24(14, 7, 1, 1),
			GenerationHelper.GridPositionInTheWorld24x24(14, 9, 1, 1),
		};
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(6, 4, 10, 6);
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Mud;
		ushort wallID = WallID.MudUnsafe;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Jungle, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateTundra() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(5, 6, 2, 1),
			GenerationHelper.GridPositionInTheWorld24x24(4, 7, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(3, 8, 5, 2),
			GenerationHelper.GridPositionInTheWorld24x24(2, 10, 6, 1),
			GenerationHelper.GridPositionInTheWorld24x24(2, 11, 5, 1),
			GenerationHelper.GridPositionInTheWorld24x24(2, 12, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(3, 13, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(4, 14, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(5, 15, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(7, 16, 2, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(2, 6, 7, 11);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.IceBlock;
		ushort wallID = WallID.IceUnsafe;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Tundra, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateDesert() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(13, 4, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(14, 5, 6, 1),
			GenerationHelper.GridPositionInTheWorld24x24(16, 6, 8, 1),
			GenerationHelper.GridPositionInTheWorld24x24(15, 7, 9, 1),
			GenerationHelper.GridPositionInTheWorld24x24(14, 8, 10, 1),
			GenerationHelper.GridPositionInTheWorld24x24(15, 9, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(20, 9, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(22, 10, 2, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(13, 4, 11, 7);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Sandstone;
		ushort wallID = WallID.Sandstone;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Desert, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateCorruption() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(2, 13, 1, 4),
			GenerationHelper.GridPositionInTheWorld24x24(3, 14, 1, 4),
			GenerationHelper.GridPositionInTheWorld24x24(4, 15, 1, 5),
			GenerationHelper.GridPositionInTheWorld24x24(5, 16, 1, 5),
			GenerationHelper.GridPositionInTheWorld24x24(6, 16, 1, 6),
			GenerationHelper.GridPositionInTheWorld24x24(7, 17, 2, 5),
			GenerationHelper.GridPositionInTheWorld24x24(9, 18, 1, 3),
			GenerationHelper.GridPositionInTheWorld24x24(10, 19, 1, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(2, 13, 9, 9);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Ebonstone;
		ushort wallID = WallID.EbonstoneUnsafe;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Corruption, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateCrimson() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(18, 9, 2, 1),
			GenerationHelper.GridPositionInTheWorld24x24(18, 10, 5, 1),
			GenerationHelper.GridPositionInTheWorld24x24(19, 11, 5, 2),
			GenerationHelper.GridPositionInTheWorld24x24(20, 13, 4, 4),
			GenerationHelper.GridPositionInTheWorld24x24(20, 16, 4, 1),
			GenerationHelper.GridPositionInTheWorld24x24(19, 17, 5, 1),
			GenerationHelper.GridPositionInTheWorld24x24(18, 18, 6, 2),
			GenerationHelper.GridPositionInTheWorld24x24(17, 20, 6, 1),
			GenerationHelper.GridPositionInTheWorld24x24(15, 21, 5, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(15, 9, 9, 13);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Crimstone;
		ushort wallID = WallID.CrimsonUnsafe1;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Crimson, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateHallow() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(15, 0, 9, 1),
			GenerationHelper.GridPositionInTheWorld24x24(16, 1, 8, 1),
			GenerationHelper.GridPositionInTheWorld24x24(17, 2, 7, 1),
			GenerationHelper.GridPositionInTheWorld24x24(15, 3, 9, 1),
			GenerationHelper.GridPositionInTheWorld24x24(16, 4, 8, 1),
			GenerationHelper.GridPositionInTheWorld24x24(20, 5, 4, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(15, 0, 9, 6);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Pearlstone;
		ushort wallID = WallID.HallowUnsafe1;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Hallow, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateTemple() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(7, 1, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(6, 2, 5, 1),
			GenerationHelper.GridPositionInTheWorld24x24(6, 3, 6, 1),
			GenerationHelper.GridPositionInTheWorld24x24(6, 4, 4, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(6, 1, 6, 4);
		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Dungeon_Template";
		ushort tileID = TileID.LihzahrdBrick;
		ushort wallID = WallID.LihzahrdBrick;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.JungleTemple, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
		WatchTracker += watch.Elapsed;
	}
	[Task]
	public void Re_GenerateGranite() {

	}

	[Task]
	public void Re_GenerateGiantLake() {
		List<Rectangle> listrect = new List<Rectangle>{
			GenerationHelper.GridPositionInTheWorld24x24(4, 5, 2, 1),
			GenerationHelper.GridPositionInTheWorld24x24(2, 6, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(1, 7, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(0, 8, 3, 2),

			GenerationHelper.GridPositionInTheWorld24x24(0, 10, 2, 7),
			GenerationHelper.GridPositionInTheWorld24x24(0, 17, 3, 1),
			GenerationHelper.GridPositionInTheWorld24x24(0, 18, 4, 2),
			GenerationHelper.GridPositionInTheWorld24x24(1, 20, 4, 1),

			GenerationHelper.GridPositionInTheWorld24x24(4, 21, 2, 1),
		};
		rect = GenerationHelper.GridPositionInTheWorld24x24(0, 5, 6, 17);

		Stopwatch watch = new();
		watch.Start();
		string TemplatePath = "Template/WG_Template";
		ushort tileID = TileID.Coralstone;
		ushort wallID = WallID.Sandstone;
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
				GenerationHelper.PlaceStructure(TemplatePath + "Horizontal" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
					}
				}, Main.rand.Next(styles));
			}
			else {
				re.Width = 32;
				re.Height = 64;
				GenerationHelper.PlaceStructure(TemplatePath + "Vertical" + WorldGen.genRand.Next(1, 10), re, (x, y, t) => {
					foreach (var item in listrect) {
						if (item.Contains(x, y)) {
							t.Tile_Type = tileID;
							t.Tile_WallData = wallID;
							GenerationHelper.Structure_PlaceTile(x, y, ref t);
							break;
						}
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
		Biome.Add(Bid.Ocean, listrect);
		ResetTemplate_GenerationValue();
		watch.Stop();
	}
	[Task]
	public void Re_GenerateMarble() {
		Console.WriteLine("Total gen time : " + WatchTracker.ToString());
	}
	[Task]
	public void FinalTask() {
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
		GenerationHelper.Safe_PlaceStructure($"Trial1", new Rectangle(X, Y, 49, 49));
		WorldGen.PlaceTile(X + 25, Y + 25, ModContent.TileType<StartTrialAltar_Template_1>());
	}
	public void Generate_Shrine(string shrineType, int X, int Y, int width = 11, int height = 12) {
		if (shrineType == string.Empty) {
			return;
		}
		GenerationHelper.Safe_PlaceStructure($"Shrine/{shrineType}", new Rectangle(X, Y, width, height));
		WorldGen.PlaceTile(X + width / 2, Y + height / 2, ModContent.TileType<SlimeBossAltar>());
	}

	/// <summary>
	/// Use <see cref="Bid"/> for BiomeID<br/>
	/// Will automatically handle template placing and auto handle shrine
	/// </summary>
	/// <param name="TileID"></param>
	/// <param name="WallID"></param>
	/// <param name="BiomeID">Use <see cref="Bid"/> for BiomeID</param>
	public void Generate_Biome(ushort TileID, ushort WallID, short BiomeID, string ShrineType) {
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			ImageData template;
			IsUsingHorizontal = ++count % 2 == 0;
			if (IsUsingHorizontal) {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateHorizontal" + WorldGen.genRand.Next(1, 10));
			}
			else {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateVertical" + WorldGen.genRand.Next(1, 10));
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
	/// <summary>
	/// Use this when it is assumed that you already have the template pre saved in a file format
	/// </summary>
	/// <param name="TemplatePath"></param>
	/// <param name="BiomeID"></param>
	/// <param name="ShrineType"></param>
	public void File_GenerateBiomeTemplate(string TemplatePath, ushort TileID, ushort WallID, short BiomeID) {
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
						GenerationHelper.Structure_PlaceTile(x, y, ref t);
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
						GenerationHelper.Structure_PlaceTile(x, y, ref t);
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
	*/
}

