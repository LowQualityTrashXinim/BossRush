using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfGreedArtifact : Artifact {
		public override Color DisplayNameColor => Color.Navy;
	}

	public class GreedPlayer : ModPlayer {
		bool Greed = false;
		public int ItemType = 0;
		public bool JustChangeWeapon = false;
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
		}
		public override void PostUpdate() {
			if (Greed) {
				chestmodplayer.amountModifier += 4;
			}
			if (Player.ItemAnimationActive) {
				ItemType = Player.HeldItem.type;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if(!Greed) {
				return;
			}
			if (item.type == ItemType) {
				damage *= .65f;
			}
		}
	}
}
