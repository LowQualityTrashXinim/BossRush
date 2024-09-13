using Terraria;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ArtifactSystem;
using System.Collections.Generic;

namespace BossRush.Contents.Artifacts {
	internal class AlchemistKnowledgeArtifact : Artifact {
		public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
		public override Color DisplayNameColor => Color.PaleVioletRed;
	}
	class AlchemistKnowledgePlayer : ModPlayer {
		bool Alchemist = false;
		public override void ResetEffects() {
			Alchemist = Player.HasArtifact<AlchemistKnowledgeArtifact>();
		}
		public override void UpdateEquips() {
			if (!Alchemist) {
				return;
			}
			int lengthPlayer = Player.buffType.Where(i => i != 0).Count();
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.BuffTime -= .7f;
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: lengthPlayer * 1.5f);
			modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: lengthPlayer * 2);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: lengthPlayer);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: lengthPlayer * 0.5f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: lengthPlayer * .25f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: lengthPlayer * .25f);

			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: lengthPlayer * .06f + 1);
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: lengthPlayer * 0.085f + 1);
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, Additive: lengthPlayer * 0.15f + 1);
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			AlchemistOverCharged(target, ref modifiers);
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			AlchemistOverCharged(target, ref modifiers);
		}
		private void AlchemistOverCharged(NPC target, ref NPC.HitModifiers modifiers) {
			if (!Alchemist) {
				return;
			}
			int lengthNPC = target.buffType.Where(i => i != 0).Count();
			if (lengthNPC > 0) {
				modifiers.SourceDamage += .1f;
			}
			if (lengthNPC >= 3) {
				Player.Heal(lengthNPC - 3);
			}
			if (lengthNPC >= 5) {
				target.Center.LookForHostileNPC(out List<NPC> npclist, 150);
				int[] bufflist = target.buffType.Where(i => i != 0).ToArray();
				foreach (NPC npc in npclist) {
					Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Player.GetWeaponDamage(Player.HeldItem), 1));
					for (int i = 0; i < bufflist.Length; i++) {
						npc.AddBuff(bufflist[i], BossRushUtils.ToSecond(6));
					}
				}
			}
			//if (Alchemist) {
			//	int lengthNPC = target.buffType.Where(i => i != 0).Count();
			//	int lengthPlayer = Player.buffType.Where(i => i != 0).Count();
			//	int finalLength = lengthNPC + Player.buffType.Where(i => i != 0).Count();
			//	if (finalLength == 0) {
			//		modifiers.FinalDamage *= 0;
			//	}
			//	else {
			//		modifiers.FinalDamage *= finalLength;
			//	}
			//	if (lengthNPC > 0) {
			//		for (int i = target.buffType.Length - 1; i >= 0; i--) {
			//			if (target.buffType[i] == 0)
			//				continue;
			//			target.buffTime[i] = (int)(target.buffTime[i] * .5f);
			//		}
			//	}
			//	if (lengthPlayer > 0) {
			//		for (int i = Player.buffType.Length - 1; i >= 0; i--) {
			//			if (Player.buffType[i] == 0)
			//				continue;
			//			Player.buffTime[i] = (int)(Player.buffTime[i] * .5f);
			//		}
			//	}
			//}
		}
	}
}
