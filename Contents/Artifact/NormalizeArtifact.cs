using BossRush.Texture;

namespace BossRush.Contents.Artifact {
	internal class NormalizeArtifact : ArtifactModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void ArtifactSetDefault() {
			width = height = 32;
			Item.rare = 9;
		}
	}
}