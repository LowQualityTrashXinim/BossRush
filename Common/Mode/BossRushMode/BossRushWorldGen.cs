using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Utils;
using Terraria.WorldBuilding;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.WorldGenOverhaul;
using Terraria.Utilities;
using StructureHelper;

namespace BossRush.Common.ChallengeMode {
	public partial class BossRushWorldGen : ModSystem {
		public override void Load() {
			On_Player.UpdateBiomes += On_Player_UpdateBiomes;
		}
		public override void Unload() {
		}
		private void On_Player_UpdateBiomes(On_Player.orig_UpdateBiomes orig, Player self) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				orig(self);
				return;
			}
			self.ZoneCorrupt = false;
			self.ZoneCrimson = false;
			self.ZoneJungle = false;
			self.ZoneSnow = false;
			self.ZoneHallow = false;
			self.ZoneUnderworldHeight = false;
			self.ZoneBeach = false;
			Tile tileSafely = Framing.GetTileSafely(self.Center);
			if (tileSafely != null)
				self.behindBackWall = tileSafely.WallType > 0;

			if (IsInBiome(self, BiomeAreaID.Corruption, Room)) {
				self.ZoneCorrupt = true;
			}
			if (IsInBiome(self, BiomeAreaID.Crimson, Room)) {
				self.ZoneCrimson = true;
			}
			if (IsInBiome(self, BiomeAreaID.BeeNest, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, BiomeAreaID.Tundra, Room)) {
				self.ZoneSnow = true;
			}
			if (IsInBiome(self, BiomeAreaID.Underground, Room)) {
				self.ZoneUnderworldHeight = true;
			}
			if (IsInBiome(self, BiomeAreaID.Jungle, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, BiomeAreaID.Hallow, Room)) {
				self.ZoneHallow = true;
			}
			else if (IsInBiome(self, BiomeAreaID.Ocean, Room)) {
				self.ZoneBeach = true;
			}
			if (ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh)) {
				Main.hardMode = true;
			}
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				return;
			}
			if (UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_WORLDGEN)) {
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
			}
			else {
				tasks.ForEach(g => g.Disable());
				tasks.AddRange(((ITaskCollection)this).Tasks);
			}
		}
		public Dictionary<short, List<Rectangle>> Room;
		public static bool IsInBiome(Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				return false;
			}
			List<Rectangle> rectList = Room[BiomeID];
			foreach (Rectangle rect in rectList) {
				if (rect.Contains((int)player.Center.X / 16, (int)player.Center.Y / 16)) {
					return true;
				}
			}
			return false;
		}
		public static bool FindSuitablePlaceToTeleport(Mod mod, Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				mod.Logger.Error("Room return null");
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				mod.Logger.Error("Biome id doesn't exist in the dictionary");
				return false;
			}
			if (BiomeID == BiomeAreaID.Underground) {
				player.Teleport(new Vector2(RogueLikeWorldGen.GridPart_X * 12f, RogueLikeWorldGen.GridPart_Y * 21.3f).ToWorldCoordinates());
				player.AddBuff(BuffID.Featherfall, BossRushUtils.ToSecond(2.5f));
				return true;
			}
			List<Rectangle> rect = Room[BiomeID];
			int failsafe = 0;
			while (failsafe <= 9999) {
				Rectangle roomPosition = Main.rand.Next(rect);
				Point position = new Point(
					Main.rand.Next(roomPosition.Left, roomPosition.Right + 1),
					Main.rand.Next(roomPosition.Top, roomPosition.Bottom));
				if (WorldGen.TileEmpty(position.X, position.Y)) {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (WorldGen.TileEmpty(position.X + offsetX, position.Y + offsetY)) {
								pass++;
							}
						}
					}
					if (pass >= 8) {
						player.Teleport(position.ToVector2().ToWorldCoordinates());
						player.AddBuff(BuffID.Featherfall, BossRushUtils.ToSecond(2.5f));
						return true;
					}
					failsafe++;
				}
			}
			mod.Logger.Error("Fail to find a suitable spot to teleport");
			return false;
		}
		public static bool FindSuitablePlaceToTeleport(Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				return false;
			}
			List<Rectangle> rect = Room[BiomeID];
			if (BiomeID == BiomeAreaID.Underground) {
				player.Teleport(new Vector2(RogueLikeWorldGen.GridPart_X * 12f, RogueLikeWorldGen.GridPart_Y * 21.3f).ToWorldCoordinates());
				player.AddBuff(BuffID.Featherfall, BossRushUtils.ToSecond(2.5f));
				return true;
			}
			int failsafe = 0;
			while (failsafe <= 9999) {
				Rectangle roomPosition = Main.rand.Next(rect);
				Point position = new Point(
					Main.rand.Next(roomPosition.Left, roomPosition.Right + 1),
					Main.rand.Next(roomPosition.Top, roomPosition.Bottom));
				if (WorldGen.TileEmpty(position.X, position.Y)) {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (WorldGen.TileEmpty(position.X + offsetX, position.Y + offsetY)) {
								pass++;
							}
						}
					}
					if (pass >= 8) {
						player.Teleport(position.ToVector2().ToWorldCoordinates());
						player.AddBuff(BuffID.Featherfall, BossRushUtils.ToSecond(2.5f));
						return true;
					}
					failsafe++;
				}
			}
			return false;
		}
		public bool BossRushWorld = false;
		public override void SaveWorldData(TagCompound tag) {
			tag["BossRushWorld"] = BossRushWorld;
			if (Room == null) {
				return;
			}
			tag["BiomeType"] = Room.Keys.ToList();
			tag["BiomeArea"] = Room.Values.ToList();
		}
		public override void LoadWorldData(TagCompound tag) {
			if (tag.TryGet("BossRushWorld", out bool BossRushMode)) {
				BossRushWorld = BossRushMode;
			}
			var Type = tag.Get<List<short>>("BiomeType");
			var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
			if (Type == null || Area == null) {
				return;
			}
			Room = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		}
		public static string StringBuilder(string FileName) => $"Assets/Structures/{FileName}";
	}
	public partial class BossRushWorldGen : ITaskCollection {
		[Task]
		public void SetUp() {
			Room = new Dictionary<short, List<Rectangle>>();
			RogueLikeWorldGen.GridPart_X = Main.maxTilesX / 24;//small world : 175
			RogueLikeWorldGen.GridPart_Y = Main.maxTilesY / 24;//small world : 50
			RogueLikeWorldGen.WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
			RogueLikeWorldGen.WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
			Main.worldSurface = (int)(Main.maxTilesY * .22f);
			Main.rockLayer = (int)(Main.maxTilesY * .34f);
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 5, 24, 15),
				(i, j) => { GenerationHelper.FastPlaceTile(i, j, TileID.GraniteBlock); });
			WorldGen._genRand = new UnifiedRandom(WorldGen._genRandSeed);
		}
		[Task]
		public void Create_Arena() {
			List<Rectangle> rectList = new List<Rectangle>();
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(3, 3, 2, 2);
			ImageData arena = ImageStructureLoader.Get(ImageStructureLoader.OverworldArena + 1);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 254) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Grass);
				}
				else if (color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Glass);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
					GenerationHelper.FastPlaceWall(a, b, WallID.Dirt);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
			});
			rectList.Add(rect);
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(3, 5, 2, 1),
				(i, j) => {
					GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
				});
			rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(3, 5, 2, 1));
			Main.spawnTileX = rect.X + ((rect.Right - rect.X) / 2);
			Main.spawnTileY = rect.Y + ((rect.Bottom - rect.Y) / 2);
			Room.Add(BiomeAreaID.Forest, rectList);
		}
		[Task]
		public void Create_JungleArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(15, 8, 3, 3);
			ImageData flesharena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.JungleArenaVar, 1)
				);
			flesharena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 254) {
					GenerationHelper.FastPlaceTile(a, b, TileID.JungleGrass);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Mud);
					GenerationHelper.FastPlaceWall(a, b, WallID.Jungle);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Jungle);
			});
			Room.Add(BiomeAreaID.Jungle, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_BeeNest() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(15, 12, 2, 4);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.BeeNestArenaVar, 1)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Hive);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms, PaintID.BluePaint);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Hive);
			});
			Room.Add(BiomeAreaID.BeeNest, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_TundraArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(11, 10, 3, 3);
			//Generator.GenerateStructure(StringBuilder($"TundraArenaVar{1}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.TundraArena, 1)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.IceBlock);
				}
				else if (color.R == 240) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BreakableIce);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.IceUnsafe);
			});
			Room.Add(BiomeAreaID.Tundra, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CrimsonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(6, 5, 3, 3);
			//Generator.GenerateStructure(StringBuilder($"CrimsonArenaVar{WorldGen.genRand.Next(1, 4)}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.CrimsonArena, 2)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Crimstone);
				}
				else if (color.R == 200 && color.G == 10) {
					GenerationHelper.FastPlaceTile(a, b, TileID.CrimsonGrass);
				}
				else if (color.R == 200 && color.G == 100) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.CrimsonUnsafe1);
			});
			Room.Add(BiomeAreaID.Crimson, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CorruptionArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(10, 5, 2, 4);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.CorruptionAreana, 2)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Ebonstone);
				}
				else if (color.R == 200 && color.G == 10) {
					GenerationHelper.FastPlaceTile(a, b, TileID.CorruptGrass);
				}
				else if (color.R == 200 && color.G == 100) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.CorruptionUnsafe1);
			});
			Room.Add(BiomeAreaID.Corruption, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_HallowArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(9, 15, 3, 3);
			//Generator.GenerateStructure(StringBuilder($"HallowArenaVar{WorldGen.genRand.Next(1, 3)}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.HallowArena, 1)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Pearlstone);
				}
				else if (color.R == 200 && color.G == 10) {
					GenerationHelper.FastPlaceTile(a, b, TileID.HallowedGrass);
				}
				else if (color.R == 200 && color.G == 100) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.HallowUnsafe1);
			});
			Room.Add(BiomeAreaID.Hallow, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_DungeonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(13, 5, 2, 2);
			//Generator.GenerateStructure(StringBuilder($"DungeonArenaVar{WorldGen.genRand.Next(1, 4)}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.DungeonAreana, 1)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BlueDungeonBrick);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				if (color.G == 255 && color.R == 0) {
					GenerationHelper.FastPlaceWall(a, b, WallID.PinkDungeon);
				}
				else {
					GenerationHelper.FastPlaceWall(a, b, WallID.BlueDungeonUnsafe);
				}
			});
			Room.Add(BiomeAreaID.Dungeon, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_SlimeArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(4, 10, 3, 3);
			GenerationHelper.PlaceStructure(rect, SaverOptimizedMethod.Default);

			//Old
			//ImageData arena = ImageStructureLoader.Get(
			//	ImageStructureLoader.StringBuilder(ImageStructureLoader.SlimeArena, 2)
			//	);
			//arena.EnumeratePixels((a, b, color) => {
			//	a += rect.X;
			//	b += rect.Y;
			//	if (a > rect.Right || b > rect.Bottom) {
			//		return;
			//	}
			//	GenerationHelper.FastRemoveTile(a, b);
			//	if (color.R == 255 && color.G == 255 && color.B == 0) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
			//	}
			//	else if (color.R == 255) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.SlimeBlock);
			//	}
			//	else if (color.R == 200) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.PinkSlimeBlock);
			//	}
			//	else if (color.R == 150) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.FrozenSlimeBlock);
			//	}
			//	else if (color.B == 255) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
			//	}
			//	GenerationHelper.FastPlaceWall(a, b, WallID.Slime);
			//});
			//Room.Add(BiomeAreaID.Slime, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_FleshArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(7, 10, 3, 3);
			//Generator.GenerateStructure(StringBuilder($"FleshArenaVar{1 + WorldGen.genRand.NextBool().ToInt()}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(ImageStructureLoader.StringBuilder(ImageStructureLoader.FleshArenaVar, 2));
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.FleshBlock);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Flesh);
			});
			Room.Add(BiomeAreaID.FleshRealm, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_OceanArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(20, 2, 3, 3);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.OceanArena, 1)
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Sand);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				else if (color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Sandstone);
				}
			});
			Room.Add(BiomeAreaID.Ocean, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_Hell() {
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 20, 24, 4),
			(i, j) => {
				if (j == RogueLikeWorldGen.GridPart_Y * 21f
				|| j == RogueLikeWorldGen.GridPart_Y * 20.5f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.Platforms);
				}
				if (j < RogueLikeWorldGen.GridPart_Y * 21.4f) {
					return;
				}
				if (j == RogueLikeWorldGen.GridPart_Y * 21.4f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.AshGrass);
				}
				else {
					GenerationHelper.FastPlaceTile(i, j, TileID.Ash);
				}
			});

			Room.Add(BiomeAreaID.Underground, new List<Rectangle> { GenerationHelper.GridPositionInTheWorld24x24(0, 20, 24, 3) });
		}
		[Task]
		public void Readjust_Final() {
		}
	}
}
