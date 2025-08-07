using BossRush.Common.General;
using BossRush.Common.Systems.ObjectSystem;
using BossRush.Common.Utils;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace BossRush.Common.WorldGenOverhaul;
public class PlayerBiome : ModPlayer {
	HashSet<short> CurrentBiome = new HashSet<short>();
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		CurrentBiome.Clear();
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		Point position = (new Vector2(Player.position.X / RogueLikeWorldGen.GridPart_X, Player.position.Y / RogueLikeWorldGen.GridPart_Y)).ToTileCoordinates();
		int WorldIndex = gen.MapIndex(position.X, position.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		if (zone == null) {
			return;
		}
		for (int i = 0; i < zone.Length; i++) {
			short biomeID = RogueLikeWorldGen.CharToBid(zone, i);
			CurrentBiome.Add(biomeID);
		}
		foreach (var item in CurrentBiome) {
			if (RogueLikeWorldGen.BiomeID.TryGetValue(item, out string value)) {
				Main.NewText("Player are currently in:" + value);
			}
			if (item == Bid.Forest) {
			}
		}
	}
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
	//These are biome ignore char value, do not uses this in general biome mapping
	public const short ShrineOfOffering = 998;
	public const short Advanced = 999;
}
public struct BiomeDataBundle {
	/// <summary>
	/// The main tile that will be used in the tile format
	/// </summary>
	public ushort tile = 0;
	/// <summary>
	/// This is for sub tile in the file format
	/// </summary>
	public ushort tile2 = ushort.MaxValue;
	/// <summary>
	/// Determines the chance of tile2 appearing
	/// </summary>
	public float weight2 = 1;
	public ushort wall = 0;
	public string FormatFile = "";
	public short Range = -1;
	public BiomeDataBundle() {
	}
	public BiomeDataBundle(ushort t, ushort w, string file, short r = -1) {
		tile = t;
		wall = w;
		FormatFile = file;
		Range = r;
	}
}
public partial class RogueLikeWorldGen : ModSystem {
	public static Dictionary<short, string> BiomeID;
	public static TimeSpan WatchTracker = TimeSpan.Zero;
	public static Dictionary<short, BiomeDataBundle> dict_BiomeBundle = new();
	public static Dictionary<short, List<Rectangle>> BiomeZone = new();
	public static Dictionary<short, List<short>> BiomeGroup = new();
	public static bool[] StaticNoise255x255 = new bool[65025];
	public override void OnModLoad() {
		Asset<Texture2D> sprite = ModContent.Request<Texture2D>(BossRushTexture.CommonTextureStringPattern + "StaticNoise255x255", AssetRequestMode.ImmediateLoad);
		Color[] color = new Color[65025];

		Main.RunOnMainThread(() => {
			sprite.Value.GetData(color);
		}).Wait();

		for (int i = 0; i < color.Length; i++) {
			//This is just extra careful
			if (color[i].R == 0 && color[i].G == 0 && color[i].B == 0) {
				StaticNoise255x255[i] = false;
			}
			else {
				StaticNoise255x255[i] = true;
			}
		}
		Main.RunOnMainThread(() => {
			sprite.Dispose();
		}).Wait();

		BiomeID = new();
		FieldInfo[] field = typeof(Bid).GetFields();
		for (int i = 0; i < field.Length; i++) {
			var obj = field[i].GetValue(null);
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
		BiomeZone = new();

		dict_BiomeBundle = new() {
			{ Bid.Forest, new BiomeDataBundle(TileID.Dirt, WallID.Dirt, "") with { tile2 = TileID.Stone} },
			{ Bid.Jungle, new(TileID.Mud, WallID.Jungle, "") },
			{ Bid.Tundra, new(TileID.SnowBlock, WallID.SnowWallUnsafe, "") },
			{ Bid.Desert, new(TileID.Sandstone, WallID.Sandstone, "") },
			{ Bid.Corruption, new(TileID.Ebonstone, WallID.EbonstoneUnsafe, "") },
			{ Bid.Crimson, new(TileID.Crimstone, WallID.CrimstoneUnsafe, "") },
			{ Bid.Dungeon, new(TileID.BlueDungeonBrick, WallID.BlueDungeon, "Dungeon") },
			{ Bid.Hallow, new(TileID.HallowedGrass, WallID.HallowedGrassUnsafe, "") },
			{ Bid.Ocean, new(TileID.Coralstone, WallID.Sandstone, "") },
			{ Bid.Space, new(TileID.Stone, WallID.None, "Space", 18) },
			{ Bid.Caven, new(TileID.Stone, WallID.Stone, "") },
			{ Bid.Underworld, new(TileID.Ash, WallID.None, "") },
			{ Bid.JungleTemple, new(TileID.LihzahrdBrick, WallID.LihzahrdBrickUnsafe, "") },
		};

		BiomeGroup = new();
	}
	public override void OnModUnload() {
		BiomeID = null;
		TrialArea = null;
		dict_BiomeBundle = null;
		BiomeZone = null;
		BiomeGroup = null;
	}
	public override void Load() {
	}

	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().WorldGenTest) {
			tasks.ForEach(g => g.Disable());
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
		tag["EyeRoom"] = EyeRoom;
	}
	public override void LoadWorldData(TagCompound tag) {
		var Type = tag.Get<List<short>>("BiomeType");
		var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
		if (Type == null || Area == null) {
			return;
		}
		Biome = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		TrialArea = tag.Get<List<Rectangle>>("TrialArea");
		EyeRoom = tag.Get<Rectangle>("EyeRoom");
	}
	public override void PostUpdateEverything() {
		if (!Main.LocalPlayer.active) {
			return;
		}
		if (EyeRoom.X != 0 && EyeRoom.Y != 0) {
			if (!ObjectSystem.AnyModObjects(ModObject.GetModObjectType<EyeCreature>())) {
				ModObject.NewModObject(EyeRoom.Center().ToWorldCoordinates(), Vector2.Zero, ModObject.GetModObjectType<EyeCreature>());
			}
		}
	}
}
public class EyeCreature : ModObject {
	public override void SetDefaults() {
		timeLeft = 9999;
	}
	public override void AI() {
		timeLeft = 9999;
	}
	public override void Draw(SpriteBatch spritebatch) {
		Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.Eye).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = position - Main.screenPosition;
		spritebatch.Draw(texture, drawpos, null, Color.White, 0, origin, 2, SpriteEffects.None, 1);
		Texture2D texture2 = ModContent.Request<Texture2D>(BossRushTexture.EyePupil).Value;
		Vector2 origin2 = texture2.Size() * .5f;
		Vector2 drawpos2 = position - Main.screenPosition;
		spritebatch.Draw(texture2, drawpos2, null, Color.White, 0, origin2, 2, SpriteEffects.None, 1);
	}
}
public partial class RogueLikeWorldGen : ITaskCollection {
	public Rectangle EyeRoom = new Rectangle();
	/// <summary>
	/// Convert from <see cref="Bid"/> biome area id to char type
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
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
	public static short CharToBid(string c, int index = 0) {
		if (string.IsNullOrEmpty(c)) {
			return Bid.None;
		}
		if (c[index] < 0 || c[index] >= char.MaxValue) {
			return Bid.None;
		}
		return (short)c[index];
	}
	public static GenerateStyle[] styles => new[] { GenerateStyle.None, GenerateStyle.FlipHorizon, GenerateStyle.FlipVertical, GenerateStyle.FlipBoth };
	private static UnifiedRandom Rand => WorldGen.genRand;
	public static readonly Point OffSetPoint = new Point(-64, -64);
	Rectangle rect = new();
	Point counter = OffSetPoint;
	HashSet<Point> EmptySpaceRecorder = new();
	int count = -1;
	bool IsUsingHorizontal = false;
	int offsetcount = 0;
	int additionaloffset = -1;
	public string[] BiomeMapping = new string[24 * 24];
	public static string GetStringDataBiomeMapping(int x, int y) {
		x = Math.Clamp(x, 0, 23);
		y = Math.Clamp(y, 0, 23);
		string assign = ModContent.GetInstance<RogueLikeWorldGen>().BiomeMapping[x + y * 24];
		if (assign == null) {
			return " ";
		}
		return assign;
	}
	public int MapIndex(int x, int y) => x + y * 24;
	public static short Get_BiomeIDViaPos(Vector2 position, int WorldBiomeIndex) {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		Point positionInWorld = (new Vector2(position.X / GridPart_X, position.Y / GridPart_Y)).ToTileCoordinates();
		int WorldIndex = gen.MapIndex(positionInWorld.X, positionInWorld.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return Bid.None;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		return CharToBid(zone, WorldBiomeIndex);
	}
	public static short Get_BiomeIDViaPos(Point position, int WorldBiomeIndex) {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		Point positionInWorld = new(position.X / GridPart_X, position.Y / GridPart_Y);
		int WorldIndex = gen.MapIndex(positionInWorld.X, positionInWorld.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return Bid.None;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		return CharToBid(zone, WorldBiomeIndex);
	}
	public List<Rectangle> ZoneToBeIgnored = new();
	/// <summary>
	/// This code here is pure definition of evil for the sake of "optimization", idfk if this is fast or not<br/>
	/// But don't touch this code<br/>
	/// <br/>
	/// For brief explanation of the code, the <see cref="BiomeMapping"/> is a array of string that store world biome data<br/>
	/// The array of string have a size of 24x24, because we are using grid of 24x24 system<br/>
	/// The biome ID <see cref="Bid"/> is stored as a char data
	/// </summary>
	private void InitializeBiomeWorld() {
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
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(13, 4), 3);
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
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(7, 13), 12);
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

		//Initialize Caven biome
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
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 11), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 12), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 13), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(21, 14), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(22, 15), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 16), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 17), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 18), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 19), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(17, 20), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(15, 21), 5);

		//Initialize Underworld biome
		BiomeMapping[MapIndex(0, 20)] = ToC(Bid.Underworld);
		BiomeMapping[MapIndex(23, 20)] = ToC(Bid.Underworld);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(10, 20), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(0, 21), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(9, 21), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(20, 21), 52);

	}
	[Task]
	public void SetUp() {
		ZoneToBeIgnored.Clear();
		WatchTracker = TimeSpan.Zero;
		Biome = new();
		GridPart_X = Main.maxTilesX / 24;//small world : 175
		GridPart_Y = Main.maxTilesY / 24;//small world : 50
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
		Main.spawnTileX = GridPart_X * 11;
		Main.spawnTileY = GridPart_Y * 14;

		Stopwatch watch = new();
		watch.Start();
		InitializeBiomeWorld();
		watch.Stop();
		Mod.Logger.Info(watch.ToString());

		//Forest spawn zone
		ZoneToBeIgnored.Add(new(Main.spawnTileX - 200, Main.spawnTileY - 200, 400, 200));
		//Space trial
		Point pos_SpaceTrial = new(Main.rand.Next(Main.maxTilesX), Main.rand.Next(Main.maxTilesY));
		while (Get_BiomeIDViaPos(pos_SpaceTrial, 0) != Bid.Space) {
			pos_SpaceTrial = new(Main.rand.Next(Main.maxTilesX), Main.rand.Next(Main.maxTilesY));
		}
		ZoneToBeIgnored.Add(new(pos_SpaceTrial.X, pos_SpaceTrial.Y, Trial_Space.width, Trial_Space.height));
		TrialArea.Add(new(pos_SpaceTrial.X, pos_SpaceTrial.Y, Trial_Space.width, Trial_Space.height));
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		foreach (var item in modsystem.list_Structure) {
			if (item.Get_FilePath == RogueLikeWorldGenSystem.FileDestination + "Watcher") {
				WatcherStructure = item;
			}
		}
	}
	StructureData Trial_Space = Generator.GetStructureData("Assets/Trial_Space", ModContent.GetInstance<BossRush>());
	public static bool Get_BiomeData(Point positions, int BiomeIndex, out BiomeDataBundle bundle) {
		if (dict_BiomeBundle.TryGetValue(Convert.ToInt16(GetStringDataBiomeMapping(positions.X / GridPart_X, positions.Y / GridPart_Y)[BiomeIndex]), out BiomeDataBundle value1)) {
			bundle = value1;
			return true;
		}
		bundle = new();
		return false;
	}

	Structure_XinimVer WatcherStructure = null;
	[Task]
	public void Create_Biome() {
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 24);
		string TemplatePath = "Template/WG_Template";
		string horizontal = TemplatePath + "Horizontal";
		string vertical = TemplatePath + "Vertical";
		string file = "";
		BiomeDataBundle bundle = new();
		short CurrentBiome = 0;
		if (dict_BiomeBundle.TryGetValue(Convert.ToInt16(BiomeMapping[0][0]), out BiomeDataBundle value)) {
			bundle = value;
			CurrentBiome = Convert.ToInt16(BiomeMapping[0][0]);
		}
		int noiseCounter = 0;
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			if (WorldGen.InWorld(counter.X, counter.Y)) {
				short possibleIDATM = Get_BiomeIDViaPos(counter, 0);
				if (CurrentBiome != possibleIDATM) {
					CurrentBiome = possibleIDATM;
					if (dict_BiomeBundle.TryGetValue(CurrentBiome, out BiomeDataBundle val)) {
						bundle = val;
					}
				}
			}
			IsUsingHorizontal = ++count % 2 == 0;
			Rectangle re = new Rectangle(rect.X + counter.X, rect.Y + counter.Y, 0, 0);
			int X = re.X, Y = re.Y, offsetY = 0, offsetX = 0, holdX, holdY;
			Structure_XinimVer structure = null;
			if (IsUsingHorizontal) {
				re.Width = 64;
				re.Height = 32;
				if (bundle.FormatFile == "") {
					file = horizontal + Rand.Next(1, 10);
				}
				else {
					if (bundle.Range == -1) {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateHorizontal" + Rand.Next(1, 10);
					}
					else {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateHorizontal" + Rand.Next(1, bundle.Range);
					}
				}
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				foreach (var item in modsystem.list_Structure) {
					if (item.Get_FilePath == file) {
						structure = item;
					}
				}
				if (structure == null) {
					continue;
				}
			}
			else {
				re.Width = 32;
				re.Height = 64;
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				if (bundle.FormatFile == "") {
					file = vertical + Rand.Next(1, 10);
				}
				else {
					if (bundle.Range == -1) {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateVertical" + Rand.Next(1, 10);
					}
					else {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateVertical" + Rand.Next(1, bundle.Range);
					}
				}
				foreach (var item in modsystem.list_Structure) {
					if (item.Get_FilePath == file) {
						structure = item;
					}
				}
				if (structure == null) {
					continue;
				}
			}
			int length;
			switch (Main.rand.Next(styles)) {
				case GenerateStyle.None:
					length = structure.Get_TotalLength();
					for (int i = 0; i < length; i++) {
						TileData data = structure.Get_CurrentTileData(i);
						if (offsetY >= re.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						foreach (Rectangle zone in ZoneToBeIgnored) {
							if (zone.Contains(holdX, holdY)) {
								break;
							}
							if (WorldGen.InWorld(holdX, holdY)) {
								if (Get_BiomeData(new Point(holdX, holdY), 0, out BiomeDataBundle val)) {
									if (val.tile2 != ushort.MaxValue && StaticNoise255x255[noiseCounter] && Rand.NextFloat() <= val.weight2) {
										data.Tile_Type = val.tile2;
									}
									else {
										data.Tile_Type = val.tile;
									}
									data.Tile_WallData = val.wall;
								}
								GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
							}
						}
						offsetY++;
						noiseCounter = BossRushUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
					}
					break;
				case GenerateStyle.FlipHorizon:
					for (int i = 0; i < structure.Get_Data.Length; i++) {
						GenPassData gdata = structure.Get_Data[i];
						TileData data = gdata.tileData;
						for (int l = gdata.Count; l > 0; l--) {
							if (offsetY >= re.Height) {
								offsetY = 0;
								offsetX++;
							}
							holdX = X + offsetX; holdY = Y + offsetY;
							foreach (Rectangle zone in ZoneToBeIgnored) {
								if (zone.Contains(holdX, holdY)) {
									break;
								}
								if (WorldGen.InWorld(holdX, holdY)) {
									if (Get_BiomeData(new Point(holdX, holdY), 0, out BiomeDataBundle val)) {
										if (val.tile2 != ushort.MaxValue && StaticNoise255x255[noiseCounter] && Rand.NextFloat() <= val.weight2) {
											data.Tile_Type = val.tile2;
										}
										else {
											data.Tile_Type = val.tile;
										}
										data.Tile_WallData = val.wall;
									}
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
							}
							offsetY++;
							noiseCounter = BossRushUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
						}
					}
					break;
				case GenerateStyle.FlipVertical:
					for (int i = structure.Get_Data.Length - 1; i >= 0; i--) {
						GenPassData gdata = structure.Get_Data[i];
						TileData data = gdata.tileData;
						for (int l = 0; l < gdata.Count; l++) {
							if (offsetY >= re.Height) {
								offsetY = 0;
								offsetX++;
							}
							holdX = X + offsetX; holdY = Y + offsetY;
							foreach (Rectangle zone in ZoneToBeIgnored) {
								if (zone.Contains(holdX, holdY)) {
									break;
								}
								if (WorldGen.InWorld(holdX, holdY)) {
									if (Get_BiomeData(new Point(holdX, holdY), 0, out BiomeDataBundle val)) {
										if (val.tile2 != ushort.MaxValue && StaticNoise255x255[noiseCounter] && Rand.NextFloat() <= val.weight2) {
											data.Tile_Type = val.tile2;
										}
										else {
											data.Tile_Type = val.tile;
										}
										data.Tile_WallData = val.wall;
									}
									GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
								}
							}
							offsetY++;
							noiseCounter = BossRushUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
						}
					}
					break;
				case GenerateStyle.FlipBoth:
					length = structure.Get_TotalLength();
					for (int i = length; i >= 0; i--) {
						TileData data = structure.Get_CurrentTileData(i);
						if (offsetY >= re.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						foreach (Rectangle zone in ZoneToBeIgnored) {
							if (zone.Contains(holdX, holdY)) {
								break;
							}
							if (WorldGen.InWorld(holdX, holdY)) {
								if (Get_BiomeData(new Point(holdX, holdY), 0, out BiomeDataBundle val)) {
									if (val.tile2 != ushort.MaxValue && StaticNoise255x255[noiseCounter] && Rand.NextFloat() <= val.weight2) {
										data.Tile_Type = val.tile2;
									}
									else {
										data.Tile_Type = val.tile;
									}
									data.Tile_WallData = val.wall;
								}
								GenerationHelper.Structure_PlaceTile(holdX, holdY, ref data);
							}
						}
						offsetY++;
						noiseCounter = BossRushUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
					}
					break;
			}
			if (counter.X < rect.Width) {
				counter.X += re.Width;
				if (Get_BiomeData(counter, 0, out BiomeDataBundle value1)) {
					bundle = value1;
				}
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
		ResetTemplate_GenerationValue();
		Mod.Logger.Info("Time it took to generate whole world with template :" + watch.ToString());
	}
	public void ResetTemplate_GenerationValue() {
		rect = new();
		counter = OffSetPoint;
		EmptySpaceRecorder.Clear();
		count = -1;
		IsUsingHorizontal = false;
		offsetcount = 0;
		additionaloffset = -1;
		BiomeZone.Clear();
	}
	[Task]
	public void Generate_SmallForest() {
		Rectangle forestArea = ZoneToBeIgnored[0];
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY = 0;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int datawidth = 20;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			if (Main.spawnTileX == i) {
				Main.spawnTileY = startingPoint - offsetRaise - 3;
				StructureData data = Generator.GetStructureData("Assets/ShrineOfOffering", Mod);
				Generator.GenerateStructure("Assets/ShrineOfOffering", new(i - data.width / 2, forestArea.Y + forestArea.Height / 2), Mod);
				Rectangle zone = new Rectangle(i - data.width / 2, forestArea.Y + forestArea.Height / 2, data.width, data.height);
				BiomeZone.TryAdd(Bid.ShrineOfOffering, new() { zone });
			}
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (Rand.NextBool(5)) {
						offsetRaise += Rand.Next(-1, 2);
					}
					MoveToNextX = false;
					j = startingPoint - offsetRaise;
					GenerationHelper.FastPlaceTile(i, j, TileID.Grass);
					continue;
				}
				GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
			}
			CurrentPosY = startingPoint - offsetRaise;
			if (i == LeftPos) {
				StructureData data = Generator.GetStructureData("Assets/StarterZoneHouse1", Mod);
				Generator.GenerateFromData(data, new(i - data.width / 2, CurrentPosY - data.height + 3));
			}
			else if (i == RightPos) {
				StructureData data = Generator.GetStructureData("Assets/StarterZoneHouse2", Mod);
				Generator.GenerateFromData(data, new(i - data.width / 2, CurrentPosY - data.height + 3));
			}
			else if (Rand.NextBool(26)
				&& i < LeftPos - datawidth || i > LeftPos + datawidth
				&& i < RightPos - datawidth || i > RightPos + datawidth) {
				WorldGen.GrowTree(i, CurrentPosY);
			}

		}
		Rectangle SpaceTrial = ZoneToBeIgnored[1];
		Generator.GenerateFromData(Trial_Space, new(SpaceTrial.X, SpaceTrial.Y));
	}
	[Task]
	public void Generate_EyeRoom() {
		Point point = new((int)(Main.maxTilesX * Main.rand.NextFloat(.7f, .9f)), (int)(Main.maxTilesY * Main.rand.NextFloat(.7f, .9f)));
		StructureData data = Generator.GetStructureData("Assets/Watcher", Mod);
		Generator.GenerateFromData(data, new(point.X - data.width / 2, point.Y + data.height / 2));
		EyeRoom = new(point.X - data.width / 2, point.Y + data.height / 2, data.width, data.height);
	}
	[Task]
	public void Generate_PostWorld() {
		Rectangle goldRoomSize = new(0, 0, 150, 150);
		for (int i = 0; i < Main.maxTilesX; i++) {
			for (int j = 0; j < Main.maxTilesY; j++) {
				if (CanGenerateGoldRoom(i, j)) {
					continue;
				}
			}
		}
	}

	Rectangle goldRoomSize = new(0, 0, 150, 150);
	/// <summary>
	/// Return true if the method successfully generated gold room
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public bool CanGenerateGoldRoom(int i, int j) {
		if (i > 100 && i < Main.maxTilesX - 100 && j > 100 && j < Main.maxTilesY - 100) {
			//This is where we generate our gold room via code
			if (i == Main.maxTilesX * .9f && j == Main.maxTilesY * .1f) {
				goldRoomSize.X = i;
				goldRoomSize.Y = j;
			}
			else if (i == Main.maxTilesX * .1f && j == Main.maxTilesY * .1f) {
				goldRoomSize.X = i;
				goldRoomSize.Y = j;
			}
			if (goldRoomSize.X != 0 && goldRoomSize.Y != 0) {
				if (goldRoomSize.Contains(i, j)) {
					if (i == goldRoomSize.Left
					|| j == goldRoomSize.Top
					|| i == goldRoomSize.Right - 1
					|| j == goldRoomSize.Bottom - 1) {
						GenerationHelper.FastPlaceTile(i, j, TileID.Stone);
					}
					else {
						GenerationHelper.FastPlaceTile(i, j, TileID.Gold);
					}
					if (i == goldRoomSize.Right - 1 && j == goldRoomSize.Bottom - 1) {
						goldRoomSize.X = 0;
						goldRoomSize.Y = 0;
					}
					return true;
				}
			}
		}
		return false;
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
	[Task]
	public void FinalTask() {
		//Generate_TrialTest(Main.maxTilesX / 3, Main.maxTilesY / 2);
	}
}

