using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Transfixion.SoulBound;
using BossRush.Texture;

namespace BossRush.Contents.Items.Toggle;
public class TransmuteTablet : ModItem {
	public override void SetDefaults() {
		Item.width = 32;
		Item.height = 32;
		Item.useTime = 15;
		Item.useAnimation = 15;
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
public class TransmutationUIState : UIState {
	UIPanel panel;
	TransmutationUIConfirmButton btn_confirm;
	TransmutationUI slot1;
	TransmutationUI slot2;
	ExitUI btn_exit;
	UITextBox txtbox;
	UITextBox CurrentMode;
	UITextBox transmutateText;
	UIPanel panelToBeDrawnOnTop;
	UIPanel slotPanel;
	UIPanel headerPanel;
	UIImageButton sa_btn;
	UIPanel littleButtonPanel;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.UISetWidthHeight(450, 250);
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		Append(panel);

		headerPanel = new();
		headerPanel.Width.Percent = 1f;
		headerPanel.Height.Pixels = 60f;
		headerPanel.SetPadding(4);
		panel.Append(headerPanel);

		txtbox = new("Transmutate tablet remain dormant...");
		txtbox.UISetWidthHeight(450, 30);
		txtbox.SetTextMaxLength(255);
		txtbox.MarginTop = headerPanel.Height.Pixels + 10;
		txtbox.TextHAlign = 0;
		txtbox.ShowInputTicker = false;
		panel.Append(txtbox);

		float marginTop = headerPanel.Height.Pixels + txtbox.Height.Pixels + 30;

		CurrentMode = new("Transmutation Mode");
		CurrentMode.VAlign = .5f;
		CurrentMode.MarginLeft = 10;
		CurrentMode.TextHAlign = 0;
		CurrentMode.Width.Percent = 1;
		CurrentMode.Width.Pixels = -72;
		CurrentMode.ShowInputTicker = false;
		headerPanel.Append(CurrentMode);

		slotPanel = new();
		slotPanel.MarginTop = marginTop;
		slotPanel.Width.Precent = .55f;
		slotPanel.Height.Pixels = 76;
		slotPanel.HAlign = 1f;
		slotPanel.BackgroundColor = Color.Gray;
		panel.Append(slotPanel);

		transmutateText = new("");
		transmutateText.Width.Percent = 1;
		transmutateText.Width.Pixels -= slotPanel.Width.GetValue(panel.GetInnerDimensions().Width) + 20;
		transmutateText.MarginTop = marginTop;
		transmutateText.TextHAlign = 0;
		transmutateText.ShowInputTicker = false;
		panel.Append(transmutateText);

		slot1 = new TransmutationUI(TextureAssets.InventoryBack);
		slot1.UISetWidthHeight(52, 52);
		slot1.OnLeftClick += Slot_OnLeftClick;
		slotPanel.Append(slot1);

		slot2 = new TransmutationUI(TextureAssets.InventoryBack);
		slot2.UISetWidthHeight(52, 52);
		slot2.HAlign = .5f;
		slot2.OnLeftClick += Slot_OnLeftClick;
		slotPanel.Append(slot2);

		btn_confirm = new TransmutationUIConfirmButton(TextureAssets.InventoryBack10);
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.HAlign = 1f;
		btn_confirm.OnLeftClick += Btn_confirm_OnLeftClick;
		slotPanel.Append(btn_confirm);

		btn_exit = new ExitUI(TextureAssets.InventoryBack13);
		btn_exit.UISetWidthHeight(52, 52);
		btn_exit.HAlign = 1f;
		headerPanel.Append(btn_exit);

		littleButtonPanel = new();
		littleButtonPanel.Width.Percent = 1;
		littleButtonPanel.Width.Pixels -= slotPanel.Width.GetValue(panel.GetInnerDimensions().Width) + 20;
		littleButtonPanel.Height.Pixels = 28;
		littleButtonPanel.SetPadding(5f);
		littleButtonPanel.PaddingLeft = 15;
		littleButtonPanel.MarginTop += transmutateText.MarginTop + littleButtonPanel.Height.Pixels + 20;
		panel.Append(littleButtonPanel);

		sa_btn = new(ModContent.Request<Texture2D>(BossRushTexture.Boxes));
		sa_btn.UISetWidthHeight(16, 16);
		sa_btn.OnLeftClick += Sa_btn_OnLeftClick;
		sa_btn.VAlign = .5f;
		sa_btn.SetVisibility(.8f, 1f);
		littleButtonPanel.Append(sa_btn);

		panelToBeDrawnOnTop = new();
		panelToBeDrawnOnTop.VAlign = 1;
		panelToBeDrawnOnTop.Width.Percent = 1;
		panelToBeDrawnOnTop.Height.Pixels = 16;
		panelToBeDrawnOnTop.BorderColor = new(0, 0, 0, 255);
		panel.Append(panelToBeDrawnOnTop);
	}
	public bool SwitchModeTo_TransmutationConvertPower = false;
	private void Sa_btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SwitchModeTo_TransmutationConvertPower = !SwitchModeTo_TransmutationConvertPower;
	}

	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (TransmutationUIConfirmButton.SpecialInteraction(slot1.item, slot2.item)) {
			SoundEngine.PlaySound(SoundID.AchievementComplete with { Pitch = -1 });
		}
		else {
			SoundEngine.PlaySound(SoundID.AbigailSummon with { Pitch = -1 });
		}
	}

	private void Slot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		PlayerStatsHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		Rectangle rect = panelToBeDrawnOnTop.GetInnerDimensions().ToRectangle();
		Point point = new(rect.X, rect.Y);
		BossRushUtils.DrawProgressLine(modplayer.TransmutationPower, modplayer.TransmutationPowerMaximum, point, rect.TopLeft().ToPoint(), 0, new(0, -6, rect.Width, 4), spriteBatch, Color.Wheat, Color.White);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		PlayerStatsHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
		if (sa_btn.IsMouseHovering) {
			sa_btn.Disable_MouseItemUsesWhenHoverOverAUI();
			if (!SwitchModeTo_TransmutationConvertPower) {
				CurrentMode.SetText("Transmutation Mode");
				Main.instance.MouseText("Switch to charging power up mode");
			}
			else {
				CurrentMode.SetText("Powering Mode");
				Main.instance.MouseText("Switch to transmutation mode");
			}
		}
		transmutateText.SetText($"Power : {modplayer.TransmutationPower} / {modplayer.TransmutationPowerMaximum}");
		if (SwitchModeTo_TransmutationConvertPower) {
			txtbox.SetText("Convert item into tablet power");
			return;
		}
		txtbox.SetText("Transmutation tablet remain dormant...");
		if (slot1.item.type == ItemID.None || slot2.item.type == ItemID.None) {
			if (slot1.item.type == ItemID.None && slot2.item.type == ItemID.None) {
				return;
			}
			txtbox.SetText("Transmute the following item ...");
			return;
		}
		float offsetchance = modplayer.Transmutation_SuccessChance;
		bool AnyRelic = slot1.item.ModItem is Relic || slot2.item.ModItem is Relic;
		if (slot1.item.ModItem is Relic re1 && slot2.item.ModItem is Relic re2) {
			txtbox.SetText("Chance to merge relic : " + RelicTemplateLoader.RelicValueToNumber(TransmutationUIConfirmButton.GetRelicChance(re1, re2, offsetchance) * 100) + "%");
			return;
		}
		Item item1 = slot1.item;
		Item item2 = slot2.item;
		if (AnyRelic &&
			(item1.IsAWeapon() || item2.IsAWeapon()
			|| ((item1.accessory || item2.accessory)
			|| (item1.headSlot > 0 || item2.headSlot > 0)
			|| (item1.bodySlot > 0 || item2.bodySlot > 0)
			|| (item1.legSlot > 0 || item2.legSlot > 0) && (!item1.vanity && !item2.vanity)))) {

			Relic relicItem;
			Item slotitem;
			if (slot1.item.ModItem is Relic) {
				relicItem = slot1.item.ModItem as Relic;
				slotitem = slot2.item;
			}
			else {
				relicItem = slot2.item.ModItem as Relic;
				slotitem = slot1.item;
			}
			float rarityOffSet;
			if (slotitem.rare >= ItemRarityID.LightRed && relicItem.RelicTier > 2) {
				rarityOffSet = ItemRarityID.Orange * .03f + (slotitem.OriginalRarity - 3) * .02f;
			}
			else {
				rarityOffSet = slotitem.OriginalRarity * .03f;
			}
			float SuccessChance;
			switch (relicItem.RelicTier) {
				case 1:
					SuccessChance = Relic.chanceTier1;
					break;
				case 2:
					SuccessChance = Relic.chanceTier2;
					break;
				case 3:
					SuccessChance = Relic.chanceTier3;
					break;
				case 4:
					SuccessChance = Relic.chanceTier4;
					break;
				default:
					SuccessChance = Relic.chanceTier4 + .05f * relicItem.RelicTier;
					break;
			}
			SuccessChance += offsetchance;
			txtbox.SetText($"Success item rarity upgrade : {RelicTemplateLoader.RelicValueToNumber(Math.Clamp(SuccessChance - rarityOffSet + offsetchance, 0, 1f) * 100)}%");
		}
		else {
			txtbox.SetText($"Transmuting to different item of same rarity");
		}
	}
}
public class TransmutationUI : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item = new();
	public int Timer = 0;
	private Texture2D texture;
	public int itemToShow = -1;
	public TransmutationUI(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		if (item != null && Main.mouseItem.type != ItemID.None) {
			//Swap item here
			Item itemcache = Main.mouseItem.Clone();
			Main.mouseItem = item.Clone();
			player.inventory[58] = item.Clone();
			item = itemcache.Clone();
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 1 });
		}
		else if (Main.mouseItem.type != ItemID.None && item.type == ItemID.None) {
			//When the slot is available
			item = Main.mouseItem.Clone();
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 1 });
			if (Main.mouseItem.buffType != 0 && Main.mouseItem.stack > 1) {
				Main.mouseItem.stack--;
				item.stack = 1;
				return;
			}
			if (Main.mouseItem.stack > 1) {
				Main.mouseItem.stack--;
				player.inventory[58].stack--;
			}
			else {
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
			}
		}
		else if (Main.mouseItem.type == ItemID.None && item.type != ItemID.None) {
			//When player want to change item
			Main.mouseItem = item.Clone();
			player.inventory[58] = item.Clone();
			item.TurnToAir();
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = -.5f });
		}
		else {
			//Do nothing lmao
		}
	}
	public override void OnDeactivate() {
		Player player = Main.LocalPlayer;
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) if (player.CanItemSlotAccept(player.inventory[i], item)) {
				player.inventory[i] = item.Clone();
				item.TurnToAir();
				return;
			}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
	}
	public override void Update(GameTime gameTime) {
		Timer = BossRushUtils.Safe_SwitchValue(Timer, 120);
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
		if (Timer == 120) {
			int cached = itemToShow;
			while (itemToShow == cached) {
				itemToShow = Main.rand.Next(new int[] { ItemID.SilverBroadsword, ItemID.SilverBow, ItemID.SapphireStaff, ItemID.FlinxStaff, ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves, ItemID.AvengerEmblem, ModContent.ItemType<Relic>() });
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		var drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
		base.Draw(spriteBatch);
		try {
			if (item != null) {
				if (IsMouseHovering) {
					Main.HoverItem = item.Clone();
					Main.hoverItemName = item.HoverName;
				}
				Main.instance.LoadItem(item.type);
				var texture = TextureAssets.Item[item.type].Value;
				var origin = texture.Size() * .5f;
				float scaling = ScaleCalculation(texture.Size());
				spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);

				if (itemToShow != -1) {
					Main.instance.LoadItem(itemToShow);
					var tex = TextureAssets.Item[itemToShow].Value;
					var origin2 = tex.Size() * .5f;
					float scaling2;
					if (itemToShow == ModContent.ItemType<Relic>()) {
						scaling2 = .5f;
					}
					else {
						scaling2 = ScaleCalculation(texture.Size());
					}
					spriteBatch.Draw(tex, drawpos, null, Color.Gray * .6f, 0, origin2, scaling2, SpriteEffects.None, 0);
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) {
		Vector2 origin = texture.Size();
		float multiplier = 3;
		if (origin.X <= textureSize.X && origin.Y <= textureSize.Y) {
			multiplier = 1;
		}
		float length = origin.Length();
		float length2 = textureSize.Length();
		return length / (length2 * multiplier);
	}
}
public class TransmutationUIConfirmButton : UIImageButton {
	public TransmutationUIConfirmButton(Asset<Texture2D> texture) : base(texture) {
		SetVisibility(.67f, 1f);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
		if (IsMouseHovering) {
			Main.instance.MouseText("Transmute");
		}
	}
	public static float GetRelicChance(Relic relic1, Relic relic2, float chance = 0) {
		return Math.Clamp(1f - .15f * (relic1.RelicTier + relic2.RelicTier) + chance, .01f, 1f);
	}
	private static bool TransmutationPowerChargeUp(ref Item item, PlayerStatsHandle modplayer) {
		if (modplayer.TransmutationPower < modplayer.TransmutationPowerMaximum) {
			modplayer.TransmutationPower++;
			item.TurnToAir();
			return true;
		}
		return false;
	}
	/// <summary>
	/// Please check null on your own
	/// </summary>
	/// <param name="item1"></param>
	/// <param name="item2"></param>
	/// <returns></returns>
	public static bool SpecialInteraction(Item item1, Item item2) {
		var player = Main.LocalPlayer;
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (ModContent.GetInstance<UniversalSystem>().transmutationUI.SwitchModeTo_TransmutationConvertPower) {
			bool verify = false;
			if (item1.type != ItemID.None) {
				if (TransmutationPowerChargeUp(ref item1, modplayer))
					verify = true;
			}
			if (item2.type != ItemID.None) {
				if (TransmutationPowerChargeUp(ref item2, modplayer))
					verify = true;
			}
			return verify;
		}
		if (item1.ModItem != null || item2.ModItem != null) {
			BaseSoulBoundItem soul = null;
			Item armor = null;
			if (item1.ModItem is BaseSoulBoundItem soulbound) {
				soul = soulbound;
				armor = item2;
			}
			else if (item2.ModItem is BaseSoulBoundItem soulbound2) {
				soul = soulbound2;
				armor = item1;
			}
			if (armor != null && SoulBoundPlayer.IsSoulBoundable(armor) && soul != null) {
				SoulBoundGlobalItem.AddSoulBound(ref armor, soul.SoulBoundType);
				soul.Item.TurnToAir();
				return true;
			}
		}
		float offsetchance = modplayer.Transmutation_SuccessChance;
		if (modplayer.TransmutationPower > 0) {
			modplayer.TransmutationPower--;
			offsetchance += 1;
		}
		Relic relicItem = null;
		Item slotitem = new();
		Item slotitem2 = new();
		if (item1.ModItem != null && item1.ModItem is Relic relic) {
			relicItem = relic;
		}
		if (item2.ModItem != null && item2.ModItem is Relic relic2) {
			if (relicItem == null) {
				relicItem = relic2;
				slotitem = item1;
			}
			else {
				int count = relicItem.TemplateCount + relic2.TemplateCount;
				if (count > 4) {
					return false;
				}
				if (Main.rand.NextFloat() <= GetRelicChance(relicItem, relic2, offsetchance) && !player.IsDebugPlayer()) {
					item1.TurnToAir();
					item2.TurnToAir();
					return false;
				}
				RelicTemplateLoader.MergeStat(relicItem, relic2);
				return true;
			}
		}
		else {
			slotitem = item1;
			slotitem2 = item2;
		}
		int Option = 0;
		int rareval1 = ContentSamples.ItemsByType[slotitem.type].rare;

		//Slot item are never null so no need to check for them
		if (rareval1 < ItemRarityID.Purple - 2) {
			if (slotitem.IsAWeapon()) {
				Option = 1;
			}
			if (!slotitem.vanity) {
				if (slotitem.accessory) {
					Option = 2;
				}
				else if (slotitem.headSlot > 0) {
					Option = 3;
				}
				else if (slotitem.bodySlot > 0) {
					Option = 4;
				}
				else if (slotitem.legSlot > 0) {
					Option = 5;
				}
			}
		}

		//Upgrading item rarity, it is still imporant to check for null here
		if (relicItem != null) {
			if (Option != 0) {
				float chance = Main.rand.NextFloat();

				float rarityOffSet = rareval1 * .03f;
				if (rareval1 >= ItemRarityID.LightRed && relicItem.RelicTier > 2) {
					rarityOffSet += (rareval1 - 3) * .02f;
				}
				//Look, the idea is certainly there, I just wonder to my past self why tf you do this ?
				chance += rarityOffSet - offsetchance;
				bool SuccessChance;
				if (relicItem.RelicTier > 4) {
					SuccessChance = chance <= Relic.chanceTier4 + .05f * relicItem.RelicTier;
				}
				else {
					SuccessChance = chance <= Relic.GetTierChance(relicItem.RelicTier);
				}
				int rare = rareval1;
				if (SuccessChance) {
					rare++;
				}
				int itemType = GetItemRarityDB(rare, Option);
				if (itemType == ItemID.None) {
					Main.NewText($"Detected no rarity found ! at {rare} rarity at {Option} option");
					return false;
				}
				int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), itemType);
				if (Main.item[itemSpawn].CanHavePrefixes())
					Main.item[itemSpawn].ResetPrefix();
				item1.TurnToAir();
				item2.TurnToAir();
				return true;
			}
			else {
				return false;
			}
		}
		else {//Equivalent exchange
			int rareval2 = ContentSamples.ItemsByType[slotitem2.type].rare;
			int Option2 = 0;
			if (rareval2 < ItemRarityID.Purple - 2) {
				if (slotitem2.IsAWeapon()) {
					Option2 = 1;
				}
				if (!slotitem2.vanity) {
					if (slotitem2.accessory) {
						Option2 = 2;
					}
					else if (slotitem2.headSlot > 0) {
						Option2 = 3;
					}
					else if (slotitem2.bodySlot > 0) {
						Option2 = 4;
					}
					else if (slotitem2.legSlot > 0) {
						Option2 = 5;
					}
				}
			}
			if (Option2 == 0 || Option != Option2) {
				return false;
			}
			//Changing to other same tier item
			if (rareval1 == rareval2) {
				int itemType = GetItemRarityDB(rareval1, Option);
				if (itemType == ItemID.None) {
					Main.NewText($"Detected no rarity found ! at {rareval1} rarity at {Option} option");
					return false;
				}
				int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), itemType);
				if (Main.item[itemSpawn].CanHavePrefixes())
					Main.item[itemSpawn].ResetPrefix();
				item1.TurnToAir();
				item2.TurnToAir();
				return true;
			}
			else {//Choosing between 2 different rarities
				int spawmItemType = Main.rand.Next(BossRushModSystem.WeaponRarityDB[rareval1]);
				int spawnItemType2 = Main.rand.Next(BossRushModSystem.WeaponRarityDB[rareval2]);
				int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), Main.rand.NextBool() ? spawmItemType : spawnItemType2);
				if (Main.item[itemSpawn].CanHavePrefixes())
					Main.item[itemSpawn].ResetPrefix();
				item1.TurnToAir();
				item2.TurnToAir();
			}
		}
		return true;
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
