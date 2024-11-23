using BossRush.Common.Systems;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Contents.Items.aDebugItem.RelicDebug;
internal class RelicTransmutator : ModItem {
	public override string Texture => BossRushTexture.EMPTYCARD;
	public override void SetDefaults() {
		Item.useTime = Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.width = Item.height = 30;
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateDebugUI();
		}
		return base.UseItem(player);
	}
}
class RelicTransmuteUI : UIState {
	UIImageButton btn_select;
	UIPanel panel;
	List<UIText> textlist;
	int currentSelectTemplate = -1;

	private int linePosition;
	private int maxLinePosition;
	private const int MAX_LINES = 6;
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

		textlist = new List<UIText>();
	}

	private void Btn_select_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Btn_select_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		UIText text = textlist.Where(i => i.UniqueId == currentSelectTemplate).FirstOrDefault();
		if (text == null) {
			return;
		}
		RelicTemplate template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
		if (template == null) {
			return;
		}
		Item item = Main.LocalPlayer.QuickSpawnItemDirect(Main.LocalPlayer.GetSource_FromThis(), ModContent.ItemType<Relic>());
		if (item.ModItem is Relic relic) {
			relic.AddRelicTemplate(Main.LocalPlayer, template.Type);
		}
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}

	public override void ScrollWheel(UIScrollWheelEvent evt) {
		linePosition -= MathF.Sign(evt.ScrollWheelValue);
	}
	public override void OnActivate() {
		textlist.Clear();
		panel.RemoveAllChildren();
		for (int i = 0; i < RelicTemplateLoader.TotalCount; i++) {
			UIText text = new UIText(RelicTemplateLoader.GetTemplate(i).Name);
			text.OnLeftClick += Text_OnLeftClick;
			text.OnUpdate += Text_OnUpdate;
			text.VAlign = i / (RelicTemplateLoader.TotalCount - 1f);
			panel.Append(text);
			textlist.Add(text);
		}
	}

	private void Text_OnUpdate(UIElement affectedElement) {
		if (currentSelectTemplate == affectedElement.UniqueId) {
			UIText text = textlist.Where(i => i.UniqueId == currentSelectTemplate).FirstOrDefault();
			if (text == null) {
				return;
			}
			RelicTemplate template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
			if (template == null) {
				return;
			}
			text.SetText($"[c/{Color.Yellow.Hex3()}:{template.Name}]");
		}
		else {
			UIText text = textlist.Where(i => i.UniqueId == affectedElement.UniqueId).FirstOrDefault();
			if (text == null) {
				return;
			}
			RelicTemplate template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
			if (template == null) {
				return;
			}
			text.SetText(template.Name);
		}
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		currentSelectTemplate = listeningElement.UniqueId;
	}

}
