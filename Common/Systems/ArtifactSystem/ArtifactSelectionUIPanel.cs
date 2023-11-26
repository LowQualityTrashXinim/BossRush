using BossRush.Contents.Artifacts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BossRush.Common.Systems.ArtifactSystem {
	internal class ArtifactSelectionUIPanel : UIPanel {
		public const int ARTIFACTS_PER_ROW = 4; // Set this up to 10 cuz reasons
		public ArtifactSelectionUIPanel(Player player, int height, int top) {
			Width = StyleDimension.FromPercent(1f);
			Height = StyleDimension.FromPixels(height);
			Top = StyleDimension.FromPixels(top);
			BackgroundColor = new Color(33, 43, 79) * 0.8f;

			UIElement artifactSelectionElement = new() {
				Width = StyleDimension.FromPercent(ARTIFACTS_PER_ROW / 10f),
				Height = StyleDimension.FromPercent(1f),
			};

			//I mess with your code, hope ya don't mind
			ArtifactSelectionUIButton normalbutton = new(Artifact.ArtifactType<NormalizeArtifact>(), player) {
				Width = StyleDimension.FromPixels(44f),
				Height = StyleDimension.FromPixels(44f),
				Left = StyleDimension.FromPixels(0 % ARTIFACTS_PER_ROW * 46.0f + 6.0f),
				Top = StyleDimension.FromPixels(0 / ARTIFACTS_PER_ROW * 48.0f + 1.0f)
			};
			artifactSelectionElement.Append(normalbutton);

			int realtype = 0;
			for (int type = 1; type < Artifact.ArtifactCount; type++) {
				if (realtype == Artifact.ArtifactType<NormalizeArtifact>()) {
					realtype++;
				}
				ArtifactSelectionUIButton button = new(realtype, player) {
					Width = StyleDimension.FromPixels(44f),
					Height = StyleDimension.FromPixels(44f),
					Left = StyleDimension.FromPixels((type % ARTIFACTS_PER_ROW) * 46.0f + 6.0f),
					Top = StyleDimension.FromPixels((type / ARTIFACTS_PER_ROW) * 48.0f + 1.0f)
				};

				artifactSelectionElement.Append(button);
				realtype = Math.Clamp(++realtype, 0, Artifact.ArtifactCount - 1);
			}

			Append(artifactSelectionElement);

			UIPanel activeArtifactInfoPanel = new() {
				Left = StyleDimension.FromPercent(ARTIFACTS_PER_ROW / 10f),
				Width = StyleDimension.FromPercent(1f - (ARTIFACTS_PER_ROW / 10f)),
				Height = StyleDimension.FromPercent(1f)
			};

			ActiveArtifactNameUI activeArtifactName = new(player) {
				Top = StyleDimension.FromPixels(-5f),
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels(50f)
			};

			activeArtifactInfoPanel.Append(activeArtifactName);

			ActiveArtifactDescriptionUI activeArtifactDescription = new(player) {
				Top = StyleDimension.FromPixels(45f),
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels(height - 50f)
			};

			activeArtifactInfoPanel.Append(activeArtifactDescription);

			Append(activeArtifactInfoPanel);
		}
	}
}
