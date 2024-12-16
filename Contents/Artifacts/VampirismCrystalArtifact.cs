using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems;

namespace BossRush.Contents.Artifacts {
	internal class VampirismCrystalArtifact : Artifact {
		public override int Frames => 7;
		public override Color DisplayNameColor => Color.MediumVioletRed;
	}

	public class VampirePlayer : ModPlayer {
		bool Vampire = false;
		int cooldown = 0;
		public override void ResetEffects() {
			Vampire = Player.HasArtifact<VampirismCrystalArtifact>();
			cooldown = BossRushUtils.CountDown(cooldown);
			PlayerStatsHandle.SetSecondLifeCondition(Player,"VC", !Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire);
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Vampire) {
				health.Base = -(Player.statLifeMax * 0.55f);
			}
		}
		public override void PostUpdate() {
			if (Vampire) {
				Player.AddBuff(BuffID.PotionSickness, 600);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Vampire) {
				LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Vampire) {
				LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (PlayerStatsHandle.GetSecondLife(Player, "VC")) {
				Player.Heal(Player.statLifeMax2);
				Player.AddBuff(ModContent.BuffType<SecondChance>(), 18000);
				return false;
			}
			return true;
		}
		private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1) {
			if (cooldown > 0) {
				return;
			}
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) {
				cooldown = BossRushUtils.ToSecond(.25f);
				int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
				Player.Heal(HP);
			}
		}
	}
}
