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
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace BossRush.Common.Systems.Achievement;
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
public class AchievementUI : UIState {
	private const int Row = 10;
	UIPanel mainPanel, headerPanel;
	Roguelike_WrapTextUIPanel textpanel;
	Roguelike_WrapTextUIPanel conditiontextpanel;
	List<AchievementButton> btn_Achievement;
	ExitUI exitbtn;
	private int RowOffSet = 0;
	public static string ActiveAchievement = "";
	public override void OnInitialize() {
		mainPanel = new UIPanel();
		mainPanel.HAlign = .35f;
		mainPanel.VAlign = .5f;
		mainPanel.UISetWidthHeight(100, 600);
		Append(mainPanel);

		textpanel = new Roguelike_WrapTextUIPanel("");
		textpanel.HAlign = .53f;
		textpanel.VAlign = .5f;
		textpanel.UISetWidthHeight(450, 600);
		textpanel.offSetDraw.Y += 75;
		Append(textpanel);

		headerPanel = new UIPanel();
		headerPanel.UISetWidthHeight(450, 72);
		textpanel.Append(headerPanel);

		exitbtn = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		exitbtn.UISetWidthHeight(52, 52);
		exitbtn.HAlign = 1f;
		exitbtn.VAlign = .5f;
		headerPanel.Append(exitbtn);

		btn_Achievement = new();
		for (int i = 0; i < Row; i++) {
			ModAchievement achievement = AchievementSystem.SafeGetAchievement(i);
			string text = "";
			if (achievement != null) {
				text = achievement.Name;
			}
			AchievementButton btn = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), text);
			btn.HAlign = .5f;
			btn.VAlign = MathHelper.Lerp(0f, 1f, i / (Row - 1f));
			btn.UISetWidthHeight(52, 52);
			btn_Achievement.Add(btn);
			mainPanel.Append(btn);
		}

		conditiontextpanel = new Roguelike_WrapTextUIPanel("", .77f);
		conditiontextpanel.HAlign = .1f;
		conditiontextpanel.VAlign = 1f;
		conditiontextpanel.UISetWidthHeight(450, 100);
		textpanel.Append(conditiontextpanel);
	}
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		RowOffSet -= MathF.Sign(evt.ScrollWheelValue);
		RowOffSet = Math.Clamp(RowOffSet, 0, Math.Max(AchievementSystem.Achievements.Count, Row));

		for (int i = 0; i < AchievementSystem.Achievements.Count; i++) {
			if (i >= btn_Achievement.Count) {
				break;
			}
			btn_Achievement[i].SetAchievement("");
			if (i + RowOffSet >= AchievementSystem.Achievements.Count) {
				continue;
			}
			btn_Achievement[i].SetAchievement(AchievementSystem.Achievements[i + RowOffSet].Name);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ActiveAchievement == "") {
			return;
		}
		ModAchievement achievement = AchievementSystem.GetAchievement(ActiveAchievement);
		if (achievement == null) {
			return;
		}
		string text = $"Description : {achievement.Description}";
		if (achievement.AdditionalConditionTipAfterAchieve && achievement.Achieved) {
			conditiontextpanel.SetText("Condition: " + achievement.ConditionTipAfterAchieve);
		}
		else {
			conditiontextpanel.SetText("Condition: " + achievement.ConditionTip);
		}
		text += "\nStatus : ";
		if (achievement.Achieved) {
			text += "Completed";
		}
		else {
			text += "Unfinished";
		}
		textpanel.SetText(text);
	}
}
public class AchievementButton : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
		this.SetVisibility(.5f, .5f);
	}
	public AchievementButton(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void LeftClick(UIMouseEvent evt) {
		AchievementUI.ActiveAchievement = achievementname;
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			if (achievement.Achieved) {
				this.SetVisibility(1f, 1f);
			}
		}
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			Main.instance.MouseText(achievementname);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		string texturestring;
		bool checkAchievement = achievement != null;
		if (checkAchievement) {
			texturestring = achievement.Texture;
			if (achievement.SpecialDraw) {
				achievement.Draw(this, spriteBatch);
				if (!achievement.Achieved) {
					Texture2D locktex = Lock.Value;
					Vector2 origin2 = locktex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(locktex, drawpos2, null, new Color(255, 255, 255) * .45f, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				return;
			}
		}
		else {
			texturestring = BossRushTexture.ACCESSORIESSLOT;
		}
		Texture2D skilltexture = ModContent.Request<Texture2D>(texturestring).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(texture.Size(), skilltexture.Size());
		Vector2 drawpos = this.GetDimensions().Position() + texture.Size() * .5f;
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (checkAchievement) {
			if (!achievement.Achieved) {
				Texture2D locktex = Lock.Value;
				origin = locktex.Size() * .5f;
				spriteBatch.Draw(locktex, drawpos, null, new Color(255, 255, 255) * .45f, 0, origin, .9f, SpriteEffects.None, 0);
			}
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
