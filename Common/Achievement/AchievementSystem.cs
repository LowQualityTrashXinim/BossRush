using BossRush.Texture;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossRush.Common.Achievement;
internal class AchievementSystem : ModSystem {
	public static string ModFilePath { get; private set; }
	public const string AchievementFileName = "\\AchievementData.json";
	public const string ModDataFileName = "\\ModData.json";
	public override void PostSetupContent() {
		LoadAchievementData();
		LoadModData();
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
			Mod.Logger.Error(ex);
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
