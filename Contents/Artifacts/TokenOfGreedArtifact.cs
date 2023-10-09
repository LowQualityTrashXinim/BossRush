using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifacts
{
    internal class TokenOfGreedArtifact : Artifact
    {
		public override Color DisplayNameColor => Color.Navy;
	}

	public class GreedPlayer : ModPlayer {
		bool Greed = false; 
  		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
		}
		public override void PostUpdate() {
			if (Greed)
				chestmodplayer.amountModifier += 4;
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Greed)
				damage *= .65f;
		}
	}
}
