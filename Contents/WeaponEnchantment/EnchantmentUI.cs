using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentUIState : UIState {
	public int WhoAmI = -1;
	public override void OnActivate() {
		if (WhoAmI == -1)
			return;
		Player player = Main.player[WhoAmI];
		EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack, player);
		slot.UISetWidthHeight(52, 52);
		slot.UISetPosition(player.Center + Vector2.UnitX * 100, new Vector2(26, 26));
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
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
			if (player.HeldItem.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length;
				Vector2 pos = new Vector2(Left.Pixels, Top.Pixels + 60);
				for (int i = 0; i < length; i++) {
					WeaponEnchantmentUIslot slot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack10, player);
					slot.UISetWidthHeight(52, 52);
					slot.UISetPosition(pos + Vector2.UnitX * 60 * i, new Vector2(26, 26));
					slot.WhoAmI = i;
				}
			}
		}
		else
		{
			if (item == null)
				return;
			Main.mouseItem = item;
			item = null;
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
				spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
}
public class WeaponEnchantmentUIslot : UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;
	public Texture2D textureDraw;

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
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
			if (itemOwner.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				globalItem.EnchantmenStlot[WhoAmI] = itemType;
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Main.instance.LoadItem(itemType);
				Texture2D texture = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture.Size() * .5f;
				spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
}
public class EnchantmentSystem : ModSystem {
	internal UserInterface userInterface;
	internal EnchantmentUIState Enchant_uiState;
	public static ModKeybind EnchantmentKeyBind { get; private set; }
	public static int SelectedInventorySlot = -1;
	public static int SelectedModifySlot = -1;
	public override void Load() {
		EnchantmentKeyBind = KeybindLoader.RegisterKeybind(Mod, "Enchantment UI", "L");

		//UI stuff
		if (!Main.dedServ) {
			Enchant_uiState = new();
			userInterface = new();
		}
	}
	public override void Unload() {
		EnchantmentKeyBind = null;
	}
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1) {
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: Weapon Enchantment",
				delegate {
					userInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				},
				InterfaceScaleType.UI)
			);
		}
	}
}
