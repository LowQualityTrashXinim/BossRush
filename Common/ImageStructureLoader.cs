﻿using Microsoft.Xna.Framework.Graphics;
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

	public const string OverworldArenaVar1 = "OverworldArena1";
	public const string FleshArenaVar1 = "FleshArena1";
	public const string JungleArenaVar1 = "JungleArena1";
	public const string BeeNestArenaVar1 = "BeeNestArena1"; 
	public const string SlimeArenaVar1 = "SlimeArena1";
	public const string CrimsonArenaVar1 = "CrimsonArena1";
}
public record ImageData(int Width, Color[] Data) {
	public int Height => Data.Length / Width;
	public void EnumeratePixels(Action<int, int, Color> action) {
		for (int i = 0; i < Data.Length; i++) {
			action(i % Width, i / Width, Data[i]);
		}
	}
}