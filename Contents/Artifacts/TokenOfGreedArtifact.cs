using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfGreedArtifact : Artifact {
		public override Color DisplayNameColor => Color.Navy;
	}

	public class GreedPlayer : ModPlayer {
		bool Greed = false;
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
		}
		public override void PostUpdate() {
			if (Greed) {
				chestmodplayer.DropModifier += 1;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if(!Greed) {
				return;
			}
			if (Player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
				damage *= .85f;
			}
		}
	}
}
