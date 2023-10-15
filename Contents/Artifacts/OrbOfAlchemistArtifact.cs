using BossRush.Common.Utils;
using BossRush.Contents.Items.Potion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Texture;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Artifacts {
	internal class OrbOfAlchemistArtifact : Artifact {
		public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
		public override Color DisplayNameColor => Color.PaleVioletRed;
	}

	class AlchemistKnowledgePlayer : ModPlayer {
		bool Alchemist = false;
		public override void ResetEffects() {
			Alchemist = Player.HasArtifact<OrbOfAlchemistArtifact>();
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			AlchemistOverCharged(target, ref modifiers);
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			AlchemistOverCharged(target, ref modifiers);
		}
		private void AlchemistOverCharged(NPC target, ref NPC.HitModifiers modifiers) {
			if (Alchemist) {
				int lengthNPC = target.buffType.Where(i => i != 0).Count();
				int lengthPlayer = Player.buffType.Where(i => i != 0).Count();
				modifiers.FinalDamage *= lengthNPC + Player.buffType.Where(i => i != 0).Count();
				if (lengthNPC > 0) {
					for (int i = target.buffType.Length - 1; i >= 0; i--) {
						if (target.buffType[i] == 0)
							continue;
						target.buffTime[i] = 0;
						target.DelBuff(i);
					}
				}
				if (lengthPlayer > 0) {
					for (int i = Player.buffType.Length - 1; i >= 0; i--) {
						if (Player.buffType[i] == 0)
							continue;
						Player.buffTime[i] = 0;
						Player.DelBuff(i);
					}
				}
			}
		}
	}
}
