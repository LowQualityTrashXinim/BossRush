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

namespace BossRush.Common.Systems.Achievement;
public enum AchievementTag : byte {
	None,
	Tutorial,
	Easy,
	Hard,
	BossRush,
	Mastery,
	Challenge,
	Misc
}
/// <summary>
/// This should and will be run on client side only, this should never work in multiplayer no matter what
/// </summary>
public abstract class ModAchievement {
	public AchievementTag CategoryTag = AchievementTag.None;
	public AchievementTag DifficultyTag = AchievementTag.None;
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
				if (!achievement.Achieved) {
					achievement.SpecialEffectOnAchieved();
					achievement.Achieved = true;
				}
			}
		}
	}

}
public class AchievementUI : UIState {
	private const int Row = 10;
	UIPanel achievementSelectingPanel, headerPanel;
	Roguelike_WrapTextUIPanel textpanel_main;
	Roguelike_WrapTextUIPanel textpanel_bottom;
	public List<AchievementButton> btn_Achievement;
	public List<UITextPanel<string>> txt_Achievement;
	ExitUI exitbtn;
	public static string ActiveAchievement = "";
	public int State = 0;
	public int CurrentSelectedIndex = -1;
	UIPanel main;
	Roguelike_UIImageButton buttonLeft;
	Roguelike_UIImageButton buttonRight;
	UIPanel footerPanel;
	List<PageImage> pagnitation = new();
	TagPanel tagTutorial;
	TagPanel tagEasy;
	TagPanel tagHard;
	TagPanel tagBossRush;
	TagPanel tagMastery;
	TagPanel tagChallenge;
	TagPanel tagMisc;
	HashSet<AchievementTag> hash_tag = new();
	Asset<Texture2D> asset = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
	List<ModAchievement> lib_achievement = new();
	UIPanel currentSelectAchievement;
	AchievementHeaderPreview achievementheader;
	UITextPanel<string> txtachievementheader;
	public override void OnInitialize() {
		asset = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
		hash_tag = new();
		lib_achievement = new();

		main = new();
		main.HAlign = .5f;
		main.VAlign = .6f;
		main.UISetWidthHeight(700, 700);
		Append(main);

		headerPanel = new UIPanel();
		headerPanel.UISetWidthHeight(700, 100);
		headerPanel.HAlign = .5f;
		headerPanel.VAlign = .6f;
		headerPanel.MarginBottom = main.Height.Pixels + 80;
		Append(headerPanel);

		footerPanel = new();
		footerPanel.VAlign = 1;
		footerPanel.Width.Percent = 1;
		footerPanel.Height.Pixels = 72f;
		main.Append(footerPanel);

		textpanel_main = new Roguelike_WrapTextUIPanel("");
		textpanel_main.HAlign = 1;
		textpanel_main.VAlign = 1f;
		textpanel_main.MarginBottom = footerPanel.Height.Pixels + 5;
		textpanel_main.UISetWidthHeight(325, 510);
		main.Append(textpanel_main);

		currentSelectAchievement = new();
		currentSelectAchievement.HAlign = 1;
		currentSelectAchievement.MarginBottom = textpanel_main.Height.Pixels + 10;
		currentSelectAchievement.UISetWidthHeight(325, 80);
		main.Append(currentSelectAchievement);

		achievementheader = new(asset, "");
		achievementheader.UISetWidthHeight(52, 52);
		achievementheader.VAlign = .5f;
		currentSelectAchievement.Append(achievementheader);

		txtachievementheader = new("");
		txtachievementheader.VAlign = .5f;
		txtachievementheader.TextHAlign = .5f;
		txtachievementheader.MarginLeft = 60;
		txtachievementheader.Width.Precent = .8f;
		txtachievementheader.TextHAlign = .5f;
		txtachievementheader.TextScale = .8f;
		currentSelectAchievement.Append(txtachievementheader);

		achievementSelectingPanel = new UIPanel();
		achievementSelectingPanel.HAlign = 0;
		achievementSelectingPanel.VAlign = 0;
		achievementSelectingPanel.Width.Set(325, 0);
		achievementSelectingPanel.Height.Set(600, 0);
		achievementSelectingPanel.MarginRight = 100;
		main.Append(achievementSelectingPanel);

		exitbtn = new(asset);
		exitbtn.UISetWidthHeight(52, 52);
		exitbtn.HAlign = 1f;
		exitbtn.VAlign = .5f;
		headerPanel.Append(exitbtn);

		tagTutorial = new("Tutorial", .67f);
		tagTutorial.OnLeftClick += TagUniversal_OnLeftClick;
		tagTutorial.Width.Pixels = 80;
		headerPanel.Append(tagTutorial);

		tagEasy = new("Easy", .67f);
		tagEasy.HAlign = .15f;
		tagEasy.Width.Pixels = 80;
		tagEasy.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagEasy);

		tagHard = new("Hard", .67f);
		tagHard.HAlign = .3f;
		tagHard.Width.Pixels = 80;
		tagHard.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagHard);

		tagMastery = new("Mastery", .67f);
		tagMastery.HAlign = .45f;
		tagMastery.Width.Pixels = 80;
		tagMastery.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagMastery);

		tagBossRush = new("BossRush", .67f);
		tagBossRush.VAlign = 1f;
		tagBossRush.Width.Pixels = 80;
		tagBossRush.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagBossRush);

		tagChallenge = new("Challenge", .67f);
		tagChallenge.HAlign = .15f;
		tagChallenge.VAlign = 1;
		tagChallenge.Width.Pixels = 80;
		tagChallenge.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagChallenge);

		tagMisc = new("Misc", .67f);
		tagMisc.HAlign = .3f;
		tagMisc.VAlign = 1;
		tagMisc.Width.Pixels = 80;
		tagMisc.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagMisc);

		btn_Achievement = new();
		txt_Achievement = new();
		for (int i = 0; i < Row; i++) {
			AchievementButton btn = new(asset, "");
			btn.VAlign = MathHelper.Lerp(0f, 1f, i / (Row - 1f));
			btn.UISetWidthHeight(52, 52);
			btn_Achievement.Add(btn);
			achievementSelectingPanel.Append(btn);

			UITextPanel<string> txt_panel = new("");
			txt_panel.MarginLeft = 60;
			txt_panel.Width.Precent = .8f;
			txt_panel.TextHAlign = .5f;
			txt_panel.TextScale = .8f;
			txt_panel.VAlign = btn.VAlign;
			txt_Achievement.Add(txt_panel);
			achievementSelectingPanel.Append(txt_panel);
		}

		textpanel_bottom = new Roguelike_WrapTextUIPanel("", .77f);
		textpanel_bottom.HAlign = 1f;
		textpanel_bottom.UISetWidthHeight(325, 135);
		textpanel_bottom.VAlign = 1f;
		textpanel_main.Append(textpanel_bottom);

		buttonLeft = new(asset);
		buttonLeft.HAlign = 0;
		buttonLeft.VAlign = 1f;
		buttonLeft.postTex = ModContent.Request<Texture2D>(BossRushTexture.Arrow_Left);
		buttonLeft.OnLeftClick += ButtonLeft_OnLeftClick;
		footerPanel.Append(buttonLeft);

		buttonRight = new(asset);
		buttonRight.HAlign = 1f;
		buttonRight.VAlign = 1f;
		buttonRight.postTex = ModContent.Request<Texture2D>(BossRushTexture.Arrow_Right);
		buttonRight.OnLeftClick += ButtonRight_OnLeftClick;
		footerPanel.Append(buttonRight);

		pagnitation = new();

	}
	int pageIndex = 0;
	int maxPage = 1;
	public void SetPageIndex(int index) {
		pageIndex = Math.Clamp(index, 0, maxPage);
	}
	private void TagUniversal_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (tagTutorial.UniqueId == listeningElement.UniqueId) {
			tagTutorial.SetActive();
			if (!hash_tag.Add(AchievementTag.Tutorial)) {
				hash_tag.Remove(AchievementTag.Tutorial);
				tagTutorial.SetInactive();
			}
		}
		else if (tagEasy.UniqueId == listeningElement.UniqueId) {
			tagEasy.SetActive();
			if (!hash_tag.Add(AchievementTag.Easy)) {
				hash_tag.Remove(AchievementTag.Easy);
				tagEasy.SetInactive();
			}
		}
		else if (tagHard.UniqueId == listeningElement.UniqueId) {
			tagHard.SetActive();
			if (!hash_tag.Add(AchievementTag.Hard)) {
				hash_tag.Remove(AchievementTag.Hard);
				tagHard.SetInactive();
			}
		}
		else if (tagBossRush.UniqueId == listeningElement.UniqueId) {
			tagBossRush.SetActive();
			if (!hash_tag.Add(AchievementTag.BossRush)) {
				hash_tag.Remove(AchievementTag.BossRush);
				tagBossRush.SetInactive();
			}
		}
		else if (tagMastery.UniqueId == listeningElement.UniqueId) {
			tagMastery.SetActive();
			if (!hash_tag.Add(AchievementTag.Mastery)) {
				hash_tag.Remove(AchievementTag.Mastery);
				tagMastery.SetInactive();
			}
		}
		else if (tagChallenge.UniqueId == listeningElement.UniqueId) {
			tagChallenge.SetActive();
			if (!hash_tag.Add(AchievementTag.Challenge)) {
				hash_tag.Remove(AchievementTag.Challenge);
				tagChallenge.SetInactive();
			}
		}
		else if (tagMisc.UniqueId == listeningElement.UniqueId) {
			tagMisc.SetActive();
			if (!hash_tag.Add(AchievementTag.Misc)) {
				hash_tag.Remove(AchievementTag.Misc);
				tagMisc.SetInactive();
			}
		}
		lib_achievement.Clear();
		foreach (var item in AchievementSystem.Achievements) {
			if (hash_tag.Contains(item.DifficultyTag) || hash_tag.Contains(item.CategoryTag)) {
				lib_achievement.Add(item);
			}
		}

		for (int i = 0; i < btn_Achievement.Count; i++) {
			AchievementButton btn = btn_Achievement[i];
			if (lib_achievement.Count - 1 < i) {
				btn.achievementname = string.Empty;
				txt_Achievement[i].SetText("");
				continue;
			}
			ModAchievement achievement = lib_achievement[i];
			btn.SetAchievement(achievement.Name);
			txt_Achievement[i].SetText(achievement.DisplayName);
		}

		pageIndex = 0;
		maxPage = lib_achievement.Count / 10 + 1;
		foreach (var item in pagnitation) {
			footerPanel.RemoveChild(item);
		}
		pagnitation.Clear();
		float realpageamount = MathF.Ceiling(lib_achievement.Count / 10f);
		if (realpageamount <= 1) {
			return;
		}
		for (int i = 0; i < realpageamount; i++) {
			PageImage img = new(asset);
			if (maxPage == 1) {
				img.HAlign = .5f;
			}
			else {
				img.HAlign = MathHelper.Lerp(.1f, .9f, i / (realpageamount - 1f));
			}
			img.VAlign = .5f;
			img.OnLeftClick += Img_OnLeftClick;
			pagnitation.Add(img);
			footerPanel.Append(img);
		}
	}
	private void Img_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SetPageIndex(pagnitation.Select(el => el.UniqueId).ToList().IndexOf(listeningElement.UniqueId));
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}

	private void ButtonLeft_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex > 0) {
			pageIndex--;
		}
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}

	private void ButtonRight_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex < maxPage) {
			pageIndex++;
		}
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}
	public void RefleshAchievementSelectionUIBaseOnPageIndex() {
		if (pageIndex > maxPage || pageIndex < 0 || maxPage <= 1) {
			return;
		}
		int startingPoint = Row * pageIndex;
		for (int i = 0; i < btn_Achievement.Count; i++) {
			AchievementButton btn = btn_Achievement[i];
			int indexChecker = startingPoint + i;
			if (lib_achievement.Count - 1 < i || indexChecker >= lib_achievement.Count) {
				btn.achievementname = string.Empty;
				txt_Achievement[i].SetText("");
				continue;
			}
			ModAchievement achievement = lib_achievement[indexChecker];
			btn.SetAchievement(achievement.Name);
			txt_Achievement[i].SetText(achievement.DisplayName);
		}
	}

	public override void ScrollWheel(UIScrollWheelEvent evt) {
		//RowOffSet -= MathF.Sign(evt.ScrollWheelValue);
		//RowOffSet = Math.Clamp(RowOffSet, 0, Math.Max(AchievementSystem.Achievements.Count, Row));

		//for (int i = 0; i < AchievementSystem.Achievements.Count; i++) {
		//	if (i >= btn_Achievement.Count) {
		//		break;
		//	}
		//	btn_Achievement[i].SetAchievement("");
		//	if (i + RowOffSet >= AchievementSystem.Achievements.Count) {
		//		continue;
		//	}
		//	btn_Achievement[i].SetAchievement(AchievementSystem.Achievements[i + RowOffSet].Name);
		//}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		for (int i = 0; i < pagnitation.Count; i++) {
			var item = pagnitation[i];
			if (i == pageIndex) {
				item.toggled = true;
			}
			else {
				item.toggled = false;
			}
			if (item.IsMouseHovering) {
				Main.instance.MouseText("page " + (i + 1).ToString());
			}
		}
		if (tagTutorial.IsMouseHovering) {
			Main.instance.MouseText("Tutorial");
		}
		else if (tagEasy.IsMouseHovering) {
			Main.instance.MouseText("Easy");
		}
		else if (tagHard.IsMouseHovering) {
			Main.instance.MouseText("Hard");
		}
		else if (tagBossRush.IsMouseHovering) {
			Main.instance.MouseText("BossRush");
		}
		else if (tagMastery.IsMouseHovering) {
			Main.instance.MouseText("Mastery");
		}
		else if (tagChallenge.IsMouseHovering) {
			Main.instance.MouseText("Challenge");
		}
		else if (tagMisc.IsMouseHovering) {
			Main.instance.MouseText("Misc");
		}
		else if (buttonLeft.IsMouseHovering) {
			Main.instance.MouseText("Previous page");
		}
		else if (buttonRight.IsMouseHovering) {
			Main.instance.MouseText("Next page");
		}

		if (ActiveAchievement == "") {
			return;
		}
		//Main.NewText(CurrentSelectedIndex);
		ModAchievement achievement = AchievementSystem.GetAchievement(ActiveAchievement);
		if (achievement == null) {
			return;
		}
		//achievementName.SetText(achievement.DisplayName);
		string text = $"Description : {achievement.Description}";
		if (achievement.AdditionalConditionTipAfterAchieve && achievement.Achieved) {
			textpanel_bottom.SetText("Condition: " + achievement.ConditionTipAfterAchieve);
		}
		else {
			textpanel_bottom.SetText("Condition: " + achievement.ConditionTip);
		}
		text += "\nStatus : ";
		if (achievement.Achieved) {
			text += "Completed";
		}
		else {
			text += "Unfinished";
		}
		textpanel_main.SetText(text);
		achievementheader.SetAchievement(achievement.Name);
		txtachievementheader.SetText(achievement.DisplayName);
	}
}
public class TagPanel : UITextPanel<string> {
	public void SetActive() {
		this.BorderColor = Color.Yellow;
	}
	public void SetInactive() {
		this.BorderColor = Color.Black;
	}
	public TagPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}
}
public class PageImage : UIImage {
	public Asset<Texture2D> unselected;
	public Asset<Texture2D> selected;
	public bool CustomDraw = true;
	public bool toggled = false;
	public PageImage(Asset<Texture2D> texture) : base(texture) {
		unselected = ModContent.Request<Texture2D>(BossRushTexture.Page_StateUnselected);
		selected = ModContent.Request<Texture2D>(BossRushTexture.Page_StateSelected);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (!CustomDraw) {
			base.Draw(spriteBatch);
		}
		else {
			Vector2 originalorigin = new Vector2(26, 26);
			Vector2 drawpos = this.GetInnerDimensions().Position() + originalorigin * .5f + originalorigin * .25f;
			Vector2 origin;
			if (!toggled) {
				origin = unselected.Value.Size() * .5f;
				drawpos += origin * .5f;
				spriteBatch.Draw(unselected.Value, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
			}
			else {
				origin = selected.Value.Size() * .5f;
				drawpos += origin * .5f;
				spriteBatch.Draw(selected.Value, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
			}
		}
	}
}
public class AchievementHeaderPreview : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	Asset<Texture2D> achieved;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
	}
	public AchievementHeaderPreview(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		achieved = ModContent.Request<Texture2D>(BossRushTexture.CommonTextureStringPattern + "UI/complete");
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			this.SetVisibility(1f, 1f);
		}
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		string texturestring;
		bool checkAchievement = achievement != null;
		if (achievementname == string.Empty) {
			return;
		}
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
				else {
					Texture2D achievetex = achieved.Value;
					Vector2 origin2 = achievetex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(achievetex, drawpos2.Add(0, 3.5f), null, Color.White, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				return;
			}
		}
		else {
			texturestring = BossRushTexture.MissingTexture_Default;
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
			else {
				Texture2D achievetex = achieved.Value;
				origin = achievetex.Size() * .5f;
				spriteBatch.Draw(achievetex, drawpos.Add(0, 3.5f), null, Color.White, 0, origin, .9f, SpriteEffects.None, 0);
			}
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
public class AchievementButton : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	Asset<Texture2D> achieved;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
	}
	public AchievementButton(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		achieved = ModContent.Request<Texture2D>(BossRushTexture.CommonTextureStringPattern + "UI/complete");
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void LeftClick(UIMouseEvent evt) {
		AchievementUI.ActiveAchievement = achievementname;
		UniversalSystem uni = ModContent.GetInstance<UniversalSystem>();
		uni.achievementUI.State = 1;
		uni.achievementUI.CurrentSelectedIndex = uni.achievementUI.btn_Achievement.Select(el => el.UniqueId).ToList().IndexOf(UniqueId);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			this.SetVisibility(1f, 1f);
			if (IsMouseHovering) {
				Main.instance.MouseText(achievement.DisplayName);
			}
		}
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
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
		if (achievementname == string.Empty) {
			return;
		}
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
				else {
					Texture2D achievetex = achieved.Value;
					Vector2 origin2 = achievetex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(achievetex, drawpos2.Add(0, 3.5f), null, Color.White, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				return;
			}
		}
		else {
			texturestring = BossRushTexture.MissingTexture_Default;
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
			else {
				Texture2D achievetex = achieved.Value;
				origin = achievetex.Size() * .5f;
				spriteBatch.Draw(achievetex, drawpos.Add(0, 3.5f), null, Color.White, 0, origin, .9f, SpriteEffects.None, 0);
			}
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
