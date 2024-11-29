using System.IO;
using System.Text;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace BossRush.Common;
public class RogueLikeWorldGenSystem : ModSystem {
	public List<GenPassData> list_genPass = new();
	public Dictionary<string, List<GenPassData>> dict_Struture = new();
	public const string FileDestination = "Assets/TestFormat/";
	public override void PostSetupContent() {
		Stopwatch watch = new();
		try {
			watch.Start();
			string fileName = "";
			List<GenPassData> list_genPass = new();
			foreach (string filenamepath in Mod.GetFileNames()) {
				if (!filenamepath.StartsWith(FileDestination)) {
					continue;
				}
				StringBuilder strbld = new StringBuilder();
				strbld.AppendLine(filenamepath);
				strbld.Remove(0, FileDestination.Length);
				fileName = strbld.ToString();
				strbld.Remove(fileName.Length - 6, 6);
				fileName = strbld.ToString();
				strbld.Clear();
				Stream filepath = Mod.GetFileStream(filenamepath);
				int currentchar = 0;
				ushort amount = 1;
				TileData tile = TileData.Default;
				using StreamReader r = new StreamReader(filepath);
				while (currentchar != -1) {
					currentchar = r.Read();
					char c = (char)currentchar;
					//This mean the upcoming next tile data is definitely gonna be number or another new tile data
					if (c == '}') {
						currentchar = r.Read();
						if (currentchar == -1) {
							break;
						}
						//We are reading new tile data, as such this mean that previous tile data only have 1
						//So we are creating a new genpass with count of amount
						c = (char)currentchar;
						if (c == '{') {
							if (strbld.Length > 0) {
								list_genPass.Add(new(new(strbld.ToString()), amount));
							}
							strbld.Clear();
							amount = 1;
							continue;
						}
						//This mean there are multiple of said tile above
						//Which mean we should just create a new tile datat and then set count to it after we retrieve all the needed amount
						else {
							tile = new(strbld.ToString());
							strbld.Clear();
							strbld.Append(c);
							continue;
						}
					}
					//This mean we are entering a new tile data
					if (c == '{') {
						//Check in case the previous check if tile data is present or not
						if (!tile.Equals(TileData.Default)) {
							amount = ushort.Parse(strbld.ToString());
							list_genPass.Add(new(tile, amount));
							tile = TileData.Default;
						}
						amount = 1;
						strbld.Clear();
						continue;
					}
					if (currentchar != -1)
						strbld.Append(c);
				}
				if (strbld.Length > 0) {
					if (ushort.TryParse(strbld.ToString(), out ushort result)) {
						list_genPass.Add(new(tile, result));
					}
				}
				dict_Struture.Add(fileName, new(list_genPass));
				list_genPass.Clear();
			}
		}
		catch {

		}
		finally {
			watch.Stop();
			string result = $"Time it take to load structure dictionary : {watch.ToString()}";
			Mod.Logger.Info(result);
			Console.WriteLine(result);
		}
	}
}
public class GenPassData {
	public TileData tileData { get; private set; }
	public ushort Count { get; private set; }
	public ushort CountX { get; private set; }
	public ushort CountY { get; private set; }
	public GenPassData() {

	}
	public GenPassData(TileData tileData, ushort count) {
		this.tileData = tileData;
		Count = count;
	}
	public void Default_Set(TileData data, ushort count) {
		this.Count = count;
		this.tileData = data;
	}
	public void Rect_Set(TileData data, ushort countX, ushort countY) {
		this.CountX = countX;
		this.CountY = countY;
		this.tileData = data;
	}
	public void Clear() {
		this.Count = 0;
		this.CountX = 0;
		this.CountY = 0;
		this.tileData = new();
	}
}
