using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Common.Systems;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.aDebugItem.SkillDebug;
internal class SkillGetter : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
	}
	public override bool AltFunctionUse(Player player) {
		return true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateDebugUI("skill");
		}
		return base.UseItem(player);
	}
}
class btn_Skill : UIImageButton {
	public int ModSkillID = -1;
	Texture2D texture = null;
	public void ChangeModSKillID(int newID) {
		ModSkillID = Math.Clamp(newID, 0, SkillModSystem.TotalCount);
	}
	public btn_Skill(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Vector2 drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
		if (ModSkillID < 0 || ModSkillID >= SkillModSystem.TotalCount) {
			return;
		}
		Texture2D skilltexture = ModContent.Request<Texture2D>(SkillModSystem.GetSkill(ModSkillID).Texture).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
class SkillGetterUI : UIState {
	UIImageButton btn_select;
	UIPanel panel;
	int currentSelectTemplate = -1;

	private int linePosition;
	List<btn_Skill> btn_list;
	public const int SKILL_MAXLINE = 10;
	public override void OnInitialize() {
		btn_select = new UIImageButton(TextureAssets.InventoryBack);
		btn_select.HAlign = .65f;
		btn_select.VAlign = .5f;
		btn_select.OnLeftClick += Btn_select_OnLeftClick;
		btn_select.OnUpdate += Btn_select_OnUpdate;
		Append(btn_select);

		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(500, 500);
		Append(panel);

		btn_list = new();
	}
	public override void OnActivate() {
		btn_list.Clear();
		panel.RemoveAllChildren();
		int length = SkillModSystem.TotalCount;
		int lineCounter = 0;
		for (int i = 0; i < length; i++) {
			if (i % SKILL_MAXLINE == 0) {
				lineCounter++;
			}


			btn_Skill button = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)) {
				Width = StyleDimension.FromPixels(44f),
				Height = StyleDimension.FromPixels(44f),
				Left = StyleDimension.FromPixels((i % SKILL_MAXLINE) * 46.0f + 6.0f),
				Top = StyleDimension.FromPixels((i / SKILL_MAXLINE) * 48.0f + 1.0f)
			};
			button.ModSkillID = i;
			button.OnLeftClick += Text_OnLeftClick;
			button.OnUpdate += Text_OnUpdate;
			btn_list.Add(button);
			panel.Append(button);

		}
	}

	private void Btn_select_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Btn_select_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		btn_Skill text = btn_list.Where(i => i.UniqueId == currentSelectTemplate).FirstOrDefault();
		if (text == null) {
			return;
		}
		ModSkill template = SkillModSystem.GetSkill(btn_list.IndexOf(text));
		if (template == null) {
			return;
		}
		Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(template.Type);
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}

	public override void ScrollWheel(UIScrollWheelEvent evt) {
		linePosition -= MathF.Sign(evt.ScrollWheelValue);
		int offsetvalue = linePosition * SKILL_MAXLINE;
		int length = SkillModSystem.TotalCount;
		int offsetlength = length - offsetvalue;
		for (int i = 0; i < length; i++) {
			if (i < 0 || i >= btn_list.Count) {
				continue;
			}
			int arty = Math.Clamp(i + offsetvalue, 0, length - 1);
			btn_list[i].ChangeModSKillID(-1);
			if (i > offsetlength) {
				continue;
			}
			btn_list[i].ChangeModSKillID(arty);
		}
	}
	private void Text_OnUpdate(UIElement affectedElement) {
		if (affectedElement.IsMouseHovering) {
			btn_Skill text = btn_list.Where(i => i.UniqueId == affectedElement.UniqueId).FirstOrDefault();
			if (text == null) {
				return;
			}
			ModSkill template = SkillModSystem.GetSkill(btn_list.IndexOf(text));
			if (template == null) {
				return;
			}
			Main.instance.MouseText(template.DisplayName);
		}
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	private void Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		currentSelectTemplate = listeningElement.UniqueId;
	}
}
