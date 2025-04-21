using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Common;
public class ImageStructureLoader : ILoadable {
	private static readonly Dictionary<string, ImageData> Images = new();
	private static readonly Dictionary<string, ImageData> ImagesTemplate = new();
	private static readonly Dictionary<string, ImageData> ImagesTrial = new();
	public void Unload() {
		Images.Clear();
		ImagesTemplate.Clear();
		ImagesTrial.Clear();
	}
	public void Load(Mod mod) {
		foreach (string filePath in mod.GetFileNames()) {
			if (!filePath.EndsWith(".rawimg")) {
				continue;
			}
			if (filePath.StartsWith("Assets/Images/Structures")) {
				Texture2D texture = mod.Assets.Request<Texture2D>(filePath[..^7], AssetRequestMode.ImmediateLoad).Value;

				Color[] textureData = new Color[texture.Width * texture.Height];
				Main.RunOnMainThread(() => texture.GetData(textureData)).Wait();

				Images[Path.GetFileName(filePath)] = new ImageData(texture.Width, textureData);
			}
			else
			if (filePath.StartsWith("Assets/Images/Template")) {
				Texture2D texture = mod.Assets.Request<Texture2D>(filePath[..^7], AssetRequestMode.ImmediateLoad).Value;

				Color[] textureData = new Color[texture.Width * texture.Height];
				Main.RunOnMainThread(() => texture.GetData(textureData)).Wait();

				ImagesTemplate[Path.GetFileName(filePath)] = new ImageData(texture.Width, textureData);
			}
			else if (filePath.StartsWith("Assets/Images/Trials")) {
				Texture2D texture = mod.Assets.Request<Texture2D>(filePath[..^7], AssetRequestMode.ImmediateLoad).Value;

				Color[] textureData = new Color[texture.Width * texture.Height];
				Main.RunOnMainThread(() => texture.GetData(textureData)).Wait();

				ImagesTrial[Path.GetFileName(filePath)] = new ImageData(texture.Width, textureData);
			}
		}
	}
	public static ImageData Get(string structureName) {
		return Images[structureName + ".rawimg"];
	}
	public static ImageData Get_Tempate(string structureName) {
		return ImagesTemplate[structureName + ".rawimg"];
	}
	public static ImageData Get_Trials(string structureName) {
		return ImagesTrial[structureName + ".rawimg"];
	}

	public const string OverworldArena = "2x1OverworldArena";
	public const string FleshArena = "FleshArena";
	public const string JungleArenaVar = "3x3JungleArena";
	public const string BeeNestArenaVar = "2x4BeeNestArena";
	public const string SlimeArena = "SlimeArena";
	public const string CrimsonArena = "CrimsonArena";
	public const string CorruptionAreana = "CorruptionArena";
	public const string DungeonAreana = "2x2DungeonArena";
	public const string TundraArena = "3x3TundraArena";
	public const string HallowArena = "3x3HallowArena";
	public const string OceanArena = "3x3OceanArena";
	public static string StringBuilder(string ArenaName, int Variant) => $"{ArenaName}{Variant}";
}
public record ImageData(int Width, Color[] Data) {
	public int Height => Data.Length / Width;
	public void EnumeratePixels(Action<int, int, Color> action) {
		for (int i = 0; i < Data.Length; i++) {
			action(i % Width, i / Width, Data[i]);
		}
	}
}
