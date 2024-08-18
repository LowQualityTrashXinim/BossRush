using BossRush.Contents.Skill;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SteelSeries.GameSense;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

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
		protected override void DrawSelf(SpriteBatch spriteBatch) {
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
}
