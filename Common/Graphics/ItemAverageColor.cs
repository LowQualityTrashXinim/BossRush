using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics;
public class ItemAverageColor : ModSystem {

	public static Dictionary<int, Color> averageColorByID = new();
	public override void PostAddRecipes() {
		Stopwatch watch = new();
		watch.Start();
		foreach (Item item in BossRushModSystem.List_Weapon) {
			AddColor(item);
		}
		foreach (Item item in BossRushModSystem.SynergyItem) {
			AddColor(item);
		}
		watch.Stop();
		Console.WriteLine("Shader color calculation took : " + watch.ToString());
	}

	private static void AddColor(Item item) {
		int id = item.type;

		if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(ContentSamples.ItemsByType[id]))
			return;
		Main.instance.LoadItem(id);
		Texture2D sprite = TextureAssets.Item[id].Value;
		int R = 0,G = 0, B = 0;
		int length = sprite.Width * sprite.Height;
		int nonTransparentPixelsAmount = 1;

		Color[] color = new Color[length];

		Main.RunOnMainThread(() => {
			sprite.GetData(color);


		}).Wait();
		averageColorByID.Add(id, new Color());

		//loop for each pixel
		//Xinim optimization : check for Alpha check only instead all 3 colours check, and also only do addition if alpha is not 0
		for (int i = 0; i < length; i++) {
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

		averageColorByID[id] = new Color(R, G, B);

	}
}
