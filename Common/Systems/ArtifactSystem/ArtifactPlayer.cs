using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Consumable.Potion;
using System.IO;

namespace BossRush.Common.Systems.ArtifactSystem {
	internal class ArtifactPlayer : ModPlayer {
		public int ActiveArtifact { get; set; } = Artifact.ArtifactType<NormalizeArtifact>();

		public override void OnEnterWorld() {
			if (UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE)) {
				while (ActiveArtifact == Artifact.ArtifactType<RandomArtifact>() && Artifact.GetArtifact(ActiveArtifact).CanBeSelected(Player)) {
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
		public void ReceivePlayerSync(BinaryReader reader) {
			ActiveArtifact = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ArtifactPlayer clone = (ArtifactPlayer)targetCopy;
			clone.ActiveArtifact = ActiveArtifact;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ArtifactPlayer clone = (ArtifactPlayer)clientPlayer;
			if (ActiveArtifact != clone.ActiveArtifact) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
