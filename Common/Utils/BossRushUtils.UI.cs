using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Skill;
using BossRush.Texture;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using SteelSeries.GameSense;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BossRush {
	public static partial class BossRushUtils {
		//for real, who the fuk came up with these name
		public readonly static int ScreenWidth = Main.PendingResolutionWidth;
		public readonly static int ScreenHeight = Main.PendingResolutionHeight;

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
	}
	public class Roguelike_ProgressUIBar : UIElement {
		protected Asset<Texture2D> texture;
		private float barProgress;
		public bool Hide = false;
		private int Delay = 0;
		public void DelayHide(int HideDelay) {
			if(Delay <= 0 && !Hide) {
				Delay = HideDelay;
			}
			else {
				Delay = BossRushUtils.CountDown(Delay);
				if(Delay <= 1) {
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
		public override void Draw(SpriteBatch spriteBatch) {
			if(Hide) {
				return;
			}
			base.Draw(spriteBatch);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if(Hide) {
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
		//Stole from ActiveArtifactDescriptionUI cause idk how to do text wrapping stuff
		private int linePosition;
		private int maxLinePosition;
		private const int MAX_LINES = 6;
		public Roguelike_WrapTextUIPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			DynamicSpriteFont font = FontAssets.MouseText.Value;
			float scale = 1;
			string cachedText = Text;
			SetText("");
			base.Recalculate();
			base.DrawSelf(spriteBatch);
			string[] lines = Utils.WordwrapString(
				cachedText,
				font,
				430,
				100,
			out int lineCount
			).Where(line => line is not null).ToArray();

			maxLinePosition = Math.Max(lines.Length, 0);
			linePosition = Math.Clamp(linePosition, 0, 100);

			float yOffset = 0f;
			for (int i = linePosition; i < lines.Length; i++) {
				string text = lines[i];
				ChatManager.DrawColorCodedStringWithShadow(
					spriteBatch,
					font,
					text,
					GetInnerDimensions().Position() + Vector2.UnitY * yOffset,
					Color.White,
					0f,
					Vector2.Zero,
					Vector2.One * scale
				);

				yOffset += scale * 25f;
			}
		}
		public override void ScrollWheel(UIScrollWheelEvent evt) {
			linePosition -= MathF.Sign(evt.ScrollWheelValue);
		}
	}
	class Roguelike_UIImage : UIImage {
		public bool Hide = false;
		public Roguelike_UIImage(Asset<Texture2D> texture) : base(texture) {
		}
		public virtual void DrawImage(SpriteBatch spriteBatch) { }
		public sealed override void Draw(SpriteBatch spriteBatch) {
			if(Hide) {
				return;
			}
			base.Draw(spriteBatch);
			DrawImage(spriteBatch);
		}
	}
}
