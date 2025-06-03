using BossRush.Common.Systems;
using BossRush.Contents.Items.RelicItem;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace BossRush {
	public static partial class BossRushUtils {
		//for real, who the fuk came up with these name
		public readonly static int ScreenWidth = Main.PendingResolutionWidth;
		public readonly static int ScreenHeight = Main.PendingResolutionHeight;
		public static void Disable_MouseItemUsesWhenHoverOverAUI(this UIElement el) {
			if (el.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}
		public static void UISetWidthHeight(this UIElement ui, float width, float height) {
			ui.Width.Pixels = width;
			ui.Height.Pixels = height;
		}
		public static void UISetPosition(this UIElement ui, Vector2 position, Vector2 origin) {
			Vector2 drawpos = position - Main.screenPosition - origin;
			ui.Left.Pixels = drawpos.X + (drawpos.X * (1 - Main.UIScale));
			ui.Top.Pixels = drawpos.Y + (drawpos.Y * (1 - Main.UIScale));
		}
		public static void UISetPosition(this UIElement ui, Vector2 position) {
			ui.Left.Pixels = position.X;
			ui.Left.Percent = 0;
			ui.Top.Pixels = position.Y;
			ui.Top.Percent = 0;
		}
		/// <summary>
		/// Code source credit : StructureHelper
		/// Draws the outline of a box in the style of the DragonLens GUI.
		/// </summary>
		/// <param name="spriteBatch">the spriteBatch to draw the outline with</param>
		/// <param name="target">where/how big the outline should be drawn</param>
		/// <param name="color">the color of the outline</param>
		public static void DrawOutline(SpriteBatch spriteBatch, Rectangle target, Color color = default) {
			Texture2D tex = ModContent.Request<Texture2D>("BossRush/Texture/StructureHelper_Box2").Value;

			if (color == default)
				color = new Color(49, 84, 141) * 0.9f;

			var sourceCorner = new Rectangle(0, 0, 6, 6);
			var sourceEdge = new Rectangle(6, 0, 4, 6);

			spriteBatch.Draw(tex, new Rectangle(target.X + 6, target.Y, target.Width - 12, 6), sourceEdge, color, 0, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X, target.Y - 6 + target.Height, target.Height - 12, 6), sourceEdge, color, -(float)Math.PI * 0.5f, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X - 6 + target.Width, target.Y + target.Height, target.Width - 12, 6), sourceEdge, color, (float)Math.PI, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X + target.Width, target.Y + 6, target.Height - 12, 6), sourceEdge, color, (float)Math.PI * 0.5f, Vector2.Zero, 0, 0);

			spriteBatch.Draw(tex, new Rectangle(target.X, target.Y, 6, 6), sourceCorner, color, 0, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X + target.Width, target.Y, 6, 6), sourceCorner, color, (float)Math.PI * 0.5f, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X + target.Width, target.Y + target.Height, 6, 6), sourceCorner, color, (float)Math.PI, Vector2.Zero, 0, 0);
			spriteBatch.Draw(tex, new Rectangle(target.X, target.Y + target.Height, 6, 6), sourceCorner, color, (float)Math.PI * 1.5f, Vector2.Zero, 0, 0);
		}
	}
	public class Roguelike_ProgressUIBar : UIElement {
		protected Asset<Texture2D> texture;
		private float barProgress;
		public bool Hide = false;
		public bool HideBar = false;
		public bool HideText = false;
		private int Delay = 0;
		public void DelayHide(int HideDelay) {
			if (Delay <= 0 && !Hide) {
				Delay = HideDelay;
			}
			else {
				Delay = BossRushUtils.CountDown(Delay);
				if (Delay <= 1) {
					Hide = true;
				}
			}
		}
		public Roguelike_ProgressUIBar(Asset<Texture2D> bartexture, Color starterColor, Color endColor, string textstring, float textscale = 1, bool isLarge = false) {
			if (bartexture == null) {
				barFrame = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI)); // Frame of our resource bar
			}
			else {
				barFrame = new UIImage(bartexture);
			}
			texture = bartexture;
			gradientA = starterColor;
			gradientB = endColor;
			text = new(textstring, textscale, isLarge);
			Append(text);
			Append(barFrame);
			if (HideBar) {
				barFrame.ImageScale = 0;
			}
		}
		public UIText text;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;

		public float BarProgress { get => barProgress; set => barProgress = value; }

		public void SetPosition(Rectangle barFrameRect, Rectangle textRect) {
			barFrame.UISetPosition(barFrameRect.TopLeft());
			barFrame.UISetWidthHeight(barFrameRect.Width, barFrameRect.Height);
			text.UISetPosition(textRect.TopLeft());
			text.UISetWidthHeight(textRect.Width, textRect.Height);
		}
		public void SetColorA(Color color) {
			gradientA = color;
		}
		public void SetColorB(Color color) {
			gradientB = color;
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (HideText) {
				text.SetText("");
			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.Draw(spriteBatch);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.DrawSelf(spriteBatch);
			DrawBarUI(spriteBatch);
		}
		private void DrawBarUI(SpriteBatch spriteBatch) {
			// Calculate quotient
			float quotient = barProgress; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Math.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
			hitbox.X += 12;
			hitbox.Width -= 24;
			hitbox.Y += 8;
			hitbox.Height -= 16;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1) {
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}
	}
	public class Roguelike_WrapTextUIPanel : UITextPanel<string> {
		public bool Hide = false;
		//Stole from ActiveArtifactDescriptionUI cause idk how to do text wrapping stuff
		private int linePosition;
		private int maxLinePosition;
		public int MAX_LINES = 0;
		public Vector2 offSetDraw = Vector2.Zero;
		public Roguelike_WrapTextUIPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			DynamicSpriteFont font = FontAssets.MouseText.Value;
			float scale = TextScale;
			string cachedText = Text;
			SetText("");
			this.Recalculate();
			base.DrawSelf(spriteBatch);
			string[] lines = Utils.WordwrapString(
				cachedText,
				font,
				(int)(this.GetInnerDimensions().Width * (1 + Math.Abs(1 - scale))),
				100,
			out int lineCount
			).Where(line => line is not null).ToArray();

			maxLinePosition = Math.Max(lines.Length - MAX_LINES, 0);
			linePosition = Math.Clamp(linePosition, 0, maxLinePosition);

			float yOffset = 0f;
			for (int i = 0; i < lines.Length; i++) {
				string text = lines[i];
				ChatManager.DrawColorCodedStringWithShadow(
					spriteBatch,
					font,
					text,
					GetInnerDimensions().Position() + offSetDraw + Vector2.UnitY * yOffset,
					Color.White,
					0f,
					Vector2.Zero,
					Vector2.One * scale
				);

				yOffset += scale * 25f;
			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.Draw(spriteBatch);
		}
		public override void ScrollWheel(UIScrollWheelEvent evt) {
			linePosition -= MathF.Sign(evt.ScrollWheelValue);
		}
	}
	public class Roguelike_UITextPanel : UITextPanel<string> {
		public bool Hide = false;
		public bool UseCustmSetHeight = false;
		public Roguelike_UITextPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			this.IgnoresMouseInteraction = Hide;
		}
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.DrawSelf(spriteBatch);
		}
		public override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			if (!UseCustmSetHeight) {
				Vector2 stringsize = ChatManager.GetStringSize(FontAssets.MouseText.Value, Text, Vector2.UnitY);
				Height.Pixels = stringsize.Y + 10;
			}
			base.Draw(spriteBatch);
		}
	}
	public class Roguelike_UIText : UIText {
		public bool Hide = false;
		public Roguelike_UIText(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		}
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.DrawSelf(spriteBatch);
		}
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.Draw(spriteBatch);
		}
	}
	public class Roguelike_UIPanel : UIPanel {
		public bool Hide = false;
		public Roguelike_UIPanel() {
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			this.IgnoresMouseInteraction = Hide;
		}
		public virtual void PreDraw(SpriteBatch spriteBatch) { }
		public virtual void PostDraw(SpriteBatch spriteBatch) { }
		protected sealed override void DrawSelf(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.DrawSelf(spriteBatch);
		}
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			PreDraw(spriteBatch);
			base.Draw(spriteBatch);
			PostDraw(spriteBatch);
		}
	}
	public class Roguelike_UIImage : UIImage {
		public bool Hide = false;
		public bool Highlight = false;
		public Color OriginalColor = Color.White;
		public Color HighlightColor = Color.White;
		/// <summary>
		/// Set this to have value if you want a specific texture to be drawn on top of it<br/>
		/// The drawing will be handle automatically
		/// </summary>
		public Asset<Texture2D> postTex = null;
		public Texture2D innerTex = null;
		bool _CustomWeirdDraw = false;
		public void SetPostTex(Asset<Texture2D> tex, bool CustomWeirdDraw = false) {
			postTex = tex;
			_CustomWeirdDraw = CustomWeirdDraw;
		}
		public void SwapHightlightColorWithOriginalColor() {
			Color origin = OriginalColor;
			OriginalColor = HighlightColor;
			HighlightColor = origin;
		}
		public Roguelike_UIImage(Asset<Texture2D> texture) : base(texture) {
			innerTex = texture.Value;
			OriginalColor = Color;
		}
		public override sealed void Update(GameTime gameTime) {
			base.Update(gameTime);
			this.IgnoresMouseInteraction = Hide;
			this.Disable_MouseItemUsesWhenHoverOverAUI();
			if (Highlight) {
				Color = HighlightColor;
			}
			else {
				Color = OriginalColor;
			}
			UpdateImage(gameTime);
		}
		public virtual void UpdateImage(GameTime gameTime) { }
		public virtual void DrawImage(SpriteBatch spriteBatch) { }
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}

			base.Draw(spriteBatch);
			DrawImage(spriteBatch);
			if (postTex != null) {
				Vector2 origin2 = innerTex.Size() * .5f;
				Vector2 drawpos = this.GetInnerDimensions().Position();
				Vector2 origin = postTex.Size() * .5f;
				if (_CustomWeirdDraw) {
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
					spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
				}
				else {
					spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
				}
			}
		}
	}
	public class Roguelike_UIImageButton : UIImageButton {
		/// <summary>
		/// Set this to have value if you want a specific texture to be drawn on top of it<br/>
		/// The drawing will be handle automatically
		/// </summary>
		public Asset<Texture2D> postTex = null;
		public Texture2D innerTex = null;
		public string HoverText = null;
		public void SetPostTex(Asset<Texture2D> tex) {
			postTex = tex;
		}
		public Roguelike_UIImageButton(Asset<Texture2D> texture) : base(texture) {
			innerTex = texture.Value;
		}
		public bool Hide = false;
		public override sealed void Update(GameTime gameTime) {
			base.Update(gameTime);
			this.IgnoresMouseInteraction = Hide;
			this.Disable_MouseItemUsesWhenHoverOverAUI();
			if (HoverText != null) {
				Main.instance.MouseText(HoverText);
			}
		}
		public virtual void DrawImage(SpriteBatch spriteBatch) { }
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			base.Draw(spriteBatch);
			DrawImage(spriteBatch);
			if (postTex != null) {
				Vector2 origin = postTex.Size() * .5f;
				Vector2 origin2 = innerTex.Size() * .5f;
				Vector2 drawpos = this.GetInnerDimensions().Position() + origin2;
				spriteBatch.Draw(postTex.Value, drawpos, null, new Color(255, 255, 255), 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
			}
		}
	}
	/// <summary>
	/// This is a framework of a UI menu<br/>
	/// This UI menu support paging and book type UI<br/>
	/// </summary>
	public abstract class Roguelike_UImenu<T> : UIElement {
		protected int CurrentPage = 0;
		protected List<Roguelike_UIImageButton> list_Btn = new();
		protected Roguelike_UIPanel panel = new();
		public bool Hide = false;
		public sealed override void OnInitialize() {
			panel = new();
			list_Btn = new();
			CurrentPage = 0;
			SetStaticDefaults();

			Append(panel);
		}
		public virtual void SetStaticDefaults() { }
		public void AddDataElement() {

		}
		public virtual void PreDraw(SpriteBatch spriteBatch) {

		}
		public virtual void PostDraw(SpriteBatch spriteBatch) {

		}
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if (Hide) {
				return;
			}
			PreDraw(spriteBatch);
			base.Draw(spriteBatch);
			PostDraw(spriteBatch);
		}
	}
	public class ExitUI : UIImageButton {
		Texture2D textureInner;
		public ExitUI(Asset<Texture2D> texture) : base(texture) {
			SetVisibility(.7f, 1f);
			textureInner = texture.Value;
		}

		public override void LeftClick(UIMouseEvent evt) {
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			SoundEngine.PlaySound(SoundID.MenuClose);
		}
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.CrossSprite).Value;
			Vector2 rect = this.GetDimensions().Position() + textureInner.Size() * .5f + Vector2.One;
			spriteBatch.Draw(texture, rect, null, Color.White, 0, textureInner.Size() * .5f, .7f, SpriteEffects.None, 0);
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			this.Disable_MouseItemUsesWhenHoverOverAUI();
			if (IsMouseHovering) {
				Main.instance.MouseText("Exit");
			}
		}
	}
	public class ItemHolderSlot : Roguelike_UIImage {
		private Texture2D texture;
		public Item item = new Item(0);
		public string Description = "";
		public bool DisplayOnHover = true;
		public ItemHolderSlot(Asset<Texture2D> texture) : base(texture) {
			this.texture = texture.Value;
		}
		public override void DrawImage(SpriteBatch spriteBatch) {
			if (item == null) {
				return;
			}
			if (item.type == 0) {
				return;
			}
			if (this.IsMouseHovering && DisplayOnHover) {
				if (!string.IsNullOrEmpty(Description)) {
					UICommon.TooltipMouseText(Description);
				}
				else {
					Main.HoverItem = item;
					Main.instance.MouseText("");
					Main.mouseText = true;
				}
			}
			Texture2D itemtexture;
			Color colorToDraw = Color.White;
			if (item.ModItem is Relic relic) {
				RelicPrefix relicprefix = RelicPrefixSystem.GetRelicPrefix(relic.RelicPrefixedType);
				if (relicprefix != null && !string.IsNullOrEmpty(relicprefix.TextureString)) {
					itemtexture = ModContent.Request<Texture2D>(relicprefix.TextureString).Value;
					colorToDraw = relic.GetRelicTierColor(colorToDraw);
				}
				else {
					itemtexture = TextureAssets.Item[item.type].Value;
				}
			}
			else {
				Main.instance.LoadItem(item.type);
				itemtexture = TextureAssets.Item[item.type].Value;
			}

			Vector2 origin = itemtexture.Size() * .5f;
			Vector2 FrameOrigin = texture.Size() * .5f;
			Vector2 DrawPos = this.GetInnerDimensions().Position() + FrameOrigin;
			spriteBatch.Draw(itemtexture, DrawPos, null, colorToDraw, 0, origin, BossRushUtils.Scale_OuterTextureWithInnerTexture(FrameOrigin, origin, .8f), SpriteEffects.None, 0);
		}
	}
}

