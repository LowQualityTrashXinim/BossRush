using BossRush.Contents.BuffAndDebuff;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Enraged {
	internal class EnragedPlayer : ModPlayer {
		public bool Enraged = false;
		public override void PostUpdate() {
			EnragedBoss();
		}
		private void EnragedBoss() {
			if (!ModContent.GetInstance<BossRushModConfig>().Enraged && !Enraged) {
				return;
			}
			//Enraged here
			if (NPC.AnyNPCs(NPCID.MoonLordCore)) {
				Player.AddBuff(ModContent.BuffType<MoonLordWrath>(), 5);
				Player.AddBuff(BuffID.PotionSickness, 5);
			}
			if (NPC.AnyNPCs(NPCID.KingSlime)) {
				Player.AddBuff(BuffID.Slimed, 5);
				Player.AddBuff(ModContent.BuffType<KingSlimeRage>(), 5);
			}
			if (NPC.AnyNPCs(NPCID.EyeofCthulhu)) {
				Player.AddBuff(BuffID.Blackout, 5);
				Player.AddBuff(BuffID.Darkness, 5);
				Player.AddBuff(ModContent.BuffType<EvilPresence>(), 5);
			}
			if (NPC.AnyNPCs(NPCID.BrainofCthulhu)) {
				Player.AddBuff(ModContent.BuffType<MindBreak>(), 5);
			}
			if (
				(NPC.AnyNPCs(NPCID.EaterofWorldsHead)
				|| NPC.AnyNPCs(NPCID.EaterofWorldsBody)
				|| NPC.AnyNPCs(NPCID.EaterofWorldsTail))) {
				if (Player.ZoneOverworldHeight) {
					Player.AddBuff(BuffID.CursedInferno, 120);
					Player.AddBuff(ModContent.BuffType<Rotting>(), 5);
				}
			}
			if (NPC.AnyNPCs(NPCID.QueenBee)) {
				if (Player.ZoneJungle) {
					Player.AddBuff(BuffID.Poisoned, 90);
				}
				Player.AddBuff(ModContent.BuffType<RoyalAntiEscapeTm>(), 5);
			}
			if (!BossRushUtils.IsAnyVanillaBossAlive()) {
				Enraged = false;
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (!ModContent.GetInstance<BossRushModConfig>().Enraged && !Enraged) {
				return;
			}
			if (NPC.AnyNPCs(NPCID.BrainofCthulhu)) {
				Player.AddBuff(BuffID.PotionSickness, 240);
			}
		}
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			base.Kill(damage, hitDirection, pvp, damageSource);
			Enraged = false;
		}
	}
}