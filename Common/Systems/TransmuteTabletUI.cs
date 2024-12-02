using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.RelicItem;
using BossRush.Texture;

namespace BossRush.Common.Systems;
public class TransumtationRecipe {

}
public class TransmutationSystem : ModSystem {

}
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
		panel.Append(slot1);

		slot2 = new TransmutationUI(TextureAssets.InventoryBack);
		slot2.UISetWidthHeight(52, 52);
		slot2.HAlign = MathHelper.Lerp(.1f, .9f, 1 / 3f);
		slot2.VAlign = .9f;
		panel.Append(slot2);

		btn_confirm = new TransmutationUIConfirmButton(TextureAssets.InventoryBack10);
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.HAlign = MathHelper.Lerp(.1f, .9f, 2 / 3f);
		btn_confirm.VAlign = .9f;
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
		}
		else if (Main.mouseItem.type != ItemID.None && item == null || item.type == ItemID.None) {
			//When the slot is available
			item = Main.mouseItem.Clone();
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
	public override void LeftMouseDown(UIMouseEvent evt) {
		var resultlist = new List<TransmutationUI>();
		foreach (var element in Parent.Children) if (element is TransmutationUI transmutateResult) {
				if (transmutateResult.item == null || transmutateResult.item.type == ItemID.None)
					continue;
				resultlist.Add(transmutateResult);
			}
		var itemList = resultlist.Select(i => i.item).ToList();
		if (CheckForSpecialDrop(itemList)) {
			resultlist.ForEach(i => i.item = null);
			return;
		}
	}
	private bool CheckForSpecialDrop(List<Item> item) {
		if (item.Count < 2) {
			return false;
		}
		var player = Main.LocalPlayer;
		if (item.Where(i => i.ModItem is Relic).Count() > 1) {
			Relic relic1 = null;
			Relic relic2 = null;
			foreach (var it in item) if (it.ModItem is Relic re) {
					if (relic1 == null) {
						relic1 = re;
						continue;
					}
					if (relic2 == null) {
						relic2 = re;
						break;
					}
				}
			int count = relic1.TemplateCount + relic2.TemplateCount;
			if (relic1 != null && relic2 != null && count <= 4) {
				RelicTemplateLoader.MergeStat(relic1, relic2);
				player.QuickSpawnItem(player.GetSource_FromThis(), relic1.Item);
				return true;
			}
		}
		Item relicpreItem = item.Where(i => i.ModItem is Relic).FirstOrDefault();
		Relic relicItem = relicpreItem != null ? relicpreItem.ModItem as Relic : null;
		Item weaponItem = item.Where(i => i.IsAWeapon()).FirstOrDefault();
		Item accItem = item.Where(i => i.accessory).FirstOrDefault();
		Item headItem = item.Where(i => i.headSlot > 0 && !i.vanity).FirstOrDefault();
		Item bodyItem = item.Where(i => i.bodySlot > 0 && !i.vanity).FirstOrDefault();
		Item legItem = item.Where(i => i.legSlot > 0 && !i.vanity).FirstOrDefault();
		Item univitem = null;
		int Option = 0;
		if (weaponItem != null && weaponItem.rare < ItemRarityID.Purple - 2) {
			univitem = weaponItem;
			Option = 1;
		}
		if (accItem != null && accItem.rare < ItemRarityID.Purple - 2) {
			univitem = accItem;
			Option = 2;
		}
		if (headItem != null && headItem.rare < ItemRarityID.Purple - 2) {
			univitem = accItem;
			Option = 3;
		}
		if (bodyItem != null && bodyItem.rare < ItemRarityID.Purple - 2) {
			univitem = accItem;
			Option = 4;
		}
		if (legItem != null && legItem.rare < ItemRarityID.Purple - 2) {
			univitem = accItem;
			Option = 5;
		}
		if (relicItem != null) {
			if (Option != 0) {

				float chance = Main.rand.NextFloat();

				float rarityOffSet = univitem.rare * .03f;
				if (univitem.rare >= ItemRarityID.LightRed && relicItem.RelicTier > 2) {
					rarityOffSet += (univitem.rare - 3) * .02f;
				}
				chance += rarityOffSet;
				bool SuccessChance = false;
				switch (relicItem.RelicTier) {
					case 1:
						SuccessChance = chance <= Relic.chanceTier1;
						break;
					case 2:
						SuccessChance = chance <= Relic.chanceTier2;
						break;
					case 3:
						SuccessChance = chance <= Relic.chanceTier3;
						break;
					case 4:
						SuccessChance = chance <= Relic.chanceTier4;
						break;
					default:
						SuccessChance = chance <= Relic.chanceTier4 + .05f * relicItem.RelicTier;
						break;
				}
				int rare = ContentSamples.ItemsByType[univitem.type].rare;
				if (SuccessChance) {
					rare = ContentSamples.ItemsByType[univitem.type].rare + 1;
				}
				int itemType = GetItemRarityDB(rare, Option);
				if (itemType == ItemID.None) {
					Main.NewText($"Detected no rarity found ! at {rare} rarity at {Option} option");
					return false;
				}
				int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), itemType);
				if (Main.item[itemSpawn].CanHavePrefixes())
					Main.item[itemSpawn].ResetPrefix();
				return true;
			}
		}

		if (ContentSamples.ItemsByType[item[0].type].rare >= ItemRarityID.Purple || ContentSamples.ItemsByType[item[0].type].rare <= -1
			|| ContentSamples.ItemsByType[item[1].type].rare >= ItemRarityID.Purple || ContentSamples.ItemsByType[item[1].type].rare <= -1) {
			return false;
		}

		if (ContentSamples.ItemsByType[item[0].type].rare == ContentSamples.ItemsByType[item[1].type].rare) {
			int rare = ContentSamples.ItemsByType[item[0].type].rare;
			int itemType = GetItemRarityDB(rare, Option);
			if (itemType == ItemID.None) {
				Main.NewText($"Detected no rarity found ! at {rare} rarity at {Option} option");
				return false;
			}
			int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), itemType);
			if (Main.item[itemSpawn].CanHavePrefixes())
				Main.item[itemSpawn].ResetPrefix();
			return true;
		}
		else {
			int rare = ContentSamples.ItemsByType[item[0].type].rare;
			int rare2 = ContentSamples.ItemsByType[item[1].type].rare;
			int spawmItemType = Main.rand.Next(BossRushModSystem.WeaponRarityDB[rare]);
			int spawnItemType2 = Main.rand.Next(BossRushModSystem.WeaponRarityDB[rare2]);
			int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), Main.rand.NextBool() ? spawmItemType : spawnItemType2);
			if (Main.item[itemSpawn].CanHavePrefixes())
				Main.item[itemSpawn].ResetPrefix();
		}
		return true;
		//var itemType = item.Select(i => i.type);

		//if (itemType.Contains(ItemID.Minishark) && itemType.Contains(ItemID.IceBlade)) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<FrozenShark>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.Minishark || i.type == ItemID.Musket).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SingleBarrelMinishark>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.Musket).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<LongerMusket>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.Starfury || i.type == ItemID.MagicMissile).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ManaStarFury>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.SnowballCannon || i.type == ItemID.Minishark).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SnowballRifle>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.SnowballCannon || i.type == ItemID.Boomstick || i.type == ItemID.QuadBarrelShotgun).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SnowballShotgunCannon>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.Musket || i.type == ItemID.Boomstick || i.type == ItemID.QuadBarrelShotgun).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<HuntingRifle>());
		//	return true;
		//}
		//else if (item.Where(i => i.type == ItemID.EnchantedSword || i.type == ItemID.IceBlade).Count() == 2) {
		//	player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<FrozenEnchantedSword>());
		//	return true;
		//}
	}
	private int GetItemRarityDB(int rare, int option) {
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
