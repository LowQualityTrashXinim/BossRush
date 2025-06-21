using BossRush.Common.Global;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Weapon.UnfinishedItem;
using BossRush.Contents.Transfixion.SoulBound;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Texture;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Contents.Items.Toggle;
public class TransmuteTablet : ModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.autoReuse = false;
		Item.noUseGraphic = true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateTransmutationUI();
		}
		return base.UseItem(player);
	}
}
public class EnergyDrawPanel : Roguelike_UIPanel {
	public override void PreDraw(SpriteBatch spriteBatch) {
		PlayerStatsHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		Rectangle rect = this.GetDimensions().ToRectangle();
		Point point = new(rect.X, rect.Y);
		BossRushUtils.DrawProgressLine(spriteBatch,
			modplayer.TransmutationPower,
			modplayer.TransmutationPowerMaximum,
			new(rect.X, rect.Y + 2),
			new(0, 0, rect.Width, rect.Height - 4),
			Color.Cyan,
			Color.Blue,
			16f,
			9f);
	}
}
public class TransmutationUIState : UIState {
	Asset<Texture2D> tex = TextureAssets.InventoryBack;

	UIPanel panel;
	ExitUI btn_exit;
	EnergyDrawPanel panelToBeDrawnOnTop;
	UIPanel slotPanel;

	UIPanel headerPanel;
	Roguelike_UIImage btn_EnergyMode;
	Roguelike_UIImage btn_RelicMergeMode;
	Roguelike_UIImage btn_ItemShift;
	UIPanel FooterPanel;

	ItemHolderSlot Relicslot1;
	ItemHolderSlot Relicslot2;
	Roguelike_UIImageButton btn_RelicMerge;
	ItemHolderSlot Relicresultslot;
	UITextBox transmutateText;
	Roguelike_UITextPanel mergeInfo;

	Roguelike_UITextPanel EquivalentExchange;
	Roguelike_UITextPanel UpgradeRarity;
	ItemHolderSlot ItemShiftSlot;
	ItemHolderSlot ItemResultSlotShift;
	Roguelike_UIImageButton btn_ItemShiftConfirm;
	ItemHolderSlot ItemAccSelection;
	ItemHolderSlot ItemArmorSelection;
	ItemHolderSlot ItemWeaponSelection;
	Roguelike_UITextPanel shiftTextInfo;
	bool UpgradeRarityMode = false;

	ItemHolderSlot energyItemslot1;
	ItemHolderSlot energyItemslot2;
	ItemHolderSlot energyItemslot3;
	ItemHolderSlot energyItemslot4;
	Roguelike_UIImageButton btn_energy;
	Roguelike_UITextPanel energyinfo;
	public void GeneralInit() {
		panel = new UIPanel();
		panel.UISetWidthHeight(450, 350);
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		Append(panel);

		headerPanel = new();
		headerPanel.Width.Percent = 1f;
		headerPanel.Height.Pixels = 70f;
		headerPanel.SetPadding(0);
		headerPanel.BorderColor = new(0, 0, 0, 0);
		headerPanel.BackgroundColor = new(0, 0, 0, 0);
		panel.Append(headerPanel);

		slotPanel = new();
		slotPanel.MarginTop = headerPanel.Height.Pixels + 5;
		slotPanel.Width.Precent = 1f;
		slotPanel.Height.Pixels = 193;
		slotPanel.HAlign = 1f;
		panel.Append(slotPanel);
	}
	public void HeaderInit() {
		btn_RelicMergeMode = new(tex);
		btn_RelicMergeMode.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<TransmuteTablet>("TransmuteTablet_Relic")));
		btn_RelicMergeMode.VAlign = .5f;
		btn_RelicMergeMode.OnLeftClick += btn_Mode_OnLeftClick;
		btn_RelicMergeMode.HighlightColor = btn_RelicMergeMode.OriginalColor.ScaleRGB(.5f);
		btn_RelicMergeMode.Highlight = true;
		btn_RelicMergeMode.SwapHightlightColorWithOriginalColor();
		headerPanel.Append(btn_RelicMergeMode);

		btn_EnergyMode = new(tex);
		btn_EnergyMode.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<TransmutationUIState>("TransmutationEnergy")), false);
		btn_EnergyMode.MarginLeft += btn_RelicMergeMode.Width.Pixels + 10;
		btn_EnergyMode.VAlign = .5f;
		btn_EnergyMode.HighlightColor = btn_EnergyMode.OriginalColor.ScaleRGB(.5f);
		btn_EnergyMode.OnLeftClick += btn_Mode_OnLeftClick;
		btn_EnergyMode.SwapHightlightColorWithOriginalColor();
		headerPanel.Append(btn_EnergyMode);

		btn_ItemShift = new(tex);
		btn_ItemShift.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAsEntity<TransmuteTablet>()), false);
		btn_ItemShift.MarginLeft += btn_RelicMergeMode.Width.Pixels * 2 + 20;
		btn_ItemShift.VAlign = .5f;
		btn_ItemShift.HighlightColor = btn_EnergyMode.OriginalColor.ScaleRGB(.5f);
		btn_ItemShift.OnLeftClick += btn_Mode_OnLeftClick;
		btn_ItemShift.SwapHightlightColorWithOriginalColor();
		headerPanel.Append(btn_ItemShift);


		btn_exit = new ExitUI(tex);
		btn_exit.UISetWidthHeight(52, 52);
		btn_exit.HAlign = 1f;
		btn_exit.VAlign = .5f;
		headerPanel.Append(btn_exit);
	}
	public void RelicMergeInit() {
		Relicslot1 = new ItemHolderSlot(tex);
		Relicslot1.UISetWidthHeight(52, 52);
		Relicslot1.OnLeftClick += Slot_OnLeftClick;
		slotPanel.Append(Relicslot1);

		Relicslot2 = new ItemHolderSlot(tex);
		Relicslot2.UISetWidthHeight(52, 52);
		Relicslot2.HAlign = .33f;
		Relicslot2.OnLeftClick += Slot_OnLeftClick;
		slotPanel.Append(Relicslot2);

		btn_RelicMerge = new Roguelike_UIImageButton(TextureAssets.InventoryBack10);
		btn_RelicMerge.HAlign = .66f;
		btn_RelicMerge.OnLeftClick += Btn_confirm_OnLeftClick;
		btn_RelicMerge.SetVisibility(.6f, 1f);
		slotPanel.Append(btn_RelicMerge);

		Relicresultslot = new(tex);
		Relicresultslot.HAlign = 1f;
		Relicresultslot.OnLeftClick += Resultslot_OnLeftClick;
		slotPanel.Append(Relicresultslot);

		mergeInfo = new("");
		mergeInfo.VAlign = 1f;
		mergeInfo.Width.Percent = 1f;
		mergeInfo.Height.Pixels = 110;
		mergeInfo.UseCustmSetHeight = true;
		slotPanel.Append(mergeInfo);
	}
	public void Visual_RelicMerge(bool hide) {
		Relicslot1.Hide = hide;
		Relicslot2.Hide = hide;
		btn_RelicMerge.Hide = hide;
		Relicresultslot.Hide = hide;
		mergeInfo.Hide = hide;
	}
	public void TransmutationEnergyInit() {
		FooterPanel = new();
		FooterPanel.VAlign = 1f;
		FooterPanel.Width.Percent = 1f;
		FooterPanel.Height.Pixels = 61;
		FooterPanel.BackgroundColor = new(0, 0, 0, 0);
		FooterPanel.BorderColor = new(0, 0, 0, 0);
		panel.Append(FooterPanel);

		panelToBeDrawnOnTop = new();
		panelToBeDrawnOnTop.HAlign = .5f;
		panelToBeDrawnOnTop.VAlign = 1;
		panelToBeDrawnOnTop.Width.Percent = 1f;
		panelToBeDrawnOnTop.Height.Pixels = 32;
		panelToBeDrawnOnTop.BorderColor = new(255, 255, 255, 0);
		panelToBeDrawnOnTop.BackgroundColor = new(0, 0, 0, 0);
		FooterPanel.Append(panelToBeDrawnOnTop);

		transmutateText = new("", .76f);
		transmutateText.VAlign = 1;
		transmutateText.HAlign = .5f;
		transmutateText.TextHAlign = .5f;
		transmutateText.ShowInputTicker = false;
		transmutateText.BorderColor = new(0, 0, 0, 0);
		transmutateText.BackgroundColor = new(0, 0, 0, 0);
		FooterPanel.Append(transmutateText);
	}
	public void Visual_ItemShift(bool hide) {
		ItemShiftSlot.Hide = hide;
		ItemResultSlotShift.Hide = hide;
		btn_ItemShiftConfirm.Hide = hide;
		ItemAccSelection.Hide = hide;
		ItemArmorSelection.Hide = hide;
		ItemWeaponSelection.Hide = hide;
		EquivalentExchange.Hide = hide;
		UpgradeRarity.Hide = hide;
		shiftTextInfo.Hide = hide;
	}
	public void ItemShiftInit() {
		EquivalentExchange = new("Equivalent Exchange");
		EquivalentExchange.OnLeftClick += ItemShiftOptionPanel_OnLeftClick;
		EquivalentExchange.Hide = true;
		EquivalentExchange.Width.Set(0, .45f);
		EquivalentExchange.BorderColor = Color.Yellow;
		slotPanel.Append(EquivalentExchange);

		UpgradeRarity = new("Upgrade Rarity");
		UpgradeRarity.OnLeftClick += ItemShiftOptionPanel_OnLeftClick;
		UpgradeRarity.Width.Set(0, .45f);
		UpgradeRarity.Hide = true;
		UpgradeRarity.HAlign = 1f;
		slotPanel.Append(UpgradeRarity);

		ItemShiftSlot = new ItemHolderSlot(tex);
		ItemShiftSlot.OnLeftClick += ItemShift_OnLeftClick;
		ItemShiftSlot.VAlign = .5f;
		ItemShiftSlot.Hide = true;
		slotPanel.Append(ItemShiftSlot);

		btn_ItemShiftConfirm = new Roguelike_UIImageButton(TextureAssets.InventoryBack10);
		btn_ItemShiftConfirm.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAsEntity<TransmuteTablet>()));
		btn_ItemShiftConfirm.MarginLeft = 52 + 10;
		btn_ItemShiftConfirm.VAlign = .5f;
		btn_ItemShiftConfirm.OnLeftClick += Btn_ItemShiftConfirm_OnLeftClick;
		btn_ItemShiftConfirm.SetVisibility(.6f, 1f);
		btn_ItemShiftConfirm.Hide = true;
		slotPanel.Append(btn_ItemShiftConfirm);

		ItemResultSlotShift = new ItemHolderSlot(tex);
		ItemResultSlotShift.MarginLeft = (52 + 10) * 2;
		ItemResultSlotShift.VAlign = .5f;
		ItemResultSlotShift.OnLeftClick += ItemShift_OnLeftClick;
		ItemResultSlotShift.Hide = true;
		slotPanel.Append(ItemResultSlotShift);

		ItemAccSelection = new(tex);
		ItemAccSelection.HighlightColor = ItemAccSelection.OriginalColor.ScaleRGB(.7f);
		ItemAccSelection.SwapHightlightColorWithOriginalColor();
		ItemAccSelection.OnLeftClick += ItemSelection_OnLeftClick;
		ItemAccSelection.Highlight = true;
		ItemAccSelection.HAlign = 0;
		ItemAccSelection.VAlign = 1;
		ItemAccSelection.Hide = true;
		ItemAccSelection.item = new Item(ItemID.AvengerEmblem);
		ItemAccSelection.Description = "Transmute to accessory";
		slotPanel.Append(ItemAccSelection);

		ItemArmorSelection = new(tex);
		ItemArmorSelection.MarginLeft = 52 + 10;
		ItemArmorSelection.HighlightColor = ItemArmorSelection.OriginalColor.ScaleRGB(.7f);
		ItemArmorSelection.SwapHightlightColorWithOriginalColor();
		ItemArmorSelection.OnLeftClick += ItemSelection_OnLeftClick;
		ItemArmorSelection.VAlign = 1;
		ItemArmorSelection.Hide = true;
		ItemArmorSelection.item = new Item(ItemID.IronChainmail);
		ItemArmorSelection.Description = "Transmute to armor piece";
		slotPanel.Append(ItemArmorSelection);

		ItemWeaponSelection = new(tex);
		ItemWeaponSelection.MarginLeft = (52 + 10) * 2;
		ItemWeaponSelection.HighlightColor = ItemWeaponSelection.OriginalColor.ScaleRGB(.7f);
		ItemWeaponSelection.SwapHightlightColorWithOriginalColor();
		ItemWeaponSelection.OnLeftClick += ItemSelection_OnLeftClick;
		ItemWeaponSelection.VAlign = 1;
		ItemWeaponSelection.Hide = true;
		ItemWeaponSelection.item = new Item(ItemID.IronBroadsword);
		ItemWeaponSelection.Description = "Transmute to weapon";
		slotPanel.Append(ItemWeaponSelection);

		shiftTextInfo = new("", .77f);
		shiftTextInfo.Width.Pixels = 200;
		shiftTextInfo.Height.Pixels = 110;
		shiftTextInfo.HAlign = 1f;
		shiftTextInfo.VAlign = 1f;
		shiftTextInfo.UseCustmSetHeight = true;
		shiftTextInfo.Hide = true;
		slotPanel.Append(shiftTextInfo);
	}
	private void ItemShiftOptionPanel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement.UniqueId == EquivalentExchange.UniqueId) {
			UpgradeRarityMode = false;
			EquivalentExchange.BorderColor = Color.Yellow;
			UpgradeRarity.BorderColor = Color.Black;
		}
		else if (listeningElement.UniqueId == UpgradeRarity.UniqueId) {
			UpgradeRarityMode = true;
			UpgradeRarity.BorderColor = Color.Yellow;
			EquivalentExchange.BorderColor = Color.Black;
		}
	}
	public void Visual_Energy(bool hide) {
		energyItemslot1.Hide = hide;
		energyItemslot2.Hide = hide;
		energyItemslot3.Hide = hide;
		energyItemslot4.Hide = hide;
		btn_energy.Hide = hide;
		energyinfo.Hide = hide;
	}
	public void EnergyModeInit() {
		energyItemslot1 = new ItemHolderSlot(tex);
		energyItemslot1.UISetWidthHeight(52, 52);
		energyItemslot1.OnLeftClick += EnergyItemslot_OnLeftClick;
		slotPanel.Append(energyItemslot1);

		energyItemslot2 = new ItemHolderSlot(tex);
		energyItemslot2.UISetWidthHeight(52, 52);
		energyItemslot2.HAlign = .25f;
		energyItemslot2.OnLeftClick += EnergyItemslot_OnLeftClick;
		slotPanel.Append(energyItemslot2);

		energyItemslot3 = new(tex);
		energyItemslot3.HAlign = .5f;
		energyItemslot3.OnLeftClick += EnergyItemslot_OnLeftClick;
		slotPanel.Append(energyItemslot3);

		energyItemslot4 = new(tex);
		energyItemslot4.HAlign = .75f;
		energyItemslot4.OnLeftClick += EnergyItemslot_OnLeftClick;
		slotPanel.Append(energyItemslot4);

		btn_energy = new Roguelike_UIImageButton(TextureAssets.InventoryBack10);
		btn_energy.HAlign = 1;
		btn_energy.OnLeftClick += btn_energy_OnLeftClick;
		btn_energy.SetVisibility(.6f, 1f);
		slotPanel.Append(btn_energy);

		energyinfo = new("");
		energyinfo.VAlign = 1f;
		energyinfo.Width.Percent = 1f;
		energyinfo.Height.Pixels = 110;
		energyinfo.UseCustmSetHeight = true;
		slotPanel.Append(energyinfo);

		Visual_Energy(true);
	}
	private void EnergyItemslot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		Item item = Main.mouseItem;
		if (item.IsAWeapon() || item.accessory || item.IsThisArmorPiece()) {
			if (listeningElement.UniqueId == energyItemslot1.UniqueId) {
				SimpleItemMouseExchange(player, ref energyItemslot1.item);
			}
			else if (listeningElement.UniqueId == energyItemslot2.UniqueId) {
				SimpleItemMouseExchange(player, ref energyItemslot2.item);
			}
			else if (listeningElement.UniqueId == energyItemslot3.UniqueId) {
				SimpleItemMouseExchange(player, ref energyItemslot3.item);
			}
			else if (listeningElement.UniqueId == energyItemslot4.UniqueId) {
				SimpleItemMouseExchange(player, ref energyItemslot4.item);
			}
		}
	}
	private void btn_energy_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		PlayerStatsHandle handler = player.ModPlayerStats();
		int total = GetEnergyFromEnergySlot();
		if (total > 0) {
			handler.Add_TransmutationPower(total);
			energyItemslot1.item.TurnToAir();
			energyItemslot2.item.TurnToAir();
			energyItemslot3.item.TurnToAir();
			energyItemslot4.item.TurnToAir();
		}
	}
	public int GetEnergyFromEnergySlot() {
		int total = 0;
		if (energyItemslot1.item.type != 0) {
			total += EnergyPoint(energyItemslot1.item.OriginalRarity, true);
		}
		if (energyItemslot2.item.type != 0) {
			total += EnergyPoint(energyItemslot2.item.OriginalRarity, true);
		}
		if (energyItemslot3.item.type != 0) {
			total += EnergyPoint(energyItemslot3.item.OriginalRarity, true);
		}
		if (energyItemslot4.item.type != 0) {
			total += EnergyPoint(energyItemslot4.item.OriginalRarity, true);
		}
		return total;
	}
	public int EnergyPoint(int rarity, bool Charging = false, float extramultiplication = 0) {
		rarity += 1;
		float multiplier = 1f + extramultiplication;
		if (Charging) {
			multiplier -= .25f;
		}
		int baseVal = 10;
		return (int)Math.Ceiling((baseVal + 10 * (rarity * (.25f + .1f * rarity))) * multiplier);
	}
	private void ItemSelection_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ItemAccSelection.Highlight = false;
		ItemArmorSelection.Highlight = false;
		ItemWeaponSelection.Highlight = false;
		if (listeningElement.UniqueId == ItemAccSelection.UniqueId) {
			ItemAccSelection.Highlight = true;
		}
		else if (listeningElement.UniqueId == ItemArmorSelection.UniqueId) {
			ItemArmorSelection.Highlight = true;
		}
		else if (listeningElement.UniqueId == ItemWeaponSelection.UniqueId) {
			ItemWeaponSelection.Highlight = true;
		}
	}
	private void Btn_ItemShiftConfirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (ItemShiftSlot.item.type != 0 && ItemResultSlotShift.item.type == 0) {
			Player player = Main.LocalPlayer;
			PlayerStatsHandle modplayer = player.ModPlayerStats();
			Item item = ItemShiftSlot.item;
			int rareval1 = item.OriginalRarity;
			byte rareOffset = 0;
			if (UpgradeRarityMode) {
				rareOffset = 1;
			}
			float extra = 0;
			int itemType = ItemID.None;
			if (ItemAccSelection.Highlight) {
				if (!item.accessory) {
					extra += .55f;
				}
				itemType = GetItemRarityDB(rareval1 + rareOffset, 2);
			}
			else if (ItemWeaponSelection.Highlight) {
				if (!item.IsAWeapon()) {
					extra += .5f;
				}
				itemType = GetItemRarityDB(rareval1 + rareOffset, 1);
			}
			else if (ItemArmorSelection.Highlight) {
				if (item.headSlot > 0) {
					itemType = GetItemRarityDB(rareval1 + rareOffset, 3);
				}
				else if (item.bodySlot > 0) {
					itemType = GetItemRarityDB(rareval1 + rareOffset, 4);
				}
				else if (item.legSlot > 0) {
					itemType = GetItemRarityDB(rareval1 + rareOffset, 5);
				}
				else {
					extra += .35f;
					itemType = GetItemRarityDB(rareval1 + rareOffset, Main.rand.Next(3, 6));
				}
			}
			int cost = EnergyPoint(rareval1 + rareOffset, extramultiplication: extra);
			if (!modplayer.Modify_TransmutationPower(-cost)) {
				return;
			}
			if (itemType == ItemID.None) {
				Main.NewText($"Detected no rarity found ! at {rareval1} rarity with {item.Name}");
				return;
			}
			LootBoxBase.AmmoForWeapon(player, itemType);
			ItemResultSlotShift.item = new Item(itemType);
			ItemShiftSlot.item.TurnToAir();
		}
	}
	private void ItemShift_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		if (listeningElement.UniqueId == ItemShiftSlot.UniqueId) {
			Item item = Main.mouseItem;
			SimpleItemMouseExchange(player, ref ItemShiftSlot.item);
		}
		else if (listeningElement.UniqueId == ItemResultSlotShift.UniqueId) {
			Item item = Main.mouseItem;
			if (item.type != 0) {
				return;
			}
			if (ItemResultSlotShift.item.type == 0) {
				return;
			}
			Main.mouseItem = ItemResultSlotShift.item.Clone();
			Main.LocalPlayer.inventory[58] = ItemResultSlotShift.item.Clone();
			ItemResultSlotShift.item.TurnToAir();
		}
	}
	public override void OnInitialize() {
		GeneralInit();

		HeaderInit();

		RelicMergeInit();

		ItemShiftInit();

		TransmutationEnergyInit();

		EnergyModeInit();
	}
	private void Resultslot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Item item = Main.mouseItem;
		if (item.type != 0) {
			return;
		}
		if (Relicresultslot.item.type == 0) {
			return;
		}
		Main.mouseItem = Relicresultslot.item.Clone();
		Main.LocalPlayer.inventory[58] = Relicresultslot.item.Clone();
		Relicresultslot.item.TurnToAir();
	}
	private void btn_Mode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		btn_EnergyMode.Highlight = false;
		btn_RelicMergeMode.Highlight = false;
		btn_ItemShift.Highlight = false;
		Visual_RelicMerge(true);
		Visual_ItemShift(true);
		Visual_Energy(true);
		//visual change
		if (listeningElement.UniqueId == btn_RelicMergeMode.UniqueId) {
			btn_RelicMergeMode.Highlight = true;
			Visual_RelicMerge(false);
		}
		else if (listeningElement.UniqueId == btn_EnergyMode.UniqueId) {
			btn_EnergyMode.Highlight = true;
			Visual_Energy(false);
		}
		else if (listeningElement.UniqueId == btn_ItemShift.UniqueId) {
			btn_ItemShift.Highlight = true;
			Visual_ItemShift(false);
		}
	}
	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (Relicslot1.item.type != 0 && Relicslot2.item.type != 0 && Relicresultslot.item.type == 0) {
			if (Relicslot1.item.ModItem is Relic relic1 && Relicslot2.item.ModItem is Relic relic2) {
				if (relic1.RelicTier + relic2.RelicTier > 4) {
					SoundEngine.PlaySound(SoundID.AbigailSummon with { Pitch = -1 });
					return;
				}
				PlayerStatsHandle handler = Main.LocalPlayer.ModPlayerStats();
				if (!handler.Modify_TransmutationPower(-RelicMergeCost(relic1.RelicTier, relic2.RelicTier))) {
					return;
				}
				if (RelicTemplateLoader.MergeStat(relic1, relic2)) {
					Relicresultslot.item = relic1.Item.Clone();
					relic1.Item.TurnToAir();
					SoundEngine.PlaySound(SoundID.AchievementComplete with { Pitch = -1 });
					return;
				}
			}
		}
		SoundEngine.PlaySound(SoundID.AbigailSummon with { Pitch = -1 });
	}
	public int RelicMergeCost(int Tier1, int Tier2) => (EnergyPoint(Tier1) + EnergyPoint(Tier2) * (Tier1 + Tier2));
	private void Slot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		Item item = Main.mouseItem;
		if (item.type != ModContent.ItemType<Relic>()) {
			return;
		}
		if (listeningElement.UniqueId == Relicslot1.UniqueId) {
			SimpleItemMouseExchange(player, ref Relicslot1.item);
		}
		else if (listeningElement.UniqueId == Relicslot2.UniqueId) {
			SimpleItemMouseExchange(player, ref Relicslot2.item);
		}
	}
	public void SimpleItemMouseExchange(Player player, ref Item item) {
		Item mouseitem = Main.mouseItem;
		if (item.type == 0) {
			if (Main.mouseItem.type != 0) {
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
			}
		}
		else {
			if (Main.mouseItem.type != 0) {
				Item cached = item.Clone();
				item = Main.mouseItem.Clone();
				Main.mouseItem = cached.Clone();
				player.inventory[58] = cached.Clone();
			}
			else {
				Main.mouseItem = item.Clone();
				player.inventory[58] = item.Clone();
				item.TurnToAir();
			}
		}
	}
	public override void OnDeactivate() {
		int optimization = 0;
		optimization = BossRushUtils.FastDropItem(Relicslot1.item);
		optimization = BossRushUtils.FastDropItem(Relicslot2.item, optimization);
		optimization = BossRushUtils.FastDropItem(Relicresultslot.item, optimization);
		optimization = BossRushUtils.FastDropItem(ItemShiftSlot.item, optimization);
		optimization = BossRushUtils.FastDropItem(ItemResultSlotShift.item, optimization);
		optimization = BossRushUtils.FastDropItem(energyItemslot1.item, optimization);
		optimization = BossRushUtils.FastDropItem(energyItemslot2.item, optimization);
		optimization = BossRushUtils.FastDropItem(energyItemslot3.item, optimization);
		optimization = BossRushUtils.FastDropItem(energyItemslot4.item, optimization);

	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		PlayerStatsHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		transmutateText.SetText($"Energy : {modplayer.TransmutationPower} / {modplayer.TransmutationPowerMaximum}");
		if (btn_EnergyMode.Highlight) {
			int totalEnergyCharge = GetEnergyFromEnergySlot();
			string text = "";
			energyinfo.SetText(text + "\nTotal energy charging : " + totalEnergyCharge);

		}
		else if (btn_ItemShift.Highlight) {
			Item item = ItemShiftSlot.item;
			if (item.type == 0) {
				shiftTextInfo.SetText("");
				return;
			}
			int rareval1 = item.OriginalRarity;
			byte rareOffset = 0;
			if (UpgradeRarityMode) {
				rareOffset = 1;
			}
			float extra = 0;
			if (ItemAccSelection.Highlight) {
				if (!item.accessory) {
					extra += .55f;
				}
			}
			else if (ItemWeaponSelection.Highlight) {
				if (!item.IsAWeapon()) {
					extra += .5f;
				}
			}
			else if (ItemArmorSelection.Highlight) {
				if (item.headSlot <= 0 && item.bodySlot <= 0 && item.legSlot <= 0) {
					extra += .35f;
				}
			}
			int cost = EnergyPoint(rareval1 + rareOffset, extramultiplication: extra);
			if (modplayer.TransmutationPower < cost) {
				shiftTextInfo.SetText("Insufficient energy\nRequired energy cost : " + cost);
			}
			else {
				shiftTextInfo.SetText("Item transmutation cost : " + cost);
			}
		}
		else if (btn_RelicMergeMode.Highlight) {
			if (Relicslot1.item.type != 0 && Relicslot2.item.type != 0) {
				if (Relicslot1.item.ModItem is Relic relic1 && Relicslot2.item.ModItem is Relic relic2) {
					if (relic1.RelicTier + relic2.RelicTier > 4) {
						mergeInfo.SetText("Excessing relic merge tier\nYou are attempting to create : " + relic1.RelicTier + relic2.RelicTier + " tier");
						return;
					}
					int cost = RelicMergeCost(relic1.RelicTier, relic2.RelicTier);
					if (modplayer.TransmutationPower < cost) {
						mergeInfo.SetText("Insufficient energy\nRequired energy cost : " + cost);
					}
					else {
						mergeInfo.SetText("Merging energy cost : " + cost);
					}
				}
			}
			else {
				mergeInfo.SetText("");
			}
		}
	}
	public static int GetItemRarityDB(int rare, int option) {
		switch (option) {
			case 1:
				return BossRushModSystem.Safe_GetWeaponRarity(rare);
			case 2:
				return BossRushModSystem.Safe_GetAccRarity(rare);
			case 3:
				return BossRushModSystem.Safe_GetHeadRarity(rare);
			case 4:
				return BossRushModSystem.Safe_GetBodyRarity(rare);
			case 5:
				return BossRushModSystem.Safe_GetLegsRarity(rare);
			default:
				return ItemID.None;
		}
	}
}
