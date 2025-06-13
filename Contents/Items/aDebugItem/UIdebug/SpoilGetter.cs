using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Common.Systems;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common.Systems.SpoilSystem;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.aDebugItem.UIdebug;
internal class SpoilGetter : ModItem {
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
			player.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.Add(ModContent.ItemType<WoodenLootBox>());
			ModContent.GetInstance<UniversalSystem>().ActivateDebugUI("spoil");
		}
		return base.UseItem(player);
	}
}
class SpoilGetterUI : UIState {
	UIPanel panel;
	int currentSelectTemplate = -1;
	private const int MAX_LINES = 6;

	List<SpoilsUIButton> btn_list;
	private List<ModSpoil> list_Spoil = new();
	public const int SPOIL_MAXLINE = 10;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(500, 500);
		Append(panel);

		btn_list = new();
	}
	public override void OnActivate() {
		btn_list.Clear();
		list_Spoil.Clear();
		panel.RemoveAllChildren();
		list_Spoil.AddRange(ModSpoilSystem.GetSpoilsList());
		int length = list_Spoil.Count;
		int lineCounter = 0;

		for (int i = 0; i < length; i++) {
			if (i % SPOIL_MAXLINE == 0) {
				lineCounter++;
			}
			if (lineCounter < SPOIL_MAXLINE) {

				SpoilsUIButton button = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), list_Spoil[i]) {
					Width = StyleDimension.FromPixels(44f),
					Height = StyleDimension.FromPixels(44f),
					Left = StyleDimension.FromPixels(i % SPOIL_MAXLINE * 46.0f + 6.0f),
					Top = StyleDimension.FromPixels(i / SPOIL_MAXLINE * 48.0f + 1.0f)
				};
				button.OnLeftClick += Text_OnLeftClick;
				btn_list.Add(button);
				panel.Append(button);
			}
		}
	}
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		//linePosition -= MathF.Sign(evt.ScrollWheelValue);
		//int offsetvalue = linePosition * SPOIL_MAXLINE;
		//int length = list_Spoil.Count;
		//int offsetlength = length - offsetvalue;
		//for (int i = 0; i < length; i++) {
		//	int arty = Math.Clamp(i + offsetvalue, 0, length - 1);
		//	btn_list[i].spoil = null;
		//	if (i > offsetlength) {
		//		continue;
		//	}
		//	btn_list[i].spoil = list_Spoil[arty];
		//}
	}
	private void Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		currentSelectTemplate = listeningElement.UniqueId;
	}
}
