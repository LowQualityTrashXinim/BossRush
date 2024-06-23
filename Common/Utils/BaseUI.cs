using Terraria;
using Terraria.UI;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
namespace BossRush.Common.Utils;

/// <summary>
/// This is made with the purpose of allowing the modder to do whatever they want with the UI<br/>
/// However there are ofc limitation to what this can allow modder to do<br/>
/// Be aware that this doesn't mean the UI is faster or a lot easier to customize, it just mean it is a lot more central
/// </summary>
internal class BaseUI {
	public class BR_UI_Element : UIElement {
		public bool HideElement = false;
		public override void Draw(SpriteBatch spriteBatch) {
			if (HideElement) {
				return;
			}
			base.Draw(spriteBatch);
		}
	}
	public class BR_UI_UIImage : BR_UI_Element {
		public float ImageScale = 1f;
		public float Rotation;
		public bool ScaleToFit;

		public Color Color = Color.White;

		public bool RemoveFloatingPointsFromDrawPosition;

		private Asset<Texture2D> _texture;
		private Texture2D _nonReloadingTexture;

		public BR_UI_UIImage(Asset<Texture2D> texture) {
			SetImage(texture);
		}

		public BR_UI_UIImage(Texture2D nonReloadingTexture) {
			SetImage(nonReloadingTexture);
		}

		public void SetImage(Asset<Texture2D> texture) {
			_texture = texture;
			_nonReloadingTexture = null;

			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		public void SetImage(Texture2D nonReloadingTexture) {
			_texture = null;
			_nonReloadingTexture = nonReloadingTexture;

			Width.Set(_nonReloadingTexture.Width, 0f);
			Height.Set(_nonReloadingTexture.Height, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			CalculatedStyle dimensions = GetDimensions();
			Texture2D texture2D = null;
			if (_texture != null)
				texture2D = _texture.Value;

			if (_nonReloadingTexture != null)
				texture2D = _nonReloadingTexture;

			if (ScaleToFit) {
				spriteBatch.Draw(texture2D, dimensions.ToRectangle(), Color);
				return;
			}

			Vector2 vector = texture2D.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) * .5f + vector;

			vector2 = vector2.Floor();

			spriteBatch.Draw(texture2D, vector2, null, Color, Rotation, vector, ImageScale, SpriteEffects.None, 0f);
		}
	}

}
