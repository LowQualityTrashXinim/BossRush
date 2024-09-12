using BossRush.Contents.Artifacts;
using Microsoft.CodeAnalysis.Text;
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
		public const int ARTIFACTS_MAX_LINES = 4;

		private int currentOffset = 0;

		private List<int> list_artifactInOrder = new();
		private List<ArtifactSelectionUIButton> list_btnArtifact = new();
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
			list_btnArtifact.Add(normalbutton);
			list_artifactInOrder.Add(Artifact.ArtifactType<NormalizeArtifact>());
			artifactSelectionElement.Append(normalbutton);
			int realtype = 0;
			int lineCounter = 0;
			for (int type = 1; type < Artifact.ArtifactCount; type++) {
				if (realtype == Artifact.ArtifactType<NormalizeArtifact>()) {
					realtype++;
				}
				if (type % 4 == 0) {
					lineCounter++;
				}
				if (lineCounter < ARTIFACTS_MAX_LINES) {

					ArtifactSelectionUIButton button = new(realtype, player) {
						Width = StyleDimension.FromPixels(44f),
						Height = StyleDimension.FromPixels(44f),
						Left = StyleDimension.FromPixels((type % ARTIFACTS_PER_ROW) * 46.0f + 6.0f),
						Top = StyleDimension.FromPixels((type / ARTIFACTS_PER_ROW) * 48.0f + 1.0f)
					};
					list_btnArtifact.Add(button);
					artifactSelectionElement.Append(button);
				}

				list_artifactInOrder.Add(realtype);
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
		public override void ScrollWheel(UIScrollWheelEvent evt) {
			currentOffset -= MathF.Sign(evt.ScrollWheelValue);
			currentOffset = Math.Clamp(currentOffset, 0, 1);

			int offsetvalue = currentOffset * ARTIFACTS_PER_ROW;
			int offsetlength = list_btnArtifact.Count - offsetvalue;
			for (int i = 0; i < list_btnArtifact.Count; i++) {
				int arty = Math.Clamp(i + offsetvalue, 0, Artifact.ArtifactCount - 1);
				list_btnArtifact[i].ChangeArtifactType(-1);
				if (i > offsetlength) {
					continue;
				}
				list_btnArtifact[i].ChangeArtifactType(list_artifactInOrder[arty]);
			}
		}
	}
}
