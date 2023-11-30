using Terraria;
using Terraria.UI;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Card;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.Potion;

namespace BossRush.Contents.Items;
public class DeCardUIState : UIState {
	public override void OnActivate() {
		Elements.Clear();
		var cardUI = new UICardItemKill(TextureAssets.InventoryBack);
		cardUI.UISetWidthHeight(52, 52);
		cardUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(0, -80), new Vector2(26, 26));
		Append(cardUI);
	}
}
public class TransmuteTablet : ModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 20;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.rare = ItemRarityID.Red;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			if (uiSystemInstance.userInterface.CurrentState == null) uiSystemInstance.userInterface.SetState(uiSystemInstance.DeCardUIState);
			else uiSystemInstance.userInterface.SetState(null);
		}
		return false;
	}
}
public class UICardItemKill : UIImageButton {
	public UICardItemKill(Asset<Texture2D> texture) : base(texture) {
	}
	public override void LeftMouseDown(UIMouseEvent evt) {
		var item = Main.mouseItem;
		if (item.type == ItemID.None)
			return;

		if (CheckWeapon(item))
			return;
		if (CheckPotion(item))
			return;
	}
	private bool CheckWeapon(Item item) {
		if (item.consumable || !item.accessory && item.damage < 1 && item.buffType == 0)
			return false;
		Main.mouseItem.TurnToAir();
		Main.LocalPlayer.inventory[58].TurnToAir();
		int type = ModContent.ItemType<CopperCard>();
		var cardplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		if (Main.rand.Next(201) < cardplayer.CardLuck) type = ModContent.ItemType<PlatinumCard>();
		if (Main.rand.Next(201) < cardplayer.CardLuck * 1.5f) type = ModContent.ItemType<GoldCard>();
		if (Main.rand.Next(201) < cardplayer.CardLuck * 3) type = ModContent.ItemType<SilverCard>();
		Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), type);
		return true;
	}
	private bool CheckPotion(Item item) {
		if (item.buffType == 0)
			return false;
		Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), ModContent.ItemType<MysteriousPotion>());
		return true;
	}
}
