using System;
using Terraria;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems.Achievement;

namespace BossRush.Common.Systems.IOhandle;

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
				FieldInfo[] fields = typeof(RoguelikeData).GetFields(BindingFlags.Static | BindingFlags.Public);
				foreach (var field in fields) {
					if (!tag.ContainsKey(field.Name)) {
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
		if (!File.Exists(DataFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}
			File.Create(DataFilePath);
		}
		try {
			TagCompound tag = new();
			FieldInfo[] fields = typeof(RoguelikeData).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var field in fields) {
				tag.Set(field.Name, field.GetValue(null));
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
	private void On_Main_Main_Exiting(On_Main.orig_Main_Exiting orig, Main self, object sender, EventArgs e) {
		if (!File.Exists(DataFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}
			File.Create(DataFilePath);
		}
		try {
			TagCompound tag = new();
			FieldInfo[] fields = typeof(RoguelikeData).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var field in fields) {
				tag.Set(field.Name, field.GetValue(null));
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
