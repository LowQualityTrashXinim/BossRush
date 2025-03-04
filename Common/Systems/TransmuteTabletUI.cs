using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Transfixion.SoulBound;

namespace BossRush.Common.Systems;
public class TransmutationUIState : UIState {
	UIPanel panel;
	TransmutationUIConfirmButton btn_confirm;
	TransmutationUI slot1;
	TransmutationUI slot2;
	ExitUI btn_exit;
	UITextBox txtbox;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.UISetWidthHeight(450, 150);
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		Append(panel);

		slot1 = new TransmutationUI(TextureAssets.InventoryBack);
		slot1.UISetWidthHeight(52, 52);
		slot1.HAlign = MathHelper.Lerp(.1f, .9f, 0);
		slot1.VAlign = .9f;
		slot1.OnLeftClick += Slot_OnLeftClick;
		panel.Append(slot1);

		slot2 = new TransmutationUI(TextureAssets.InventoryBack);
		slot2.UISetWidthHeight(52, 52);
		slot2.HAlign = MathHelper.Lerp(.1f, .9f, 1 / 3f);
		slot2.VAlign = .9f;
		slot2.OnLeftClick += Slot_OnLeftClick;
		panel.Append(slot2);

		btn_confirm = new TransmutationUIConfirmButton(TextureAssets.InventoryBack10);
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.HAlign = MathHelper.Lerp(.1f, .9f, 2 / 3f);
		btn_confirm.VAlign = .9f;
		btn_confirm.OnLeftClick += Btn_confirm_OnLeftClick;
		panel.Append(btn_confirm);

		btn_exit = new ExitUI(TextureAssets.InventoryBack13);
		btn_exit.UISetWidthHeight(52, 52);
		btn_exit.HAlign = MathHelper.Lerp(.1f, .9f, 1f);
		btn_exit.VAlign = .9f;
		panel.Append(btn_exit);

		txtbox = new("");
		txtbox.UISetWidthHeight(450, 60);
		txtbox.TextHAlign = 0;
		txtbox.ShowInputTicker = false;
		panel.Append(txtbox);
	}

	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (slot1.item == null || slot2.item == null) {
			return;
		}
		if (TransmutationUIConfirmButton.SpecialInteraction(slot1.item, slot2.item)) {
			SoundEngine.PlaySound(SoundID.AchievementComplete with { Pitch = -1 });
		}
		else {
			SoundEngine.PlaySound(SoundID.AbigailSummon with { Pitch = -1 });
		}
	}

	private void Slot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		txtbox.SetTextMaxLength(255);
		txtbox.SetText("");
		if (slot1.item == null || slot2.item == null) {
			return;
		}
		float offsetchance = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().Transmutation_SuccessChance;
		if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().perks.ContainsKey(Perk.GetPerkType<Dirt>())) {
			offsetchance = .05f;
		}
		bool AnyRelic = slot1.item.ModItem is Relic || slot2.item.ModItem is Relic;
		if (slot1.item.ModItem is Relic re1 && slot2.item.ModItem is Relic re2) {
			txtbox.SetText("Chance to upgrade relic : " + RelicTemplateLoader.RelicValueToNumber(TransmutationUIConfirmButton.GetRelicChance(re1, re2, offsetchance) * 100) + "%");
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
			float rarityOffSet = slotitem.rare * .03f;
			if (slotitem.rare >= ItemRarityID.LightRed && relicItem.RelicTier > 2) {
				rarityOffSet += (slotitem.rare - 3) * .02f;
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
			txtbox.SetText($"Success rarity upgrade : {RelicTemplateLoader.RelicValueToNumber(Math.Clamp(SuccessChance - rarityOffSet + offsetchance, 0, 1f) * 100)}%");
		}
	}
}
public class ExitUI : UIImageButton {
	public ExitUI(Asset<Texture2D> texture) : base(texture) {
	}

	public override void LeftClick(UIMouseEvent evt) {
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.CrossSprite).Value;
		CalculatedStyle rect = this.GetDimensions();
		spriteBatch.Draw(texture, rect.Position() + texture.Size() * .5f, Color.White);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			Main.instance.MouseText("Exit ?");
		}
	}
}
public class TransmutationUI : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
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
		else if (Main.mouseItem.type != ItemID.None && item == null) {
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
		else if (Main.mouseItem.type == ItemID.None && item != null) {
			//When player want to change item
			Main.mouseItem = item.Clone();
			player.inventory[58] = item.Clone();
			item = null;
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
				player.inventory[i] = item;
				return;
			}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
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
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	public static float GetRelicChance(Relic relic1, Relic relic2, float chance = 0) {
		return Math.Clamp(1f - .15f * (relic1.RelicTier + relic2.RelicTier) + chance, .01f, 1f);
	}
	/// <summary>
	/// Please check null on your own
	/// </summary>
	/// <param name="item1"></param>
	/// <param name="item2"></param>
	/// <returns></returns>
	public static bool SpecialInteraction(Item item1, Item item2) {
		var player = Main.LocalPlayer;

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


		float offsetchance = player.GetModPlayer<PlayerStatsHandle>().Transmutation_SuccessChance;
		Relic relicItem = null;
		Item slotitem;
		Item slotitem2 = null;
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
				if (Main.rand.NextFloat() > GetRelicChance(relicItem, relic2, offsetchance) && !player.IsDebugPlayer()) {
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
		int rareval1 = slotitem.rare;

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
			int rareval2 = slotitem2.rare;
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
