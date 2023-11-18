using Terraria;
using Terraria.UI;
using Terraria.ID;
using BossRush.Common;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Card;
public class DeCardUIState : UIState {
	public override void OnActivate() {
		Elements.Clear();
		UICardItemKill cardUI = new UICardItemKill(TextureAssets.InventoryBack);
		cardUI.UISetWidthHeight(52, 52);
		cardUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(0, -80), new Vector2(26, 26));
		Append(cardUI);
	}
}
public class CardSacrifice : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 20;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.rare = ItemRarityID.Red;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			if (uiSystemInstance.userInterface.CurrentState == null) {
				uiSystemInstance.userInterface.SetState(uiSystemInstance.DeCardUIState);
			}
			else {
				uiSystemInstance.userInterface.SetState(null);
			}
		}
		return false;
	}
}
public class UICardItemKill : UIImageButton {
	public UICardItemKill(Asset<Texture2D> texture) : base(texture) {
	}
	public override void LeftMouseDown(UIMouseEvent evt) {
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable || !Main.mouseItem.accessory)
				return;
			Main.mouseItem.TurnToAir();
			Main.LocalPlayer.inventory[58].TurnToAir();
			int type = ModContent.ItemType<CopperCard>();
			PlayerCardHandle cardplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
			if (Main.rand.Next(201) < cardplayer.CardLuck) {
				type = ModContent.ItemType<PlatinumCard>();
			}
			if (Main.rand.Next(201) < cardplayer.CardLuck * 1.5f) {
				type = ModContent.ItemType<GoldCard>();
			}
			if (Main.rand.Next(201) < cardplayer.CardLuck * 3) {
				type = ModContent.ItemType<SilverCard>();
			}
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), type);
		}
	}
}
