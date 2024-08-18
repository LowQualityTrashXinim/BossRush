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
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.FrozenShark;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.SingleBarrelMinishark;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.LongerMusket;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.ManaStarFury;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.SnowballRifle;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.SnowballShotgunCannon;
using BossRush.Contents.Items.Weapon.NotSynergyWeapon.HuntingRifle;

namespace BossRush.Common.Systems;
public class TransmutationUIState : UIState {
	public override void OnActivate() {
		Elements.Clear();
		var panalUI = new UIPanel();
		panalUI.UISetWidthHeight(250, 150);
		panalUI.UISetPosition(Main.LocalPlayer.Center - new Vector2(0, -150), new Vector2(100, 100));
		Append(panalUI);
		var cardUI = new TransmutationUI(TextureAssets.InventoryBack, Main.LocalPlayer);
		var origin = new Vector2(26, 26);
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
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
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
	private Player player;
	public TransmutationUI(Asset<Texture2D> texture, Player player) : base(texture) {
		this.texture = texture.Value;
		this.player = player;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (item != null && Main.mouseItem.type != ItemID.None) {
			//Swap item here
			Item itemcache = Main.mouseItem.Clone();
			Main.mouseItem = item.Clone();
			player.inventory[58] = item.Clone();
			item = itemcache.Clone();
		}
		else if (Main.mouseItem.type != ItemID.None && item == null) {
			//When the slot is available
			if (Main.mouseItem.buffType != 0 && Main.mouseItem.stack > 1) {
				Main.mouseItem.stack--;
				item = Main.mouseItem.Clone();
				item.stack = 1;
				return;
			}
			item = Main.mouseItem.Clone();
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
		var drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
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
		if (item.Count < 2) {
			return false;
		}
		if (!item[0].IsAWeapon() || !item[1].IsAWeapon()) {
			return false;
		}
		if (item.Where(i => i.type == ItemID.Minishark || i.type == ItemID.IceBlade).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<FrozenShark>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.Minishark || i.type == ItemID.Musket).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SingleBarrelMinishark>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.Musket).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<LongerMusket>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.Starfury || i.type == ItemID.MagicMissile).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ManaStarFury>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.SnowballCannon || i.type == ItemID.Minishark).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SnowballRifle>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.SnowballCannon || i.type == ItemID.Boomstick || i.type == ItemID.QuadBarrelShotgun).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SnowballShotgunCannon>());
			return true;
		}
		else if (item.Where(i => i.type == ItemID.Musket || i.type == ItemID.Boomstick || i.type == ItemID.QuadBarrelShotgun).Count() == 2) {
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<HuntingRifle>());
			return true;
		}
		else {
			if (item[0].rare == item[1].rare && item[0].rare < ItemRarityID.Purple) {
				int rare = item[0].rare + 1;
				int itemSpawn = player.QuickSpawnItem(player.GetSource_DropAsItem(), Main.rand.Next(BossRushModSystem.WeaponRarityDB[rare]));
				Main.item[itemSpawn].ResetPrefix();
				return true;
			}
		}

		return false;
	}
}
