using BossRush.Common.Utils;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common;
public class RogueLikeWorldGenSystem : ModSystem {
	public List<GenPassData> list_genPass = new();
	public override void PostSetupContent() {
		try {
			//foreach (string filePath in Mod.GetFileBytes()) {
			//	if (!filePath.StartsWith("Assets/TestFormat/")) {
			//		continue;
			//	}
			//	FileStream file = File.OpenRead(filePath);
			//	StreamReader r = new StreamReader(file);

			//	int i = 0;
			//	string line = "";
			//	int amount = 1;
			//	TileData tile = null;
			//	while (i != -1) {
			//		i = r.Read();
			//		char c = (char)i;
			//		//This mean the upcoming next tile data is definitely gonna be number or another new tile data
			//		if (c == '}') {
			//			i = r.Read();
			//			if (i == -1) {
			//				break;
			//			}
			//			//We are reading new tile data, as such this mean that previous tile data only have 1
			//			//So we are creating a new genpass with count of amount
			//			if ((char)i == '{') {
			//				list_genPass.Add(new(new(line), amount));
			//				line = "";
			//				continue;
			//			}
			//			//This mean there are multiple of said tile above
			//			//Which mean we should just create a new tile datat and then set count to it after we retrieve all the needed amount
			//			else {
			//				tile = new(line);
			//				line = "";
			//				continue;
			//			}
			//		}
			//		//This mean we are entering a new tile data
			//		if (c == '{') {
			//			//Check in case the previous check if tile data is present or not
			//			if (tile != null) {
			//				list_genPass.Add(new(tile, amount));
			//				tile = null;
			//			}
			//			amount = 1;
			//			line = "";
			//			continue;
			//		}
			//		line += c;
			//	}
			//	r.Close();
			//	file.Close();
			//}
		}
		catch {

		}
	}
}
public class GenPassData {
	public TileData tileData { get; private set; }
	public int Count { get; private set; }
	public GenPassData() {

	}
	public GenPassData(TileData tileData, int count) {
		this.tileData = tileData;
		Count = count;
	}
	public void Set(TileData data, int count) {
		this.Count = count;
		this.tileData = data;
	}
	public void Clear() {
		this.Count = 0;
		this.tileData = null;
	}
}
