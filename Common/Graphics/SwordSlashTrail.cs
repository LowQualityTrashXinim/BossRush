using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics;
public class SwordSlashTrail : ModSystem {

	public static Dictionary<int, Color> averageColorByID = new();
	public static Dictionary<int, string> swordSlashShaderType = new();


	public override void PostAddRecipes() {
		Stopwatch watch = new();
		watch.Start();
		foreach (Item item in BossRushModSystem.List_Weapon.Where(RoguelikeOverhaul_ModSystem.Optimized_CheckItem)) {
			AddColor(item);
			SetTrailDefaults(item);
		}
		foreach (Item item in BossRushModSystem.SynergyItem.Where(RoguelikeOverhaul_ModSystem.Optimized_CheckItem)) {
			AddColor(item);
			SetTrailDefaults(item);
		}
		watch.Stop();
		Console.WriteLine("Total time to load Color :" + watch.ToString());

	}
	private static void SetTrailDefaults(Item item) {
	}

	public static string GetShaderType(Player player) {
		if (player.magmaStone)
			return "FlameEffect";

		return "SwordTrailEffect";
	}

	private static void AddColor(Item item) {
		int id = item.type;

		//why this is here ? since we already done it outside of the loop, no reason to add this
		//if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(ContentSamples.ItemsByType[id]))
		//	return;
		//Slight optimization for modded item load
		if (item.type < ItemID.Count) {
			Main.instance.LoadItem(id);
		}
		Texture2D sprite = TextureAssets.Item[id].Value;
		int R = 0, G = 0, B = 0;
		int length = sprite.Width * sprite.Height;
		int nonTransparentPixelsAmount = 1;

		Color[] color = new Color[length];

		Main.RunOnMainThread(() => {
			sprite.GetData(color);
		}).Wait();

		//Xinim optimization : check for Alpha check only instead all 3 colours check, and also only do addition if alpha is not 0
		for (int i = 0; i < color.Length; i++) {
			if (color[i].A == 0) {
				continue;
			}
			nonTransparentPixelsAmount++;
			R += color[i].R;
			G += color[i].G;
			B += color[i].B;
		}
		R /= nonTransparentPixelsAmount;
		G /= nonTransparentPixelsAmount;
		B /= nonTransparentPixelsAmount;

		//Slight small optimization by grouping both initialization of new struct and adding a new element into list into 1
		averageColorByID.Add(id, new Color(R, G, B));

	}
}
