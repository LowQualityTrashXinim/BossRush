using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using ReLogic.Content;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentUIState : UIState {
	public int WhoAmI = -1;
	public override void OnActivate() {
		Elements.Clear();
		if (WhoAmI == -1)
			return;
		Player player = Main.player[WhoAmI];
		EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack, player);
		slot.UISetWidthHeight(52, 52);
		slot.UISetPosition(player.Center + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(slot);
	}
}
public class EnchantmentUIslot : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
	private Player player;
	public EnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
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
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length;
				for (int i = 0; i < length; i++) {
					WeaponEnchantmentUIslot slot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack, player);
					slot.UISetWidthHeight(52, 52);
					slot.UISetPosition(player.Center + Vector2.UnitY * 60 + Vector2.UnitX * 60 * i, new Vector2(26, 26));
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
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
				if (child is WeaponEnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
					//if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
					//	globalItem.EnchantmenStlot[wmslot.WhoAmI] = 
					//}
				}
				if (child is WeaponEnchantmentUIslot { itemOwner: not null }) {
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
public class WeaponEnchantmentUIslot : UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	private Player player;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
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
			if (EnchantmentLoader.GetEnchantmentItemID(itemType) == null)
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
}
