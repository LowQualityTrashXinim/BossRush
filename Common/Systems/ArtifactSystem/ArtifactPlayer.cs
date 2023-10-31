using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Artifacts;

namespace BossRush.Common.Systems.ArtifactSystem {
	internal class ArtifactPlayer : ModPlayer {
		public int ActiveArtifact { get; set; } = 0;

		public override void OnEnterWorld() {
			if (Player.difficulty == PlayerDifficultyID.Hardcore) {
				while (ActiveArtifact == Artifact.ArtifactType<RandomArtifact>()) {
					ActiveArtifact = Main.rand.Next(Artifact.ArtifactCount);
				}
			}
			else {
				ActiveArtifact = Artifact.ArtifactType<NormalizeArtifact>();
			}
		}

		public override void SaveData(TagCompound tag) {
			tag["ActiveArtifact"] = ActiveArtifact;
		}

		public override void LoadData(TagCompound tag) {
			if (tag.TryGet("ActiveArtifact", out int value)) {
				ActiveArtifact = value;
			}
		}
	}
}
