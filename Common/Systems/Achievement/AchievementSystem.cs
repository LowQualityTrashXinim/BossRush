using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Systems.Achievement;
public class AchievementSystem : ModSystem {
	public static readonly List<Achievement> Achievements = [];
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "RogueLikeData");
	private static string FilePath => Path.Join(DirectoryPath, "Achievements");

	public override void Load() {
		// Loading achievements
		foreach (var type in Mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(Achievement)))) {
			var achievement = (Achievement)Activator.CreateInstance(type);
			Achievements.Add(achievement);
		}

		if (File.Exists(FilePath)) {
			var tag = TagIO.FromFile(FilePath);
			foreach (var achievement in Achievements) {
				if (tag.ContainsKey(achievement.Name)) {
					achievement.Achieved = true;
				}
			}
		}
	}

	public override void Unload() {
		// Saving achievements
		var tag = new TagCompound();
		foreach (var achievement in Achievements) {
			if (achievement.Achieved) {
				tag.Set(achievement.Name, 0);
			}
		}

		if (!File.Exists(FilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}

			File.Create(FilePath);
		}

		TagIO.ToFile(tag, FilePath);
	}

	public override void PostUpdateEverything() {
		foreach (var achievement in Achievements) {
			if (achievement.Condition()) {
				achievement.Achieved = true;
			}
		}
	}
}
