using BossRush.Common.Systems;
using BossRush.Common.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Common.ChallengeMode {
	public partial class BossRushWorldGen : ModSystem {
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
			return;

			if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
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
		}
		public static int GridPart_X = Main.maxTilesX / 24;
		public static int GridPart_Y = Main.maxTilesY / 24;
		public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	}
	public partial class BossRushWorldGen : ITaskCollection {
		[Task]
		public void SetUp() {
			GridPart_X = Main.maxTilesX / 24;//small world : 175
			GridPart_Y = Main.maxTilesY / 24;//small world : 50
			Main.spawnTileX = (int)(Main.maxTilesX * .08f);
			Main.spawnTileY = (int)(Main.maxTilesY * .176f);
			WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
			WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
			Main.worldSurface = (int)(Main.maxTilesY * .22f);
			Main.rockLayer = (int)(Main.maxTilesY * .34f);
			//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 5, 24, 17),
			//	(i, j) => {
			//		GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
			//	}
			//);
		}
		[Task]
		public void Create_Arena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(1, 3, 2, 2);
			ImageData flesharena = ImageStructureLoader.Get(ImageStructureLoader.OverworldArenaVar1);
			flesharena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Grass);
					GenerationHelper.FastPlaceWall(a, b, WallID.Dirt);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
			});
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(1, 5, 2, 1),
				(i, j) => {
					GenerationHelper.FastPlaceTile(i, j, TileID.Grass);
				});
		}
		[Task]
		public void Create_JungleArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(15, 8, 3, 3);
			ImageData flesharena = ImageStructureLoader.Get(ImageStructureLoader.JungleArenaVar1);
			flesharena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
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
		}
		[Task]
		public void Create_BeeNest() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(12, 11, 2, 4);
			ImageData arena = ImageStructureLoader.Get(ImageStructureLoader.BeeNestArenaVar1);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Hive);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Hive);
			});
		}
		[Task]
		public void Create_TundraArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(9, 5, 2, 6);
			GenerationHelper.ForEachInRectangle(rect,
			(i, j) => {
				if (i <= rect.PointOnRectX(.05f) || i >= rect.PointOnRectX(.95f)) {
					//only place block if I is between 20% and 80% of rect width
					GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
				}
				else if (j <= rect.PointOnRectY(.05f) || j >= rect.PointOnRectY(.95f)) {
					//only place block if J is between 20% and 80% of rect height
					{
						GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
					}
				}
				else {
					GenerationHelper.FastPlaceTile(i, j, TileID.IceBlock);
				}
			});
		}
		[Task]
		public void Create_CrimsonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(14, 5, 3, 3);
			ImageData arena = ImageStructureLoader.Get(ImageStructureLoader.CrimsonArenaVar1);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if(a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Crimstone);
				}
				//else if (color.R == 254) {
				//	GenerationHelper.FastPlaceTile(a, b, TileID.PinkSlimeBlock);
				//}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.CrimsonUnsafe1);
			});
			//Biome.Add(RogueLike_BiomeAreaID.Crimson, rectList);
		}
		[Task]
		public void Create_CorruptionArena() {
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(19, 5, 2, 4),
			(i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);

			});
			//Biome.Add(RogueLike_BiomeAreaID.Corruption, rectList);
		}
		[Task]
		public void Create_DungeonArena() {
			GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(12, 5, 2, 5),
			(i, j) => {
				GenerationHelper.FastPlaceTile(i, j, TileID.BlueDungeonBrick);
				GenerationHelper.FastPlaceWall(i, j, WallID.BlueDungeonUnsafe);
			});
		}
		[Task]
		public void Create_SlimeArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(4, 9, 2, 2);
			ImageData arena = ImageStructureLoader.Get(ImageStructureLoader.SlimeArenaVar1);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (color.R == 255 && color.G == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.SlimeBlock);
				}
				else if (color.R == 254) {
					GenerationHelper.FastPlaceTile(a, b, TileID.PinkSlimeBlock);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Slime);
			});
			//Biome.Add(RogueLike_BiomeAreaID.BlueSlime, rectList);
		}
		[Task]
		public void Create_FleshArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(4, 12, 3, 3);
			ImageData flesharena = ImageStructureLoader.Get(ImageStructureLoader.FleshArenaVar1);
			flesharena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
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
			//GenerationHelper.ForEachInRectangle(rect, (i, j) => {
			//});
			//Biome.Add(RogueLike_BiomeAreaID.FleshRealm, rectList);
		}
		[Task]
		public void Create_Hell() {
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
	}
}
