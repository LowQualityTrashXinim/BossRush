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
public class ItemAverageColor : ModSystem {

	public static Dictionary<int,Color> averageColorByID = new();
	public override void SetStaticDefaults() {
		for (int id = 0; id < ContentSamples.ItemsByType.Count; id++) {
			Texture2D sprite = TextureAssets.Item[id].Value;

			int length = sprite.Width * sprite.Height;
			Color[] color = new Color[length];

			Main.RunOnMainThread(() => 
			{
				sprite.GetData(color);


			}).Wait();
			for (int i = 0; i < length; i++) {
				averageColorByID[id] = averageColorByID[id].AddColor(color[i]);

			}
			averageColorByID[id] = averageColorByID[id].DivideColor(length);
			
		}
	}
}
