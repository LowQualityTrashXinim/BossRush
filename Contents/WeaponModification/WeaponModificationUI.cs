using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using System.Linq;
using ReLogic.Content;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.WeaponModification {
	public class ImprovisedUIpanel : UIElement {
		public int _cornerSize = 12;

		public int _barSize = 4;

		public Asset<Texture2D> _borderTexture;

		private Asset<Texture2D> _backgroundTexture;

		public Color BorderColor = Color.Black;

		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;

		private bool _needsTextureLoading;
		public int ImprovisedUIpanel_Width = 0;
		private void LoadTextures() {
			if (_borderTexture == null) {
				_borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder");
			}

			if (_backgroundTexture == null) {
				_backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
			}
		}

		public ImprovisedUIpanel() {
			SetPadding(_cornerSize);
			_needsTextureLoading = true;
		}

		public ImprovisedUIpanel(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4) {
			if (_borderTexture == null) {
				_borderTexture = customborder;
			}

			if (_backgroundTexture == null) {
				_backgroundTexture = customBackground;
			}

			_cornerSize = customCornerSize;
			_barSize = customBarSize;
			SetPadding(_cornerSize);
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color) {
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize + ImprovisedUIpanel_Width);
			int width = point2.X - point.X - _cornerSize;
			int height = point2.Y - point.Y - _cornerSize;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (_needsTextureLoading) {
				_needsTextureLoading = false;
				LoadTextures();
			}

			if (_backgroundTexture != null) {
				DrawPanel(spriteBatch, _backgroundTexture.Value, BackgroundColor);
			}

			if (_borderTexture != null) {
				DrawPanel(spriteBatch, _borderTexture.Value, BorderColor);
			}
		}
	}
	public class ImprovisedUIpanelTextBox<T> : ImprovisedUIpanel {
		protected T _text;

		protected float _textScale = 1f;

		protected Vector2 _textSize = Vector2.Zero;

		protected bool _isLarge;

		protected Color _color = Color.White;

		protected bool _drawPanel = true;

		public float TextHAlign = 0.5f;

		public bool HideContents;

		private string _asterisks;

		public bool IsLarge => _isLarge;

		public bool DrawPanel {
			get {
				return _drawPanel;
			}
			set {
				_drawPanel = value;
			}
		}

		public float TextScale {
			get {
				return _textScale;
			}
			set {
				_textScale = value;
			}
		}

		public Vector2 TextSize { get => _textSize; set => _textSize = value; }

		public string Text {
			get {
				if (_text != null) {
					return _text.ToString();
				}

				return "";
			}
		}

		public Color TextColor {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		public ImprovisedUIpanelTextBox(T text, float textScale = 1f, bool large = false) {
			SetText(text, textScale, large);
		}

		public override void Recalculate() {
			SetText(_text, _textScale, _isLarge);
			base.Recalculate();
		}

		public void SetText(T text) {
			SetText(text, _textScale, _isLarge);
		}

		public virtual void SetText(T text, float textScale, bool large) {
			Vector2 stringSize = ChatManager.GetStringSize(large ? FontAssets.DeathText.Value : FontAssets.MouseText.Value, text.ToString(), new Vector2(textScale));
			stringSize.Y = (large ? 32f : 16f) * textScale;
			_text = text;
			_textScale = textScale;
			_textSize = stringSize;
			_isLarge = large;
			MinWidth.Set(stringSize.X + PaddingLeft + PaddingRight, 0f);
			MinHeight.Set(stringSize.Y + PaddingTop + PaddingBottom, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (_drawPanel) {
				base.DrawSelf(spriteBatch);
			}

			DrawText(spriteBatch);
		}

		protected void DrawText(SpriteBatch spriteBatch) {
			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 pos = innerDimensions.Position();
			if (_isLarge) {
				pos.Y -= 10f * _textScale * _textScale;
			}
			else {
				pos.Y -= 2f * _textScale;
			}

			pos.X += (innerDimensions.Width - _textSize.X) * TextHAlign;
			string text = Text;
			if (HideContents) {
				if (_asterisks == null || _asterisks.Length != text.Length) {
					_asterisks = new string('*', text.Length);
				}

				text = _asterisks;
			}

			if (_isLarge) {
				Utils.DrawBorderStringBig(spriteBatch, text, pos, _color, _textScale);
			}
			else {
				Utils.DrawBorderString(spriteBatch, text, pos, _color, _textScale);
			}
		}
	}
	public class ImprovisedUITextBox : ImprovisedUIpanelTextBox<string> {
		private int _cursor;

		private int _frameCount;

		private int _maxLength = 20;

		public bool ShowInputTicker = true;

		public bool HideSelf;

		public ImprovisedUITextBox(string text, float textScale = 1f, bool large = false)
			: base(text, textScale, large) {
		}

		public void Write(string text) {
			SetText(base.Text.Insert(_cursor, text));
			_cursor += text.Length;
		}

		public override void SetText(string text, float textScale, bool large) {
			if (text == null) {
				text = "";
			}

			if (text.Length > _maxLength) {
				text = text.Substring(0, _maxLength);
			}

			base.SetText(text, textScale, large);
			_cursor = Math.Min(base.Text.Length, _cursor);
		}

		public void SetTextMaxLength(int maxLength) {
			_maxLength = maxLength;
		}

		public void Backspace() {
			if (_cursor != 0) {
				SetText(base.Text.Substring(0, base.Text.Length - 1));
			}
		}

		public void CursorLeft() {
			if (_cursor != 0) {
				_cursor--;
			}
		}

		public void CursorRight() {
			if (_cursor < base.Text.Length) {
				_cursor++;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (HideSelf) {
				return;
			}

			_cursor = base.Text.Length;
			base.DrawSelf(spriteBatch);
			_frameCount++;
			if ((_frameCount %= 40) <= 20 && ShowInputTicker) {
				CalculatedStyle innerDimensions = GetInnerDimensions();
				Vector2 pos = innerDimensions.Position();
				Vector2 vector = new Vector2((base.IsLarge ? FontAssets.DeathText.Value : FontAssets.MouseText.Value).MeasureString(base.Text.Substring(0, _cursor)).X, base.IsLarge ? 32f : 16f) * base.TextScale;
				if (base.IsLarge) {
					pos.Y -= 8f * base.TextScale;
				}
				else {
					pos.Y -= 2f * base.TextScale;
				}

				pos.X += (innerDimensions.Width - base.TextSize.X) * TextHAlign + vector.X - (base.IsLarge ? 8f : 4f) * base.TextScale + 6f;
				if (base.IsLarge) {
					Utils.DrawBorderStringBig(spriteBatch, "|", pos, base.TextColor, base.TextScale);
				}
				else {
					Utils.DrawBorderString(spriteBatch, "|", pos, base.TextColor, base.TextScale);
				}
			}
		}
	}
	/// <summary>
	/// This is the UI where we will handle all the stupid logic<br/>
	/// it should only be working on client side only and always load so make sure to use TryGet"X" here<br/>
	/// I will work with UI state as that allow us to change state of UI whenever active and deactive
	/// </summary>
	public class WeaponModificationUI : UIState {
		public int whoAmI = -1;
		Player player;
		public override void OnActivate() {
			Elements.Clear();
			if (whoAmI == -1)
				return;
			Player player = Main.player[whoAmI];
			if (player.TryGetModPlayer(out WeaponModificationPlayer modplayer)) {
				this.player = player;
				Vector2 originDefault = new Vector2(26, 26);
				int maxLengthX = 550;
				WeaponModificationWeaponUISlot wpUI = new WeaponModificationWeaponUISlot(TextureAssets.InventoryBack2, player);
				wpUI.UISetPosition(player.Center + new Vector2(100, 40), originDefault);
				Append(wpUI);
				for (int i = 0; i < modplayer.WeaponModification_inventory.Length; i++) {
					Vector2 offset = new Vector2(MathHelper.Lerp(-maxLengthX, maxLengthX, i / (modplayer.WeaponModification_inventory.Length - 1f)), 100);
					Vector2 position = player.Center + offset;
					if (i >= (modplayer.WeaponModification_inventory.Length - 1) * .5f) {
						position -= new Vector2(maxLengthX, -55);
					}
					WeaponModificationUIslot inventory = new WeaponModificationUIslot(TextureAssets.InventoryBack, player);
					inventory.WhoAmI = i;
					inventory.ModificationType = modplayer.WeaponModification_inventory[i];
					inventory.UISetWidthHeight(52, 52);
					inventory.UISetPosition(position, originDefault);
					Append(inventory);
				}
			}
			DeletionSlot delslot = new DeletionSlot(TextureAssets.InventoryBack11, player);
			delslot.UISetWidthHeight(52, 52);
			delslot.UISetPosition(player.Center + new Vector2(-580, 155), new Vector2(26, 26));
			Append(delslot);
		}
		public override void OnDeactivate() {
			WeaponModificationSystem.SelectedInventorySlot = -1;
			WeaponModificationSystem.SelectedModifySlot = -1;
			if (player == null)
				return;
			if (player.TryGetModPlayer(out WeaponModificationPlayer modplayer)) {
				foreach (var element in Elements) {
					if (element is WeaponModificationUIslot wmUISlot && wmUISlot != null) {
						if (wmUISlot.WeaponModificationType != null) {
							if (wmUISlot.item == null) {
								modplayer.WeaponModification_inventory[wmUISlot.WhoAmI] = (int)wmUISlot.WeaponModificationType;
							}
						}
					}
				}
			}
		}
		public override void LeftMouseDown(UIMouseEvent evt) {
			base.LeftMouseDown(evt);
		}
	}
	public class DeletionSlot : UIImage {
		Player player;
		Texture2D texture;
		public DeletionSlot(Asset<Texture2D> texture, Player player) : base(texture) {
			this.player = player;
			this.texture = texture.Value;
		}
		public override void LeftClick(UIMouseEvent evt) {
			if (WeaponModificationSystem.SelectedInventorySlot != -1) {
				player.GetModPlayer<WeaponModificationPlayer>().WeaponModification_inventory[WeaponModificationSystem.SelectedInventorySlot] = -1;
				WeaponModificationSystem.SelectedInventorySlot = -1;
			}
			if (WeaponModificationSystem.SelectedModifySlot != -1) {
				WeaponModificationUIslot UIslot = (WeaponModificationUIslot)Parent.Children.Where(e => e is WeaponModificationUIslot { item: not null } slot && slot.WhoAmI == WeaponModificationSystem.SelectedModifySlot).FirstOrDefault();
				if (UIslot.item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
					globalItem.ModWeaponSlotType[WeaponModificationSystem.SelectedModifySlot] = -1;
					WeaponModificationSystem.SelectedModifySlot = -1;
				}
			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			Texture2D trashbin = TextureAssets.Trash.Value;
			Vector2 originTexture = texture.Size() * .5f;
			Vector2 origintrashbin = trashbin.Size() * .5f;
			spriteBatch.Draw(trashbin, new Vector2(Left.Pixels, Top.Pixels) + originTexture, null, Color.White, 0, origintrashbin, 1, SpriteEffects.None, 1);
		}
	}
	public class WeaponModificationWeaponUISlot : UIImage {
		Item item;
		Player player;
		public WeaponModificationWeaponUISlot(Asset<Texture2D> texture, Player eplayer) : base(texture) {
			player = eplayer;
		}
		public override void LeftMouseDown(UIMouseEvent evt) {
			if (Main.mouseItem.type != ItemID.None) {
				if (Main.mouseItem.consumable)
					return;
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				CreateModUISlot();
			}
			else {
				if (item == null)
					return;
				int count = Parent.Children.Count();
				for (int i = count - 1; i >= 0; i--) {
					UIElement child = Parent.Children.ElementAt(i);
					if (child is WeaponModificationUIslot wmslot) {
						if (wmslot.item == null)
							continue;
						if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
							globalItem.ModWeaponSlotType[wmslot.WhoAmI] = wmslot.ModificationType;
						}
					}
					if (child is WeaponModificationUIslot { item: not null } or ImprovisedUITextBox) {
						child.Deactivate();
						child.Remove();
					}
				}
				Main.mouseItem = item;
				item = null;
			}
		}
		private void CreateModUISlot() {
			if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
				Vector2 originDefault = new Vector2(26, 26);
				int maxLengthX = 550;
				for (int i = 0; i < globalItem.ModWeaponSlotType.Length; i++) {
					Vector2 offsetPos = player.Center + new Vector2(MathHelper.Lerp(-maxLengthX, 20 * globalItem.ModWeaponSlotType.Length - maxLengthX, i / (globalItem.ModWeaponSlotType.Length - 1f)), -100);
					offsetPos.X += 31 * i;
					WeaponModificationUIslot btn = new WeaponModificationUIslot(TextureAssets.InventoryBack2, player);
					btn.item = item;
					btn.UISetWidthHeight(52, 52);
					btn.UISetPosition(offsetPos, originDefault);
					btn.WhoAmI = i;
					Parent.Append(btn);
				}
				ImprovisedUITextBox itemtextbox = new ImprovisedUITextBox("");
				string lines = $"Item : {item.Name}\n";
				itemtextbox.SetTextMaxLength(1000);
				lines += globalItem.GetWeaponModificationStats();
				itemtextbox.ImprovisedUIpanel_Width = 80;
				itemtextbox.SetText(lines);
				itemtextbox.Recalculate();
				itemtextbox.IgnoresMouseInteraction = true;
				itemtextbox.ShowInputTicker = false;
				itemtextbox.UISetPosition(player.Center + new Vector2(100, 100), originDefault);
				Parent.Append(itemtextbox);
			}
		}
		public override void OnDeactivate() {
			if (item == null) {
				return;
			}
			for (int i = 0; i < 50; i++) {
				if (player.CanItemSlotAccept(player.inventory[i], item)) {
					player.inventory[i] = item;
					return;
				}
			}
			player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
		}
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			if (item != null) {
				Main.instance.LoadItem(item.type);
				Texture2D texture = TextureAssets.Item[item.type].Value;
				Vector2 origin = texture.Size() * .5f;
				spriteBatch.Draw(texture, new Vector2(Left.Pixels, Top.Pixels) + new Vector2(Width.Pixels, Height.Pixels) * .5f, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
			}
		}
	}
	public class WeaponModificationUIslot : UIImage {
		public int? WeaponModificationType = null;
		public Item item = null;
		public bool active = false;
		public int WhoAmI = -1;
		public int ModificationType = 0;
		Texture2D texture;
		public Texture2D textureDraw;
		Player player;
		public WeaponModificationUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
			this.player = player;
			this.texture = texture.Value;
		}
		public override void LeftClick(UIMouseEvent evt) {
			SelectState();
		}
		private void SelectState() {
			if (item == null) {
				WeaponModificationSystem.SelectedInventorySlot = WeaponModificationSystem.SelectedInventorySlot != WhoAmI ? WhoAmI : -1;
				if (WeaponModificationSystem.SelectedInventorySlot == WhoAmI) {
					WeaponModificationUIslot UIslot = (WeaponModificationUIslot)Parent.Children.Where(e => e is WeaponModificationUIslot { item: not null } slot && slot.WhoAmI == WeaponModificationSystem.SelectedModifySlot).FirstOrDefault();
					WeaponModificationPlayer modplayer = player.GetModPlayer<WeaponModificationPlayer>();
					if (UIslot == null)
						return;
					if (UIslot.item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
						int cache = modplayer.WeaponModification_inventory[WeaponModificationSystem.SelectedInventorySlot];
						modplayer.WeaponModification_inventory[WeaponModificationSystem.SelectedInventorySlot] = globalItem.ModWeaponSlotType[WeaponModificationSystem.SelectedModifySlot];
						globalItem.ModWeaponSlotType[WeaponModificationSystem.SelectedModifySlot] = cache;
						WeaponModificationSystem.SelectedModifySlot = -1;
						WeaponModificationSystem.SelectedInventorySlot = -1;
					}
					return;
				}
			}
			else {
				WeaponModificationSystem.SelectedModifySlot = WeaponModificationSystem.SelectedModifySlot != WhoAmI ? WhoAmI : -1;
				if (WeaponModificationSystem.SelectedModifySlot == WhoAmI) {
					if (WeaponModificationSystem.SelectedInventorySlot == -1)
						return;
					WeaponModificationPlayer modplayer = player.GetModPlayer<WeaponModificationPlayer>();
					if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
						int cache = globalItem.ModWeaponSlotType[WeaponModificationSystem.SelectedModifySlot];
						globalItem.ModWeaponSlotType[WeaponModificationSystem.SelectedModifySlot] = modplayer.WeaponModification_inventory[WeaponModificationSystem.SelectedInventorySlot];
						modplayer.WeaponModification_inventory[WeaponModificationSystem.SelectedInventorySlot] = cache;
						WeaponModificationSystem.SelectedModifySlot = -1;
						WeaponModificationSystem.SelectedInventorySlot = -1;
					}
					return;
				}
			}
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (IsMouseHovering && ModifierWeaponLoader.GetWeaponMod(ModificationType) != null)
				Main.NewText(ModifierWeaponLoader.GetWeaponMod(ModificationType).FullName);
			try {
				if (item != null) {
					if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
						if (globalItem != null) {
							ModificationType = globalItem.ModWeaponSlotType[WhoAmI];
						}
					}
				}
				else {
					ModificationType = player.GetModPlayer<WeaponModificationPlayer>().WeaponModification_inventory[WhoAmI];
				}
			}
			catch (Exception ex) {

			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
			if (item == null) {
				if (WeaponModificationSystem.SelectedInventorySlot == WhoAmI) {
					for (int i = 0; i < 4; i++) {
						spriteBatch.Draw(texture, drawpos, null, new Color(255, 255, 0, 30), 0, texture.Size() * .5f, 1.2f, SpriteEffects.None, 0);
					}
				}
			}
			else {
				if (WeaponModificationSystem.SelectedModifySlot == WhoAmI) {
					for (int i = 0; i < 4; i++) {
						spriteBatch.Draw(texture, drawpos, null, new Color(255, 255, 0, 30), 0, texture.Size() * .5f, 1.2f, SpriteEffects.None, 0);
					}
				}
			}
			base.Draw(spriteBatch);
			try {
				WeaponModificationPlayer modplayer = player.GetModPlayer<WeaponModificationPlayer>();
				if (ModifierWeaponLoader.GetWeaponMod(ModificationType) != null) {
					if (ModifierWeaponLoader.GetWeaponMod(ModificationType).ParticleTexture != null)
						textureDraw = ModContent.Request<Texture2D>(ModifierWeaponLoader.GetWeaponMod(ModificationType).ParticleTexture).Value;
					if (textureDraw != null) {
						spriteBatch.Draw(textureDraw, drawpos, null, Color.White, 0, textureDraw.Size() * .5f, 1, SpriteEffects.None, 0);
					}
				}
			}
			catch (Exception ex) {
				Main.NewText(ex.Message);
			}
		}
		public override void OnDeactivate() {
			item = null;
			WeaponModificationSystem.SelectedModifySlot = -1;
		}
	}
	public class WeaponModificationSystem : ModSystem {
		internal UserInterface userInterface;
		internal WeaponModificationUI WM_uiState;
		public static ModKeybind WeaponModificationKeybind { get; private set; }
		public static int SelectedInventorySlot = -1;
		public static int SelectedModifySlot = -1;
		public override void Load() {
			WeaponModificationKeybind = KeybindLoader.RegisterKeybind(Mod, "WeaponModification", "P");

			//UI stuff
			if (!Main.dedServ) {
				WM_uiState = new();
				userInterface = new();
			}
		}
		public override void Unload() {
			WeaponModificationKeybind = null;
		}
		public override void UpdateUI(GameTime gameTime) {
			userInterface?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (InventoryIndex != -1) {
				layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
					"BossRush: Weapon Modification",
					delegate {
						userInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
