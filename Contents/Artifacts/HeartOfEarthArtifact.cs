using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts {
	internal class HeartOfEarthArtifact : Artifact {
		public override Color DisplayNameColor => Color.Red;
	}

	class HeartOfEarthPlayer : ModPlayer {
		bool Earth = false;
		public override void ResetEffects() {
			Earth = Player.ActiveArtifact() == Artifact.ArtifactType<HeartOfEarthArtifact>();
		}
		int ShortStanding = 0;
		int OnHitDelay = 0;
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Earth) {
				health.Flat += 100;
				health += .1f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Earth) {
				damage *= Player.statLife / (float)Player.statLifeMax2;
			}
		}
		public override void PostUpdate() {
			if (!Earth || OnHitDelay > 0) {
				OnHitDelay = BossRushUtils.CountDown(OnHitDelay);
				return;
			}
			if (Player.velocity == Vector2.Zero) {
				ShortStanding++;
				if (ShortStanding > 120) {//2s required
					if (ShortStanding % Math.Clamp(10 - ShortStanding / 100, 1, 10) == 0) {
						Player.statLife = Math.Clamp(Player.statLife + 1, 0, Player.statLifeMax2);
					}
				}
			}
			else {
				ShortStanding = 0;
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if(!Earth) {
				return;
			}
			ShortStanding = (int)(ShortStanding * .75f);
			OnHitDelay = BossRushUtils.ToSecond(.5f);
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			if (!Earth) {
				return;
			}
			ShortStanding = (int)(ShortStanding * .75f);
			OnHitDelay = BossRushUtils.ToSecond(.5f);
		}
	}
}
