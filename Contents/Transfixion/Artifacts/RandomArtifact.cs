using BossRush.Texture;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Transfixion.Artifacts {
	internal class RandomArtifact : Artifact {
		public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	}
}
