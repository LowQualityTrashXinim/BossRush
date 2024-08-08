using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace BossRush;

public partial class BossRush : Mod {
	public static string ModFilePath { get; private set; }
	public const string AchievementFileName = "\\AchievementData.json";
	public const string ModDataFileName = "\\ModData.json";
	public override void PostSetupContent() {
		LoadModData();
	}
	public override void Load() {
		base.Load();
		ModFilePath = GeneratePathToModData();
	}
	public string GeneratePathToModData() {
		string autoPathfinding = Program.SavePathShared;
		autoPathfinding += "\\RogueLikeData";
		return autoPathfinding;
	}
	private void LoadModData() {
		//try {
		//	BossRushModSystem.roguelikedata = new RogueLikeData();
		//	string json = JsonConvert.SerializeObject(BossRushModSystem.roguelikedata, Formatting.Indented);
		//	if (File.Exists(ModFilePath + ModDataFileName)) {
		//		using (StreamWriter sw = File.CreateText(ModFilePath + ModDataFileName)) {
		//			sw.WriteLine();
		//		}
		//	}
		//	else {
		//		Directory.CreateDirectory(ModFilePath).Create();
		//		using (StreamWriter sw = File.CreateText(ModFilePath + ModDataFileName)) {
		//			sw.WriteLine();
		//		}
		//	}
		//}
		//catch (Exception ex) {
		//	Console.WriteLine(ex);
		//	Logger.Error(ex);
		//}
	}
}
public class RogueLikeData {
	public int AmountOfRun = 0;
	public int AmountOfLootBoxOpen = 0;
	public List<int> SynergyItemTouch = new List<int>();
}
public class BossRushModSystem : ModSystem {
	public static RogueLikeData roguelikedata;
	public override void OnModLoad() {
		roguelikedata = new RogueLikeData();
		_synergyitem = new List<Item>();
		_lostAccs = new List<Item>();
	}
	public override void OnWorldUnload() {
		roguelikedata.AmountOfLootBoxOpen += Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
	}
	public override void OnModUnload() {
		roguelikedata = null;
		_synergyitem = null;
		_lostAccs = null;
	}
	public int AmountOfLootboxOpenInCurrentSection() {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			return Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
		}
		return -1;
	}
	public override void PostSetupContent() {
		_synergyitem = new List<Item>();
		_lostAccs = new List<Item>();
		List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
		for (int i = 0; i < cacheitemList.Count; i++) {
			if (cacheitemList[i].ModItem is SynergyModItem) {
				_synergyitem.Add(cacheitemList[i]);
			}
			if (cacheitemList[i].TryGetGlobalItem<GlobalItemHandle>(out GlobalItemHandle globalItem)) {
				if (globalItem.LostAccessories) {
					_lostAccs.Add(cacheitemList[i]);
				}
			}
		}
	}
	private static List<Item> _synergyitem;
	public static List<Item> SynergyItem => _synergyitem;
	private static List<Item> _lostAccs;
	public static List<Item> LostAccessories => _lostAccs;
}
// public static class AchievementLoader {
// 	public static readonly Dictionary<string, ModAchivement> Achievement = new();
// 	public static readonly List<string> AchievementName = new();
// 	public static int TotalCount => Achievement.Count;
// 	public static void Register(ModAchivement achieve) {
// 		Achievement.Add(achieve.Name, achieve);
// 		AchievementName.Add(achieve.Name);
// 	}
// 	public static ModAchivement GetAchievement(string type) {
// 		Achievement.TryGetValue(type, out ModAchivement value);
// 		return value;
// 	}
// }
