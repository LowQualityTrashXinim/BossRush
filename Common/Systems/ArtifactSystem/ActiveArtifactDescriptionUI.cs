using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BossRush.Common.Systems.ArtifactSystem {
	internal class ActiveArtifactDescriptionUI : UIElement {
		private Player Player { get; }
		public ActiveArtifactDescriptionUI(Player player) {
			Player = player;
		}

		private int linePosition;
		private int maxLinePosition;
		private const int MAX_LINES = 6;
		private int previousArtifactType = 0;
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			int currentArtifactType = Player.ActiveArtifact();
			if (currentArtifactType != previousArtifactType) {
				linePosition = 0;
			}
			previousArtifactType = currentArtifactType;
			Artifact artifact = Artifact.GetArtifact(currentArtifactType);

			string text = artifact.Description;
			if (UniversalSystem.Check_TotalRNG()) {
				text = "It doesn't matter what you choose, it will be random anyway";
			}


			DynamicSpriteFont font = FontAssets.MouseText.Value;
			float scale = 0.7f;
			string[] lines = Terraria.Utils.WordwrapString(
			   text,
				font,
				(int)(GetDimensions().Width / scale),
				999,
				out int lineCount
			).Where(line => line is not null).ToArray();

			maxLinePosition = Math.Max(lines.Length - MAX_LINES, 0);
			linePosition = Math.Clamp(linePosition, 0, maxLinePosition);

			float yOffset = 0f;
			for (int i = linePosition; i < Math.Min(linePosition + MAX_LINES, lines.Length); i++) {
				string text2 = lines[i];
				ChatManager.DrawColorCodedStringWithShadow(
					spriteBatch,
					font,
					text2,
					GetDimensions().Position() + Vector2.UnitY * yOffset,
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
}
