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
	public void Load(Mod mod) {
		foreach (string filePath in mod.GetFileNames()) {
			if (!filePath.StartsWith("Assets/Images/Structures") || !filePath.EndsWith(".rawimg")) {
				continue;
			}

			Texture2D texture = mod.Assets.Request<Texture2D>(filePath[..^7], AssetRequestMode.ImmediateLoad).Value;

			Color[] textureData = new Color[texture.Width * texture.Height];
			Main.RunOnMainThread(() => texture.GetData(textureData)).Wait();

			Images[Path.GetFileName(filePath)] = new ImageData(texture.Width, textureData);
		}
	}
	public static ImageData Get(string structureName) {
		return Images[structureName + ".rawimg"];
	}
	
	public void Unload() {
	}

	public const string OverworldArena = "2x1OverworldArena";
	public const string FleshArenaVar = "3x3FleshArena";
	public const string JungleArenaVar = "3x3JungleArena";
	public const string BeeNestArenaVar = "2x4BeeNestArena"; 
	public const string SlimeArena = "3x3SlimeArena";
	public const string CrimsonArena = "3x3CrimsonArena";
	public const string CorruptionAreana = "2x4CorruptionArena";
	public const string DungeonAreana = "2x2DungeonArena";
	public const string TundraArena = "3x3TundraArena";
	public const string HallowArena = "3x3HallowArena";
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
