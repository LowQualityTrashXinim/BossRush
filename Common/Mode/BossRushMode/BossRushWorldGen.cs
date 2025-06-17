using BossRush.Common.Systems;
using BossRush.Common.Systems.ObjectSystem;
using BossRush.Common.Utils;
using BossRush.Common.WorldGenOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace BossRush.Common.ChallengeMode {
	public partial class BossRushWorldGen : ModSystem {
		public override void Load() {
			On_Player.UpdateBiomes += On_Player_UpdateBiomes;
			On_Player.ItemCheck_UseBossSpawners += On_Player_ItemCheck_UseBossSpawners;
		}

		private void On_Player_ItemCheck_UseBossSpawners(On_Player.orig_ItemCheck_UseBossSpawners orig, Player self, int onWhichPlayer, Item sItem) {
			if (BossRushWorld) {
				if (sItem.type == ItemID.SlimeCrown) {
					return;
				}
			}
			orig(self, onWhichPlayer, sItem);
		}

		private void On_Player_UpdateBiomes(On_Player.orig_UpdateBiomes orig, Player self) {
			if (!UniversalSystem.CanAccessContent(self, UniversalSystem.BOSSRUSH_MODE) || self.difficulty == PlayerDifficultyID.Creative) {
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

			if (IsInBiome(self, Bid.Corruption, Room)) {
				self.ZoneCorrupt = true;
			}
			if (IsInBiome(self, Bid.Crimson, Room)) {
				self.ZoneCrimson = true;
			}
			if (IsInBiome(self, Bid.BeeNest, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, Bid.Tundra, Room)) {
				self.ZoneSnow = true;
			}
			if (IsInBiome(self, Bid.Underworld, Room)) {
				self.ZoneUnderworldHeight = true;
			}
			if (IsInBiome(self, Bid.Jungle, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, Bid.Hallow, Room)) {
				self.ZoneHallow = true;
			}
			else if (IsInBiome(self, Bid.Ocean, Room)) {
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
			BossRushWorld = true;
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
			if (BiomeID == Bid.Underworld) {
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
			if (BiomeID == Bid.Underworld) {
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
		public override void PostUpdateEverything() {
			if (!Main.LocalPlayer.active || !BossRushWorld) {
				return;
			}
			if (!NPC.downedSlimeKing && !NPC.AnyNPCs(NPCID.KingSlime)) {
				if (!ObjectSystem.AnyModObjects(ModObject.GetModObjectType<KSsealed>())) {
					Rectangle rect = Room[Bid.Slime][0];
					ModObject.NewModObject(rect.Center() * 16f, Vector2.Zero, ModObject.GetModObjectType<KSsealed>());
				}
			}
		}
	}
	public abstract class NPCSealedObject : ModObject {
		public virtual int NPCTypeToFollow => 0;
		public int Counter = 0;
		public int frame = 1;
		public int frameCounter = 0;
		public sealed override void SetDefaults() {
			timeLeft = 9999;
			NPCObject_SetDefaults();
		}
		public virtual void NPCObject_SetDefaults() { }
		public sealed override void AI() {
			if (NPCTypeToFollow == 0) {
				Kill();
				return;
			}
			timeLeft = 9999;
			Inner_AI();
		}
		public virtual void Inner_AI() { }
		public virtual void Inner_Draw(SpriteBatch spritebatch) { }
		public sealed override void Draw(SpriteBatch spritebatch) {
			if (NPCTypeToFollow == 0) {
				return;
			}
			if (frame < 1) {
				return;
			}
			Inner_Draw(spritebatch);
		}
	}
	public class KSsealed : NPCSealedObject {
		public int AuraCounter = 0;
		public bool Switch = false;
		public override int NPCTypeToFollow => NPCID.KingSlime;
		public override void NPCObject_SetDefaults() {
			frame = 6;
		}
		public override void Inner_AI() {
			if (AuraCounter < 100 && !Switch) {
				AuraCounter++;
			}
			if (AuraCounter > 0 && Switch) {
				AuraCounter--;
			}
			if (AuraCounter >= 100) {
				Switch = true;
			}
			if (AuraCounter <= 0) {
				Switch = false;
			}
			if (++Counter >= 10) {
				frameCounter = BossRushUtils.Safe_SwitchValue(frameCounter, frame - 1);
				Counter = 0;
			}
			if (NPC.downedSlimeKing || NPC.AnyNPCs(NPCID.KingSlime)) {
				this.Kill();
			}
		}
		public override void Inner_Draw(SpriteBatch spritebatch) {
			Main.instance.LoadNPC(NPCTypeToFollow);
			Texture2D texture = TextureAssets.Npc[NPCTypeToFollow].Value;
			Vector2 origin = texture.Size() * .5f;
			Vector2 drawpos = position - Main.screenPosition + origin * .75f;
			Color color = Color.White;
			frameCounter = Math.Clamp(frameCounter, 0, frame);
			spritebatch.Draw(texture, drawpos, texture.Frame(1, frame, 0, frameCounter), color, 0, origin, 1f, SpriteEffects.None, 1);

			Texture2D glow = ModContent.Request<Texture2D>(BossRushTexture.OuterInnerGlow).Value;

			spritebatch.End();
			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			float percentalge = Math.Clamp(1 - AuraCounter / 300f, .5f, 1f);
			Vector2 offset = Vector2.UnitX * origin.X * .75f - Vector2.UnitY * origin.Y / (float)frame * .5f;
			Vector2 adjustment = offset - offset * (AuraCounter * .01f);
			spritebatch.Draw(glow, position - Main.screenPosition + offset, null, Color.DodgerBlue * percentalge, 0, glow.Size() * .5f, 4 + AuraCounter * .01f, SpriteEffects.None, 0);
			spritebatch.End();
			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
		}
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
			Main.spawnTileX = Main.maxTilesX / 2;
			Main.spawnTileY = Main.maxTilesY / 2;
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
			Room.Add(Bid.Forest, rectList);
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
			Room.Add(Bid.Jungle, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_BeeNest() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(15, 12, 150, 100));
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.BeeNestArenaVar, Main.rand.Next(1, 3))
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
			Room.Add(Bid.BeeNest, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_TundraArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(11, 10, 150, 100));
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
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.IceUnsafe);
			});
			Room.Add(Bid.Tundra, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CrimsonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(6, 5, 150, 100));
			//Generator.GenerateStructure(StringBuilder($"CrimsonArenaVar{WorldGen.genRand.Next(1, 4)}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.CrimsonArena, Main.rand.Next(1, 3))
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
			Room.Add(Bid.Crimson, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CorruptionArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(10, 5, 150, 100));
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.CorruptionAreana, WorldGen.genRand.Next(1, 3))
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
			Room.Add(Bid.Corruption, new List<Rectangle> { rect });
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
			Room.Add(Bid.Hallow, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_DungeonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(13, 5, 150, 100));
			//Generator.GenerateStructure(StringBuilder($"DungeonArenaVar{WorldGen.genRand.Next(1, 4)}"), rect.TopLeft().ToPoint16(), Mod);
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.DungeonAreana, Main.rand.Next(1, 3))
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				GenerationHelper.FastPlaceWall(a, b, WallID.BlueDungeonUnsafe);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BlueDungeonBrick);
				}
				else if (color.B == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
			});
			Room.Add(Bid.Dungeon, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_SlimeArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(4, 10, 150, 100));
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.SlimeArena, Main.rand.Next(1, 3))
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				GenerationHelper.FastPlaceWall(a, b, WallID.Slime);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.SlimeBlock);
				}
				else if (color.B == 255 && color.R == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
			});
			//Stopwatch watch = new();
			//watch.Start();
			//GenerationHelper.PlaceStructure("SlimeArenaVer2", rect);
			//watch.Stop();
			//string result = $"Generation time : {watch.Elapsed.ToString()}";
			//Mod.Logger.Info(result);
			//watch.Reset();
			Room.Add(Bid.Slime, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_FleshArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(7, 10, 150, 100));
			ImageData arena = ImageStructureLoader.Get(
				ImageStructureLoader.StringBuilder(ImageStructureLoader.FleshArena, Main.rand.Next(1, 3))
				);
			arena.EnumeratePixels((a, b, color) => {
				a += rect.X;
				b += rect.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				GenerationHelper.FastPlaceWall(a, b, WallID.Flesh);
				if (color.R == 255 && color.G == 255 && color.B == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
				}
				else if (color.R == 255) {
					GenerationHelper.FastPlaceTile(a, b, TileID.FleshBlock);
				}
				else if (color.B == 255 && color.R == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
				}
			});
			Room.Add(Bid.FleshRealm, new List<Rectangle> { rect });
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
			Room.Add(Bid.Ocean, new List<Rectangle> { rect });
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

			Room.Add(Bid.Underworld, new List<Rectangle> { GenerationHelper.GridPositionInTheWorld24x24(0, 20, 24, 3) });
		}
		[Task]
		public void Readjust_Final() {
		}
	}
}
