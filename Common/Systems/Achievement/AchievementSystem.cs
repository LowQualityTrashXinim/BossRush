using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace BossRush.Common.Systems.Achievement;
/// <summary>
/// This should and will be run on client side only, this should never work in multiplayer no matter what
/// </summary>
public abstract class ModAchievement {
	public bool Achieved { get; set; }
	public bool AdditionalConditionTipAfterAchieve = false;
	public virtual string Texture => BossRushTexture.ACCESSORIESSLOT;
	public virtual bool SpecialDraw => false;
	public virtual bool CanSeeReward => true;
	public virtual void Draw(UIElement element, SpriteBatch spriteBatch) { }
	public string Name => GetType().Name;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Description");
	public string ConditionTip => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTip");
	public string ConditionTipAfterAchieve => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTipAfterAchieve");
	public string Reward => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Reward");

	public virtual void SetStaticDefault() { }
	public virtual bool Condition() {
		return false;
	}
	public virtual void SpecialEffectOnAchieved() { }
}

public class AchievementSystem : ModSystem {
	public static readonly List<ModAchievement> Achievements = [];
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "RogueLikeData");
	private static string FilePath => Path.Join(DirectoryPath, "Achievements");
	public static ModAchievement SafeGetAchievement(int type) => Achievements.Count > type && type >= 0 ? Achievements[type] : null;
	public static ModAchievement GetAchievement(string achievementName) => Achievements.Where(achieve => achieve.Name == achievementName).FirstOrDefault();
	public static bool IsAchieved(string AchievementName) => GetAchievement(AchievementName).Achieved;
	public override void PostSetupContent() {
		foreach (var item in Achievements) {
			item.SetStaticDefault();
		}
	}
	public override void Load() {
		// Loading achievements
		foreach (var type in Mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(ModAchievement)))) {
			var achievement = (ModAchievement)Activator.CreateInstance(type);
			Achievements.Add(achievement);
		}

		try {
			if (File.Exists(FilePath)) {
				var tag = TagIO.FromFile(FilePath);
				foreach (var achievement in Achievements) {
					if (tag.ContainsKey(achievement.Name)) {
						achievement.Achieved = true;
					}
				}
			}
		}
		catch {

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
		try {
			TagIO.ToFile(tag, FilePath);
		}
		catch {

		}
	}

	public override void PostUpdateEverything() {
		foreach (var achievement in Achievements) {
			if (achievement.Condition()) {
				achievement.Achieved = true;
			}
		}
	}

}
