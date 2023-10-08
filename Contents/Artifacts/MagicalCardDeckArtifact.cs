using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Contents.Artifacts
{
    internal class MagicalCardDeckArtifact : Artifact
    {
        public override int Frames => 9;
		public override Color DisplayNameColor => Color.DeepSkyBlue;
	}

	class MagicalCardDeckPlayer : ModPlayer {
		public bool MagicalCardDeck = false;
		public override void ResetEffects() {
			MagicalCardDeck = Player.HasArtifact<MagicalCardDeckArtifact>();
		}
	}
}
