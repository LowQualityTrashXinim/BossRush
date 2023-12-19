using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using System.Reflection;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentUIState : UIState {
	public int WhoAmI = -1;
	public UIText toolTip;
	public override void OnActivate() {
		Elements.Clear();
		if (WhoAmI == -1)
			return;
		Player player = Main.player[WhoAmI];
		WeaponEnchantmentUIslot slot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack, player);
		slot.UISetWidthHeight(52, 52);
		slot.UISetPosition(player.Center + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(slot);
		toolTip = new UIText("");
		Append(toolTip);
		var exitUI = new ExitUI(TextureAssets.InventoryBack13);
		exitUI.UISetWidthHeight(52, 52);
		exitUI.UISetPosition(player.Center + Vector2.UnitX * 178, new Vector2(26, 26));
		Append(exitUI);
		var InfoUI = new EnchantmentInfoUI(TextureAssets.InventoryBack14, player);
		InfoUI.UISetWidthHeight(52, 52);
		InfoUI.UISetPosition(player.Center + Vector2.UnitX * -120, new Vector2(26, 26));
		Append(InfoUI);
	}
}
public class WeaponEnchantmentUIslot : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
	private Player player;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
		this.player = player;
		this.texture = texture.Value;
	}
	List<int> textUqID = new List<int>();
	public override void LeftClick(UIMouseEvent evt) {
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			item = Main.mouseItem.Clone();
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length;
				for (int i = 0; i < length; i++) {
					Vector2 pos = player.Center + Vector2.UnitY * 60 + Vector2.UnitX * 60 * i;
					EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack, player);
					slot.UISetWidthHeight(52, 52);
					slot.UISetPosition(pos, new Vector2(26, 26));
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
					UIText text = new UIText($"{i + 1}");
					text.UISetPosition(pos + Vector2.UnitY * 56, new Vector2(26, 26));
					textUqID.Add(text.UniqueId);
					Parent.Append(text);
				}
			}
		}
		else {
			if (item == null)
				return;
			Main.mouseItem = item;
			item = null;
			int count = Parent.Children.Count();
			for (int i = count - 1; i >= 0; i--) {
				UIElement child = Parent.Children.ElementAt(i);
				if (child is EnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
				}
				if (child is EnchantmentUIslot { itemOwner: not null }) {
					child.Deactivate();
					child.Remove();
				}
				if (child is UIText text && textUqID.Contains(text.UniqueId)) {
					textUqID.Remove(text.UniqueId);
					child.Deactivate();
					child.Remove();
				}
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
public class EnchantmentInfoUI : UIImage {
	public Item item;
	public int itemType = 0;
	private Player player;
	private Texture2D texture;
	public EnchantmentInfoUI(Asset<Texture2D> texture, Player player) : base(texture) {
		this.player = player;
		this.texture = texture.Value;
	}

	public override void LeftClick(UIMouseEvent evt) {
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			item = Main.mouseItem.Clone();
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
		}
		else {
			if (item == null)
				return;
			Main.mouseItem = item.Clone();
			item = null;
			foreach (var el in Parent.Children) {
				if (el is UIText toolTip) {
					if (toolTip is null)
						return;
					toolTip.SetText("");
				}
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
		foreach (var el in Parent.Children) {
			if (el is UIText toolTip) {
				if (toolTip is null)
					return;
				toolTip.SetText("");
			}
		}
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
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (item == null)
			return;
		try {
			foreach (var el in Parent.Children) {
				if (el is UIText toolTip) {
					if (toolTip is null) {
						return;
					}
					if (IsMouseHovering) {
						FieldInfo textIsLarge = typeof(UIText).GetField("_isLarge", BindingFlags.NonPublic | BindingFlags.Instance);
						DynamicSpriteFont font = ((bool)textIsLarge.GetValue(el) ? FontAssets.DeathText : FontAssets.MouseText).Value;
						string tooltipText = "No enchantment can be found";
						if (EnchantmentLoader.GetEnchantmentItemID(item.type) != null) {
							tooltipText = EnchantmentLoader.GetEnchantmentItemID(item.type).Description;
						}
						Vector2 size = ChatManager.GetStringSize(font, tooltipText, Vector2.One);
						size.X *= .5f;
						toolTip.UISetPosition(new Vector2(Left.Pixels, Top.Pixels) - size);
						toolTip.SetText(tooltipText);
					}
					else {
						if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
							toolTip.SetText("");
						}
					}
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
}
public class EnchantmentUIslot : UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	private Player player;
	public EnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
		this.player = player;
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
			if (itemOwner.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				globalItem.EnchantmenStlot[WhoAmI] = itemType;
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (itemType == ItemID.None)
			return;
		try {
			foreach (var el in Parent.Children) {
				if (el is UIText toolTip) {
					if (toolTip is null) {
						return;
					}
					if (IsMouseHovering) {
						FieldInfo textIsLarge = typeof(UIText).GetField("_isLarge", BindingFlags.NonPublic | BindingFlags.Instance);
						DynamicSpriteFont font = ((bool)textIsLarge.GetValue(el) ? FontAssets.DeathText : FontAssets.MouseText).Value;
						string tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
						Vector2 size = ChatManager.GetStringSize(font, tooltipText, Vector2.One);
						size.X *= .5f;
						toolTip.UISetPosition(new Vector2(Left.Pixels, Top.Pixels) - size);
						toolTip.SetText(tooltipText);
					}
					else {
						if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
							toolTip.SetText("");
						}
					}
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
}
