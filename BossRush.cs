using BossRush.Achievement;
using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace BossRush {
	public partial class BossRush : Mod {
		public static string AchievementFilePath { get; private set; }
		public const string AchievementFileName = "\\AchievementData.json";
		public override void PostSetupContent() {
			LoadAchievementData();
		}
		public override void Load() {
			base.Load();
			AchievementFilePath = GeneratePathToAchievement();
			On_Main.Main_Exiting += On_Main_Main_Exiting;
		}

		private void On_Main_Main_Exiting(On_Main.orig_Main_Exiting orig, Main self, object sender, EventArgs e) {
			string jsondata = File.ReadAllText(AchievementFilePath + AchievementFileName);
			dynamic jsonObj = JsonConvert.DeserializeObject(jsondata);
			for (int i = 0; i < AchievementLoader.TotalCount; i++) {
				jsonObj[i]["Condition"] = AchievementLoader.Achievement[i].Condition;
			}
			string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
			File.WriteAllText(AchievementFilePath + AchievementFileName, output);
			orig(self, sender, e);
		}

		public override void Unload() {
		}
		public string GeneratePathToAchievement() {
			string autoPathfinding = Program.SavePathShared;
			autoPathfinding += "\\RogueLikeData";
			return autoPathfinding;
		}
		private void LoadAchievementData() {
			AchievementLoader.Achievement.Sort();
			try {
				string json = JsonConvert.SerializeObject(AchievementLoader.Achievement, Formatting.Indented);
				if (File.Exists(AchievementFilePath + AchievementFileName)) {
					//it is required that we should check the data in the file first
					//using (StreamWriter sw = File.CreateText(AchievementFilePath + AchievementFileName)) {
					//	sw.WriteLine(json);
					//}
					string jsondata = File.ReadAllText(AchievementFilePath + AchievementFileName);
					dynamic jsonObj = JsonConvert.DeserializeObject(jsondata);
					
					//JArray jObj = (JArray)JsonConvert.DeserializeObject(jsondata);
					//int count = jObj.Count;

					for (int i = 0; i < AchievementLoader.TotalCount; i++) {
						AchievementLoader.Achievement[i].Condition = jsonObj[i]["Condition"];
					}
				}
				else {
					//This is when we know that player are on a new run
					Directory.CreateDirectory(AchievementFilePath).Create();
					using (StreamWriter sw = File.CreateText(AchievementFilePath + AchievementFileName)) {
						sw.WriteLine(json);
					}
				}
			}
			catch (Exception ex) {
				Console.WriteLine(ex);
			}
		}
		public static void SaveAchievementData() {
		}
	}
	public class BossRushModSystem : ModSystem {
		public static int AmountOfLootBoxOpenDuringThisSection = 0;
		public override void OnWorldLoad() {
			base.OnWorldLoad();
		}
		public override void OnWorldUnload() {
			AmountOfLootBoxOpenDuringThisSection += Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
		}
		public override void OnModLoad() {
		}
		public override void OnModUnload() {
			AmountOfLootBoxOpenDuringThisSection = 0;
		}
		public override void Unload() {
		}
		public override void PostUpdateEverything() {
			foreach (var achieve in AchievementLoader.Achievement) {
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
		public static readonly List<ModAchivement> Achievement = new();
		public static int TotalCount => Achievement.Count;
		public static void Register(ModAchivement achieve) {
			Achievement.Add(achieve);
		}
		public static ModAchivement GetAchievement(int type) {
			return type >= 0 && type < Achievement.Count ? Achievement[type] : null;
		}
	}
	/// <summary>
	/// This should and will be run on client side only, this should never work in multiplayer no matter what
	/// </summary>
	public abstract class ModAchivement : ILoadable, IEquatable<ModAchivement>, IComparable<ModAchivement> {
		[JsonIgnore]
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.DisplayName");
		public bool Condition = false;
		public int Type;
		public string Name => GetType().Name;
		[JsonIgnore]
		public string Description => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Description");
		[JsonIgnore]
		public string ConditionTip => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTip");
		[JsonIgnore]
		public string ConditionTipAfterAchieve => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTipAfterAchieve");
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
		[JsonIgnore]
		public string textureString = BossRushTexture.MISSINGTEXTURE;
		public virtual bool ConditionCheck() => false;

		public int CompareTo(ModAchivement other) {
			return other == null ? 1 : Type.CompareTo(other.Type);
		}
		public bool Equals(ModAchivement other) {
			return other == null ? false : Type.Equals(other.Type);
		}
	}
}
