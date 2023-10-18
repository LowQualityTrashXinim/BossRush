using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.NohitReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Systems.ArtifactSystem;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfPrideArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.PaleGreen;
	}

	public class PridePlayer : ModPlayer {
		bool Pride = false; protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Pride = Player.HasArtifact<TokenOfPrideArtifact>();
		}
		public override void PostUpdate() {
			if (Pride) {
				chestmodplayer.finalMultiplier -= .5f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Pride) {
				float reward = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count * .1f;
				damage += .45f + reward;
			}
		}
	}
}
