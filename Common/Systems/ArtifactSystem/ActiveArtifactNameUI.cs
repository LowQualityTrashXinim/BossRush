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
using UtfUnknown.Core.Models.SingleByte.German;

namespace BossRush.Common.Systems.ArtifactSystem {
	internal class ActiveArtifactNameUI : UIElement {
		private Player Player { get; }
		public ActiveArtifactNameUI(Player player) {
			Player = player;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Artifact artifact = Artifact.GetArtifact(Player.ActiveArtifact());
			string text = artifact.DisplayName;
			Color color = artifact.DisplayNameColor;
			if (UniversalSystem.Check_TotalRNG()) {
				text = UniversalSystem.GetRandomGlitchyNameEffect(5);
				color = Main.DiscoColor;
			}
			DynamicSpriteFont font = FontAssets.MouseText.Value;
			Vector2 stringSize = font.MeasureString(text);
			float scale = GetDimensions().Width / font.MeasureString(text).X;
			ChatManager.DrawColorCodedStringWithShadow(
				spriteBatch,
				font,
				text,
				GetDimensions().Center(),
				color,
				0f,
				stringSize / 2f,
				Vector2.One * MathF.Min(scale, 1.4f)
			);
		}
	}
}
