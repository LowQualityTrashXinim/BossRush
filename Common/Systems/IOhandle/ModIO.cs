using System;
using Terraria;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems.Achievement;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Common.Systems;
public static class RoguelikeData {
	public static int Lootbox_AmountOpen = 0;
	public static int Run_Amount = 0;
	public static int EliteKill = 0;
	//currently there are no winning goal so this is not implemented
	public static int Win_Streak = 0;
	public static int Win_StreakRecord = 0;
	//This however can be implemented
	public static int Lose_Streak = 0;
	public static int Lose_StreakRecord = 0;
	/// <summary>
	/// Key : The inner name of the synergy weapon<br/>
	/// Value : Synergy bonus value<br>
	/// <br/>
	/// <b></b>
	/// </summary>
	public static Dictionary<string, List<SynergyBonus>> SynergyProgressTracker = new();
}
class ModIO : ModSystem {
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "RogueLikeData");
	private static string DataFilePath => Path.Join(DirectoryPath, "Data");
	private static string AchievementFilePath => Path.Join(DirectoryPath, "Achievements");
	public override void Load() {
		foreach (var type in Mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(ModAchievement)))) {
			var achievement = (ModAchievement)Activator.CreateInstance(type);
			AchievementSystem.Achievements.Add(achievement);
		}
		try {
			if (File.Exists(DataFilePath)) {
				var tag = TagIO.FromFile(DataFilePath);
				Type type = typeof(RoguelikeData);
				FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
				foreach (var field in fields) {
					if (!tag.ContainsKey(field.Name)) {
						object obj = field.GetValue(null);
						if (obj is Dictionary<string, List<SynergyBonus>> tracker) {
							var keyObj = tag.Get<List<string>>("SynergyProgressTracker_Key");
							var valueObj = tag.Get<List<List<SynergyBonus>>>("SynergyProgressTracker_Value");
							tracker = keyObj.Zip(valueObj, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
							field.SetValue(null, tracker);
						}
						continue;
					}
					field.SetValue(null, tag[field.Name]);
				}
			}
			if (File.Exists(AchievementFilePath)) {
				var tag = TagIO.FromFile(AchievementFilePath);
				foreach (var achievement in AchievementSystem.Achievements) {
					if (tag.ContainsKey(achievement.Name)) {
						achievement.Achieved = true;
					}
				}
			}
		}
		catch {

		}
		On_Main.Main_Exiting += On_Main_Main_Exiting;
	}
	public override void Unload() {
		SavingModData();
	}
	private void On_Main_Main_Exiting(On_Main.orig_Main_Exiting orig, Main self, object sender, EventArgs e) {
		SavingModData();
	}
	public void SavingModData() {
		if (!File.Exists(DataFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}
			File.Create(DataFilePath);
		}
		try {
			TagCompound tag = new();
			Type type = typeof(RoguelikeData);
			FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var field in fields) {
				object objValue = field.GetValue(null);
				if (objValue is Dictionary<string, List<SynergyBonus>> bonus) {
					tag.Set(field.Name + "_Key", bonus.Keys.ToList());
					tag.Set(field.Name + "_Value", bonus.Values.ToList());
				}
				else {
					tag.Set(field.Name, field.GetValue(null));
				}
			}
			TagIO.ToFile(tag, DataFilePath);
		}
		catch {

		}

		var achievementTag = new TagCompound();
		foreach (var achievement in AchievementSystem.Achievements) {
			if (achievement.Achieved) {
				achievementTag.Set(achievement.Name, 0);
			}
		}
		if (!File.Exists(AchievementFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}

			File.Create(AchievementFilePath);
		}
		try {
			TagIO.ToFile(achievementTag, AchievementFilePath);
		}
		catch {

		}
	}
}
