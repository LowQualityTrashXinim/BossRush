using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Potion;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.Accessories.Crystal;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.FrozenShark;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.SingleBarrelMinishark;
using BossRush.Contents.Items.Toggle;

namespace BossRush.Contents.Items;
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
			if (uiSystemInstance.userInterface.CurrentState == null) {
				uiSystemInstance.userInterface.SetState(uiSystemInstance.DeCardUIState);
			}
		}
		return false;
	}
}
public class DeCardUIState : UIState {
	public override void OnActivate() {
		Elements.Clear();
		var panalUI = new UIPanel();
		panalUI.UISetWidthHeight(250, 150);
		panalUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(0, -150), new Vector2(100, 100));
		Append(panalUI);
		var cardUI = new TransmutationUI(TextureAssets.InventoryBack, Main.LocalPlayer);
		Vector2 origin = new Vector2(26, 26);
		cardUI.UISetWidthHeight(52, 52);
		cardUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(52, -150), origin);
		Append(cardUI);
		var cardUI1 = new TransmutationUI(TextureAssets.InventoryBack, Main.LocalPlayer);
		cardUI1.UISetWidthHeight(52, 52);
		cardUI1.UISetPosition(Main.LocalPlayer.Center - new Vector2(-10, -150), origin);
		Append(cardUI1);
		var cardUIbtn = new TransmutationUIConfirmButton(TextureAssets.InventoryBack10);
		cardUIbtn.UISetWidthHeight(52, 52);
		cardUIbtn.UISetPosition(Main.LocalPlayer.Center - new Vector2(-104, -150), origin);
		Append(cardUIbtn);
		var exitUI = new ExitUI(TextureAssets.InventoryBack13);
		exitUI.UISetWidthHeight(52, 52);
		exitUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(-104, -90), origin);
		Append(exitUI);
	}
}
public class ExitUI : UIImageButton {
	public ExitUI(Asset<Texture2D> texture) : base(texture) {
	}

	public override void LeftClick(UIMouseEvent evt) {
		ModContent.GetInstance<UniversalSystem>().DeactivateState();
	}
}
public class TransmutationUI : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
	private Player player;
	public TransmutationUI(Asset<Texture2D> texture, Player player) : base(texture) {
		this.texture = texture.Value;
		this.player = player;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (item == null) {
			if (Main.mouseItem.type != ItemID.None) {
				if (Main.mouseItem.accessory) {

				}
				else if (Main.mouseItem.damage > 0) {

				}
				else if (Main.mouseItem.buffType != 0 && Main.mouseItem.stack > 1) {
					Main.mouseItem.stack--;
					item = Main.mouseItem.Clone();
					item.stack = 1;
					return;
				}
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
			}
		}
		else {
			if (Main.mouseItem.type == ItemID.None) {
				Main.mouseItem = item.Clone();
				item = null;
			}
		}
	}
	public override void OnDeactivate() {
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				player.inventory[i] = item;
				return;
			}
		}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
		base.Draw(spriteBatch);
		try {
			if (item != null) {
				Main.instance.LoadItem(item.type);
				Texture2D texture = TextureAssets.Item[item.type].Value;
				Vector2 origin = texture.Size() * .5f;
				float scaling = ScaleCalculation(texture.Size());
				spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / (textureSize.Length() * 1.5f);
}
public class TransmutationUIConfirmButton : UIImageButton {
	public TransmutationUIConfirmButton(Asset<Texture2D> texture) : base(texture) {
	}
	public override void LeftMouseDown(UIMouseEvent evt) {
		List<TransmutationUI> resultlist = new List<TransmutationUI>();
		foreach (var element in Parent.Children) {
			if (element is TransmutationUI transmutateResult) {
				if (transmutateResult.item == null || transmutateResult.item.type == ItemID.None)
					continue;
				resultlist.Add(transmutateResult);
			}
		}
		List<int> itemList = resultlist.Select(i => i.item.type).ToList();
		if (CheckForSpecialDrop(itemList)) {
			resultlist.ForEach(i => i.item = null);
			return;
		}
		foreach (var result in resultlist) {
			if (CheckWeapon(result.item)) {
				result.item = null;
				continue;
			}
			if (CheckPotion(result.item)) {
				result.item = null;
				continue;
			}
		}
	}
	private bool CheckForSpecialDrop(List<int> itemList) {
		Player player = Main.LocalPlayer;
		if (itemList.Contains(ItemID.LifeCrystal) && itemList.Contains(ItemID.ManaCrystal)) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<NatureCrystal>());
			return true;
		}
		if (Main.rand.NextBool()) {
			return false;
		}
		if (itemList.Contains(ModContent.ItemType<NatureCrystal>()) && itemList.Contains(ItemID.ManaRegenerationBand)) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<EnergeticCrystal>());
			return true;
		}
		bool MiniShark = itemList.Contains(ItemID.Minishark);
		bool IceBlade = itemList.Contains(ItemID.IceBlade);
		bool Musket = itemList.Contains(ItemID.Musket);
		if (itemList.Contains(ModContent.ItemType<CelestialWrath>()) && itemList.Contains(ModContent.ItemType<MysteriousPotion>())) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<GodDice>());
		}
		if (MiniShark && IceBlade) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<FrozenShark>());
			return true;
		}
		if (Musket && MiniShark) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SingleBarrelMinishark>());
			return true;
		}
		return false;
	}
	private bool CheckWeapon(Item item) {
		if (item.damage > 0 && !item.accessory || item.damage < 1 && item.accessory) {
			int type = ModContent.ItemType<CopperCard>();
			var cardplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			if (Main.rand.Next(201) < cardplayer.CardLuck) type = ModContent.ItemType<PlatinumCard>();
			if (Main.rand.Next(201) < cardplayer.CardLuck * 1.5f) type = ModContent.ItemType<GoldCard>();
			if (Main.rand.Next(201) < cardplayer.CardLuck * 3) type = ModContent.ItemType<SilverCard>();
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), type);
			return true;
		}
		return false;
	}
	private bool CheckPotion(Item item) {
		if (item.buffType != 0 && item.buffType != ModContent.BuffType<MysteriousPotionBuff>()) {
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), ModContent.ItemType<MysteriousPotion>());
			return true;
		}
		return false;
	}
}
