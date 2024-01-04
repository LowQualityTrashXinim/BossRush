using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts {
	internal class ManaOverloaderArtifact : Artifact {
		public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
		public override Color DisplayNameColor => Color.LimeGreen;
	}

	public class ManaOverloaderPlayer : ModPlayer {
		bool ManaOverLoader = false;
		public override void ResetEffects() {
			ManaOverLoader = Player.HasArtifact<ManaOverloaderArtifact>();
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			mana *= 2;
		}
		public override void PostUpdate() {
			if (ManaOverLoader) {
				if (Player.statMana == Player.statManaMax2 && Player.statLife > 1) {
					Player.statLife--;
				}
				else {
					Player.statMana++;
				}
			}
		}
	}
}
