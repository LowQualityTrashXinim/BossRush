using BossRush.Achievement;
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

namespace BossRush {
	public partial class BossRush : Mod {
		public static string ModFilePath { get; private set; }
		public const string AchievementFileName = "\\AchievementData.json";
		public const string ModDataFileName = "\\ModData.json";
		private static List<Item> _synergyitem;
		public static List<Item> SynergyItem => _synergyitem;
		public override void PostSetupContent() {
			LoadAchievementData();
			LoadModData();
			_synergyitem = ContentSamples.ItemsByType.Values.Where(i => i.ModItem is SynergyModItem).ToList();
		}
		public override void Load() {
			base.Load();
			ModFilePath = GeneratePathToModData();
			On_Main.Main_Exiting += On_Main_Main_Exiting;
		}

		private void On_Main_Main_Exiting(On_Main.orig_Main_Exiting orig, Main self, object sender, EventArgs e) {
			SaveAchievementData();
			orig(self, sender, e);
		}

		public override void Unload() {
			_synergyitem = null;
		}
		public string GeneratePathToModData() {
			string autoPathfinding = Program.SavePathShared;
			autoPathfinding += "\\RogueLikeData";
			return autoPathfinding;
		}
		private void LoadAchievementData() {
			try {
				string json = JsonConvert.SerializeObject(AchievementLoader.Achievement, Formatting.Indented);
				if (File.Exists(ModFilePath + AchievementFileName)) {

					string jsondata = File.ReadAllText(ModFilePath + AchievementFileName);
					dynamic jsonObj = JsonConvert.DeserializeObject(jsondata);

					for (int i = 0; i < AchievementLoader.TotalCount; i++) {
						AchievementLoader.Achievement[AchievementLoader.AchievementName[i]].Condition = jsonObj[AchievementLoader.AchievementName[i]]["Condition"];
					}
				}
				else {
					//This is when we know that player are on a new run
					Directory.CreateDirectory(ModFilePath).Create();
					using (StreamWriter sw = File.CreateText(ModFilePath + AchievementFileName)) {
						sw.WriteLine(json);
					}
				}
			}
			catch (Exception ex) {
				Console.WriteLine(ex);
				Logger.Error(ex);
			}
		}
		private void SaveAchievementData() {
			string jsondata = File.ReadAllText(ModFilePath + AchievementFileName);
			dynamic jsonObj = JsonConvert.DeserializeObject(jsondata);
			for (int i = 0; i < AchievementLoader.TotalCount; i++) {
				jsonObj[AchievementLoader.AchievementName[i]]["Condition"] = AchievementLoader.Achievement[AchievementLoader.AchievementName[i]].Condition;
			}
			string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
			File.WriteAllText(ModFilePath + AchievementFileName, output);
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
		}
		public override void OnWorldUnload() {
			roguelikedata.AmountOfLootBoxOpen += Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
		}
		public override void OnModUnload() {
			roguelikedata = null;
		}
		public override void PostUpdateEverything() {
			foreach (var achieve in AchievementLoader.Achievement.Values) {
				bool condition = achieve.ConditionCheck();
				if (condition) {
					achieve.Condition = true;
				}
			}
		}
		public int AmountOfLootboxOpenInCurrentSection() {
			if (Main.netMode == NetmodeID.SinglePlayer) {
				return Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
			}
			return -1;
		}
	}
	public static class AchievementLoader {
		public static readonly Dictionary<string, ModAchivement> Achievement = new();
		public static readonly List<string> AchievementName = new();
		public static int TotalCount => Achievement.Count;
		public static void Register(ModAchivement achieve) {
			Achievement.Add(achieve.Name, achieve);
			AchievementName.Add(achieve.Name);
		}
		public static ModAchivement GetAchievement(string type) {
			Achievement.TryGetValue(type, out ModAchivement value);
			return value;
		}
	}
	/// <summary>
	/// This should and will be run on client side only, this should never work in multiplayer no matter what
	/// </summary>
	public abstract class ModAchivement : ILoadable, IEquatable<ModAchivement>, IComparable<ModAchivement> {
		public bool Condition = false;
		[JsonIgnore]
		public int Type;
		[JsonIgnore]
		public string Name => GetType().Name;
		[JsonIgnore]
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.DisplayName");
		[JsonIgnore]
		public string Description => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Description");
		[JsonIgnore]
		public string ConditionTip => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTip");
		[JsonIgnore]
		public string ConditionTipAfterAchieve => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTipAfterAchieve");
		[JsonIgnore]
		public string textureString = BossRushTexture.MISSINGTEXTURE;
		void ILoadable.Load(Mod mod) {
			Register();
		}
		protected void Register() {
			AchievementLoader.Register(this);
			SetDefault();
		}
		public void Unload() {
			textureString = null;
		}
		protected virtual void SetDefault() { }
		public virtual bool ConditionCheck() => false;

		public int CompareTo(ModAchivement other) {
			return other == null ? 1 : Type.CompareTo(other.Type);
		}
		public bool Equals(ModAchivement other) {
			return other == null ? false : Type.Equals(other.Type);
		}
	}
}
