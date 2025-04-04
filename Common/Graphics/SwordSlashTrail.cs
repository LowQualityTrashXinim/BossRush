using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics;
public class SwordSlashTrail : ModSystem {

	public static Dictionary<int,Color> averageColorByID = new();
	public static Dictionary<int,string> swordSlashShaderType = new();


	public override void PostAddRecipes() 
	{ 
		foreach(Item item in BossRushModSystem.List_Weapon.Where(RoguelikeOverhaul_ModSystem.Optimized_CheckItem)) {
			AddColor(item);
			SetTrailDefaults(item);
		}
		foreach (Item item in BossRushModSystem.SynergyItem.Where(RoguelikeOverhaul_ModSystem.Optimized_CheckItem)) {
			AddColor(item);
			SetTrailDefaults(item);
		}

		
	}
	private static void SetTrailDefaults(Item item) 	
	{ 
		
	


	}

	public static string GetShaderType(Player player) 
	{
		if (player.magmaStone)
			return "FlameEffect";

		return "SwordTrailEffect";
	}

	private static void AddColor(Item item) {
		int id = item.type;

		if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(ContentSamples.ItemsByType[id]))
			return;
		Main.instance.LoadItem(id);
		Texture2D sprite = TextureAssets.Item[id].Value;
		int R = 0;
		int G = 0;
		int B = 0;
		int length = sprite.Width * sprite.Height;
		int nonTransparentPixelsAmount = 1;

		Color[] color = new Color[length];

		Main.RunOnMainThread(() =>
		{
			sprite.GetData(color);


		}).Wait();
		averageColorByID.Add(id, new Color());

		//loop for each pixel
		for (int i = 0; i < length; i++) {

			if (color[i].R != 0 && color[i].G != 0 && color[i].B != 0)
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
