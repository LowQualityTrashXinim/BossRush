using BossRush.Texture;

namespace BossRush.Contents.Artifact {
	internal class RandomArtifactChooser : ArtifactModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void ArtifactSetDefault() {
			width = height = 32;
		}
	}
}